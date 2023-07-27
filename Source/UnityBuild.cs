using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using BCT.Source.Model;

namespace BCT.Source
{
    public static class UnityBuild
    {
        public const int MaxFilesInOneUnityDefault = 150;
        public const string UnityBuildDefine = "UNITY_BUILD_ENABLED=1";

        private const int LinesPerOneInclude = 4;
        private const int HeaderLinesCount = 2;

        #region public
        public static bool IsUnityBuildNeeded(UnityBuildType unityBuildType, bool useUnityBuild)
        {
            switch ( unityBuildType )
            {
                case UnityBuildType.NEVER:
                    return false;
                case UnityBuildType.ALWAYS:
                    return true;
                case UnityBuildType.DEFINED_BY_USER:
                    return useUnityBuild;
            }
            throw new NotSupportedException();
        }

        public static ICollection<FileDesc> Create(ICollection<FileDesc> files, ProjectFile project)
        {
            if (project.UseUnityBuild)
            {
                Log.Info("Unity build for {0}", project.projectName);
                files = Create(
                    files,
                    project.ProjectFullPath,
                    project.MaxFilesInOneUnity,
                    project.UnityBuildIngoreList,
                    project.usePrecompiledHeaders,
					project.language );
            }
            else
            {
                Cleanup(project.ProjectFullPath);
            }
            return files;
        }

        public static ICollection<FileDesc> Create(ICollection<FileDesc> files, string unityFilesLocation, int maxFilesInOneUnity, List<Regex> ignoreList, bool precompiledHeadersEnabled, Language lang)
        {
            var result = new List<FileDesc>();

            //для каждого возможной комбинации конфигов, свои инклюдеры
            var globalIncluders = new Dictionary<FileDescConfiguration, List<FileDesc>>(new FileDescConfiguration.EqualityComparer());

            var sortedFiles = files.OrderBy(x => x.fileName);

            int includerIndex = 0;
            foreach (FileDesc file in sortedFiles)
            {
                if (!IsUnityBuildFile(file.fileName, ignoreList))
                {
                    file.customFileLines = null;
                    result.Add(file);
                    continue;
                }

                List<FileDesc> allIncludersForConfig;
                if (globalIncluders.TryGetValue(file.config, out allIncludersForConfig) == false)
                {
                    allIncludersForConfig = new List<FileDesc>();
                    globalIncluders.Add(new FileDescConfiguration(file.config), allIncludersForConfig);
                }

                FileDesc includerFile = null;
                if (allIncludersForConfig.Count > 0)
                {
                    //берем самый последний
                    includerFile = allIncludersForConfig[allIncludersForConfig.Count - 1];

                    //уже слишком много инклюдов в этом файле, заводим следующий...
                    int includedFilesCount = (includerFile.customFileLines.Count - HeaderLinesCount) / LinesPerOneInclude;
                    if (includedFilesCount > maxFilesInOneUnity)
                    {
                        includerFile = null;
                    }
                }

                //не нашли подходящий файл инклюдер - заводим новый...
                if (includerFile == null)
                {
                    //создаем файл
                    includerFile = new FileDesc();
                    includerFile.customFileLines = new List<string> { "// Generated unity build file. Do not commit to repository!" };

                    if (precompiledHeadersEnabled)
                    {
						var stdafx = (lang == Language.CPP) ? "PCH_NAME" : "\"stdafx.h\"";
                        includerFile.customFileLines.Add("#include " + stdafx);
                    }
                    else
                    {
                        includerFile.customFileLines.Add("// Precompiled headers disabled for this project.");
                    }
                    includerFile.config = new FileDescConfiguration(file.config);
                    includerFile.fileName = string.Format("UnityBuild_{0}.cpp", includerIndex);
                    allIncludersForConfig.Add(includerFile);
                    includerIndex++;
                }


                //добавляем файл в текущий инклюдер
                includerFile.customFileLines.Add("#undef __UNIQUE_CPP_ID__");
                includerFile.customFileLines.Add(String.Format("#define __UNIQUE_CPP_ID__ {0}", GetCppGuidFromString(file.fileName)));
                includerFile.customFileLines.Add(String.Format("#pragma message(\" - {0}\")", file.fileName.Replace("\\", "/")));
                includerFile.customFileLines.Add(String.Format("#include \"{0}\"", file.fileName));


                //сам файл исключен во всех конфигурациях билда (но в проекте есть, что бы интелисенс его видел)
                file.config.excludedConfigurations.AddRange(file.config.includedConfigurations);
                file.config.includedConfigurations.Clear();
                file.customFileLines = null;
                result.Add(file);
            }


            foreach (KeyValuePair<FileDescConfiguration, List<FileDesc>> configIncluders in globalIncluders)
            {
                result.AddRange(configIncluders.Value);
            }


            int filesCount = 0;
            foreach (FileDesc resultFile in result)
            {
                if (resultFile.customFileLines == null || resultFile.customFileLines.Count <= HeaderLinesCount)
                {
                    continue;
                }

                string unityFileName = unityFilesLocation + resultFile.fileName;

                using (var memoryStream = new MemoryStream())
                {
                    using (var output = new StreamWriter(memoryStream))
                    {
                        foreach (string s in resultFile.customFileLines)
                        {
                            output.WriteLine(s);
                        }

                        output.Flush();

                        Utilites.SaveFileIfChanged(unityFileName, memoryStream);

                        filesCount++;
                    }
                }
            }

            Cleanup(unityFilesLocation, filesCount);

            return result;
        }

        public static void Cleanup(string unityFilesLocation, int firstIndex = 0)
        {
            if (!Directory.Exists(unityFilesLocation))
                return;

            string[] unityBuildFiles = Directory.GetFiles(unityFilesLocation, "UnityBuild_*.cpp");
            foreach (string fileName in unityBuildFiles)
            {
                string name = Path.GetFileNameWithoutExtension(fileName);

                int fileIndex = -1;
                int.TryParse(name.Substring(11), out fileIndex);

                if (fileIndex >= firstIndex)
                {
                    Log.Info(string.Format("Delete file '{0}'", fileName));
                    File.Delete(fileName);
                }
            }
        }
        
        public static Regex GetIgnoreFileMaskRegex(string fileMask)
        {
            var regexPattern = '^' + Regex.Escape(
                fileMask.Replace(".", "__DOT__")
                        .Replace("*", "__STAR__")
                        .Replace("?", "__QM__"))
                        .Replace("__DOT__", "[.]")
                        .Replace("__STAR__", ".*")
                        .Replace("__QM__", ".") + '$';
            var fileMaskRegex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            return fileMaskRegex;
        }
        #endregion

        #region private
        private static bool IsUnityBuildFile(string fileName, List<Regex> ignoreList)
        {
            if (fileName.EndsWith(".c") || fileName.EndsWith(".cpp"))
            {
                if (!Utilites.IsPrecompiledHeaderFile(fileName))
                {
                    //проверяем с ignore list
                    if (ignoreList != null)
                    {
                        foreach (Regex wildcard in ignoreList)
                        {
                            if (wildcard.IsMatch(fileName))
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private static string GetCppGuidFromString(string inputStr)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(inputStr);
            var hash = md5.ComputeHash(inputBytes);

            string r = "";
            for (int i = 0; i < hash.Length; i++)
            {
                r += hash[i].ToString();
            }

            return r;
        }
        #endregion
    }
}
