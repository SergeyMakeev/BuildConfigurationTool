using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using BCT.Source.Model;

namespace BCT.Source
{
    /////////////////////////////////////////////////////////////////////////////////////////////
    public class FileDescConfiguration
    {
        public List<ProjectFile> excludedConfigurations = new List<ProjectFile>();
        public List<ProjectFile> includedConfigurations = new List<ProjectFile>();

        public FileDescConfiguration()
        {

        }

        public FileDescConfiguration(FileDescConfiguration other)
        {
            excludedConfigurations.AddRange(other.excludedConfigurations);
            includedConfigurations.AddRange(other.includedConfigurations);
        }


        public class EqualityComparer : IEqualityComparer<FileDescConfiguration>
        {

            public bool Equals(FileDescConfiguration x, FileDescConfiguration y)
            {
                if (x.excludedConfigurations.Count != y.excludedConfigurations.Count)
                {
                    return false;
                }

                if (x.includedConfigurations.Count != y.includedConfigurations.Count)
                {
                    return false;
                }

                for (int i = 0; i < x.excludedConfigurations.Count; i++)
                {
                    if (x.excludedConfigurations[i] != y.excludedConfigurations[i])
                    {
                        return false;
                    }
                }

                for (int i = 0; i < x.includedConfigurations.Count; i++)
                {
                    if (x.includedConfigurations[i] != y.includedConfigurations[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(FileDescConfiguration obj)
            {
                return obj.excludedConfigurations.Count | (obj.includedConfigurations.Count << 16);
            }
        }



    }

	/////////////////////////////////////////////////////////////////////////////////////////////
	public class FileDesc
	{
        public FileDescConfiguration config = new FileDescConfiguration();
        public string fileName;

        //если содержимое файла генерируетс€, строчки лежат тут
        public List<string> customFileLines;
	}

	public class CustomBuildDesc : FileDesc
	{
		public string Command { get; set; }
		public string Outputs { get; set; }
	}

	public class FileDescComparator
	{
		private readonly string fileName;

		public FileDescComparator( string _fileName )
		{
			fileName = _fileName;
		}

		public bool IsEqual( FileDesc fileDesc )
		{
			return fileDesc.fileName == fileName;
		}
	}

	public static class Utilites
	{
		static string targetDir = "";

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string GetCurrentDirectory()
		{
			var currentDir = Directory.GetCurrentDirectory();
			currentDir = FixupTrailingSlash( currentDir );
			return currentDir;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static void SetTargetDirectory( string dir )
		{
			dir = FixupTrailingSlash( dir );
			targetDir = dir;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string FixupTrailingSlash( string s )
		{
			s = s.TrimEnd( '/', '\\' ) + '\\';
			return s;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string FixupSlashes( string s )
		{
			if ( s == null )
				return null;

			return s.Replace( '/', '\\' );
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string FixupSlashesUnixStyle(string s)
		{
			if (s == null)
				return null;

			return s.Replace('\\', '/');
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string GetTargetDirectory()
		{
			return targetDir;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static Regex ConvertMaskToRegex( string wildcard )
		{
			wildcard = "*" + FixupSlashes( wildcard );
			var expression = "^" + Regex.Escape( wildcard ).Replace( @"\*", ".*" ).Replace( @"\?", "." ) + "$";

			var mask = new Regex( expression, RegexOptions.IgnoreCase );
			return mask;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static List<string> GetFilesByMask( string fileMask, bool recurseSearch)
		{
			var currentDir = GetCurrentDirectory();

			var lastIndex = fileMask.LastIndexOfAny( new[] { '\\', '/' } );

			var path = currentDir;
			if ( lastIndex >= 0 )
			{
				path = fileMask.Substring( 0, lastIndex + 1 );
				fileMask = fileMask.Substring( lastIndex + 1, fileMask.Length - lastIndex - 1 );
			}

			IEnumerable<string> dirFiles = null;
			try
			{
				//
                dirFiles = Directory.EnumerateFiles(path, fileMask, recurseSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                //
			}
			catch ( DirectoryNotFoundException ) {}

			var res = new List<string>();
            if (dirFiles != null)
            {
                res.AddRange(dirFiles.Select(FixupSlashes));
            }

            //TODO: переместить в UnityBuild.cs
            //удал€ем файлы юнити билда из поиска
            //они там могли с прошлого раза остатьс€, их надо не замечать
            for(int i = 0; i < res.Count; i++)
            {
                if (res[i].IndexOf("UnityBuild_", StringComparison.Ordinal) >= 0)
                {
                    res.RemoveAt(i);
                    i--;
                }
            }
			return res;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string RelativePath( string fromPath, string toPath )
		{
			Uri fromUri, toUri;
			try
			{
				fromUri = new Uri(fromPath);
				toUri = new Uri(toPath);
			}
			catch (System.UriFormatException)
			{
				// path can't be made relative.
				return toPath;
			}
			if ( fromUri.Scheme != toUri.Scheme )
			{
				// path can't be made relative.
				return toPath;
			}

			var relativeUri = fromUri.MakeRelativeUri( toUri );
			var relativePath = Uri.UnescapeDataString( relativeUri.ToString() );

			if ( toUri.Scheme.ToUpperInvariant() == "FILE" )
				relativePath = relativePath.Replace( Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar );

            relativePath = relativePath.TrimStart('\\', '/');

			if ( string.IsNullOrEmpty( relativePath ) )
				return ".\\";

			return relativePath;
		}

		public static ICollection<TOut> MergeConfigs<TIn, TOut>( List<ProjectFile> configs,
																														 Func<ProjectFile, IEnumerable<TIn>> getter,
																														 Func<TIn, string> keyGetter,
																														 Func<TIn, TOut> creator )
			where TOut: FileDesc
		{
			//тут надо помержить файлы из всех проектов
			//и которые не надо сделать ExcludedFromBuild свойство
			var fileDict = new Dictionary<string, TOut>();

			foreach ( var project in configs )
			{
				foreach ( var desc in getter( project ) )
				{
					var key = keyGetter( desc );

					TOut fileDesc;
					if ( !fileDict.TryGetValue( key, out fileDesc ) )
					{
						fileDesc = creator( desc );

						fileDesc.config.excludedConfigurations.AddRange( configs );
						fileDict.Add( key, fileDesc );
					}

                    fileDesc.config.excludedConfigurations.Remove(project);
                    fileDesc.config.includedConfigurations.Add(project);
				}
			}

			return fileDict.Values;
		}



        public static bool IsCppCliSourceFile(string fileName)
        {
            if (fileName.EndsWith(".cxx"))
                return true;

            return false;
        }


        public static bool IsCppSourceFile(string fileName)
        {
            if (fileName.EndsWith(".c") || fileName.EndsWith(".cpp"))
                return true;

            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        public static bool IsPrecompiledHeaderFile(string fileName)
        {
            return fileName.IndexOf("stdafx.c", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        public static bool StreamEquals(Stream stream1, Stream stream2)
        {
            stream1.Position = 0;
            stream2.Position = 0;

            if (stream1.Length != stream2.Length)
                return false;

            const int bufferSize = 2048;
            var buffer1 = new byte[bufferSize]; //buffer size
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var count1 = stream1.Read(buffer1, 0, bufferSize);
                var count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return false;

                if (count1 == 0)
                    return true;

                if (!StructuralComparisons.StructuralEqualityComparer.Equals(buffer1, buffer2))
                    return false;
            }

            //return true;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        public static bool SaveFileIfChanged(string fileName, MemoryStream data)
        {
            //FileInfo fi = new FileInfo(fileName);

            var fileIsObsolete = true;

            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (StreamEquals(fileStream, data))
                        fileIsObsolete = false;
                }
            }

            if (fileIsObsolete)
            {
                var dirName = Path.GetDirectoryName(fileName) ?? string.Empty;
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    Log.Info(string.Format("Update file '{0}'", fileName));
                    data.Position = 0;
                    data.CopyTo(fileStream);
                }
            }

            return true;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////
		public static ICollection<CustomBuildDesc> GetCustomBuildResource( List<ProjectFile> projectConfigurations )
		{
			return MergeConfigs( projectConfigurations,
													 x => x.GetCustomBuild(),
													 x => x.name,
													 x => new CustomBuildDesc
																{
																	fileName = x.name,
																	Command = x.command,
																	Outputs = x.outputs
																} );
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static ICollection<FileDesc> GetSourceFiles( List<ProjectFile> projectConfigurations )
		{
		    var project = projectConfigurations.First();
			var files = MergeConfigs( projectConfigurations,
                                                     x => x.GetFiles(project.ProjectFullPath),
													 x => x,
													 x => new FileDesc { fileName = x } );
		    files = UnityBuild.Create( files, project );
		    return files;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		public static ICollection<FileDesc> GetResourceFiles( List<ProjectFile> projectConfigurations, string parentDir )
		{
			return MergeConfigs( projectConfigurations,
													 x => x.GetResourceFiles( parentDir ),
													 x => x,
													 x => new FileDesc { fileName = x } );
		}


		/////////////////////////////////////////////////////////////////////////////////////////////
		public static string GetGuidFromString( string inputStr )
		{
			var md5 = MD5.Create();

			var inputBytes = Encoding.ASCII.GetBytes( inputStr );
			var hash = md5.ComputeHash( inputBytes );

			var guid = new byte[16];
			for ( var i = 0; i < guid.Length; i++ )
				guid[i] = 0;

			var stepsCount = hash.Length;

			if ( stepsCount > guid.Length )
				stepsCount = guid.Length;

			for ( var i = 0; i < stepsCount; i++ )
				guid[i] = hash[i];

			guid[1] = (byte)( guid[1] ^ guid[0] );

			guid[0] = (byte)inputStr.Length;

			var res = string.Format( "{0:X2}{1:X2}{2:X2}{3:X2}-{4:X2}{5:X2}-{6:X2}{7:X2}-{8:X2}{9:X2}-{10:X2}{11:X2}{12:X2}{13:X2}{14:X2}{15:X2}",
															 guid[15], guid[14], guid[13], guid[12], guid[11], guid[10], guid[9], guid[8], guid[7], guid[6], guid[5], guid[4], guid[3],
															 guid[2], guid[1], guid[0] );
			return res;
		}

		public static string GetPlatformName( PlatformType platform )
		{
			switch ( platform )
			{
				case PlatformType.Win64:
					return "x64";
				case PlatformType.Win32:
					return "Win32";
				case PlatformType.Durango:
					return "Durango";
                case PlatformType.Orbis:
                    return "ORBIS";
				default:
					throw new NotSupportedException();
			}
		}

		public static string GetPlatformNameSharp( PlatformType platform )
		{
			switch ( platform )
			{
				case PlatformType.Win64:
					return "x64";
				case PlatformType.Win32:
					return "x86";
				case PlatformType.Durango:
					return "Durango";
				default:
					throw new NotSupportedException();
			}
		}

		public static string GetConfigurationName( Configuration config )
		{
			return config.name;
		}

		public static string GetVendorsConfigurationName( Configuration config )
		{
			switch ( config.target )
			{
				case Configuration.Target.DEBUG:
					return "Debug";
				case Configuration.Target.RELEASE:
				case Configuration.Target.FINALRELEASE:
					return "Release";
				default:
					throw new NotSupportedException();
			}
		}

        public static string GetResultFolder()
        {
            return "Build/" + VSVersion.CurrentVersion;
        }

		public static string GetOutputDir( Configuration configuration, PlatformType platform )
		{
			return GetResultFolder() + "/Bin/" + GetConfigurationName( configuration ) + "_" + GetPlatformName( platform ) + "\\";
		}

        public static string GetOutputDirFullPath(Configuration configuration, PlatformType platform)
        {
            return Path.GetFullPath( FixupTrailingSlash( GetTargetDirectory() ) + GetOutputDir( configuration, platform ) );
        }

		public static string GetTempBuildDir( Configuration configuration, PlatformType platform )
		{
			return GetResultFolder() + "/Temp/" + GetConfigurationName( configuration ) + "_" + GetPlatformName( platform ) + "\\";
		}

        public static string GetTempBuildDirFullPath(Configuration configuration, PlatformType platform)
        {
            return Path.GetFullPath(FixupTrailingSlash(GetTargetDirectory()) + GetTempBuildDir(configuration, platform));
        }
		
        // ReSharper disable InconsistentNaming
        public enum FileType
        {
            SOURCE,
            HEADER,

            CS_SOURCE,
            XAML,

            RESX,

            APPXMANIFEST,
            IMAGE,
            DLL,

            OTHER
        }

        public static FileType GetFileType(string fileName)
        {
            if (IsCppSourceFile(fileName))
                return FileType.SOURCE;

            if (fileName.EndsWith(".cxx"))
                return FileType.SOURCE;

            if (fileName.EndsWith(".h") || fileName.EndsWith(".hpp") || fileName.EndsWith(".hxx") || fileName.EndsWith(".inl"))
                return FileType.HEADER;

            if (fileName.EndsWith(".cs"))
                return FileType.CS_SOURCE;

            if (fileName.EndsWith(".xaml"))
                return FileType.XAML;

            if (fileName.EndsWith(".resx") || fileName.EndsWith(".licx"))
                return FileType.RESX;

            if ( fileName.EndsWith( ".dll" ) )
                return FileType.DLL;

            return FileType.OTHER;
        }

        public static void DeleteFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return;
            try
            {
                Directory.Delete(folderPath, true);
                Log.Info(string.Format("[ OK  ] remove->{0}", FixupSlashes( folderPath )));
            }
            catch ( Exception ex)
            {
                Log.Error(string.Format("[ ERR ] remove->{0}", FixupSlashes(folderPath)));
                Log.Error(ex.Message);
            }
        }

        public static void CopyExtraBinResources(string extraBinResourcesRootFolder, Configuration configuration, PlatformType platform, bool sync = false)
        {
			var targetConfigurationName = configuration.GetTargetConfigurationName();
			if ( string.IsNullOrEmpty( targetConfigurationName ) )
			{
				Log.Warning("Can't get TargetConfigurationName for configuration {0} ", configuration);
				return;
			}
			var source = Path.GetFullPath(FixupTrailingSlash(extraBinResourcesRootFolder) + GetPlatformName(platform) + "//" + targetConfigurationName);
            var destination = GetOutputDirFullPath(configuration, platform);
            if ( !Directory.Exists( source ) )
            {
                Log.Warning(string.Format("ExtraBinResources not found for {0}|{1} ", configuration, platform));
                return;
            }
            
            var processParams = string.Format("{0} {1} *.* /XD .svn {2} /NP /NS /NC /NJS /NJH /NDL /NFL /R:5 /W:1", source, destination, sync ? "/MIR" : "");
            var startInfo = new ProcessStartInfo("robocopy.exe", processParams ){
			    UseShellExecute = true,
			    RedirectStandardOutput = false,
			    RedirectStandardInput = false,
			    RedirectStandardError = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

			var process = Process.Start( startInfo );
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (process != null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                process.WaitForExit();
                if (process.ExitCode == 0)
                    return;
                
                if (process.ExitCode <= 7)
                {
                    Log.Info(string.Format("{0} ExtraBinResources for {1}|{2}", sync ? "Sync" : "Copy", configuration, platform));
                }
                else
                {
                    Log.Error(string.Format("{0} ExtraBinResources for {1}|{2} failed!", sync ? "Sync" : "Copy", configuration, platform));
                }
            }
        }

		public static string GetLinkerPath( PlatformType platform )
		{
			switch ( platform )
			{
				case PlatformType.Win32:
					return @"$(VCInstallDir)bin\link.exe";
				case PlatformType.Win64:
					return @"$(VCInstallDir)bin\x86_amd64\link.exe";
				case PlatformType.Durango:
                    return @"$(VCInstallDir)bin\amd64\link.exe";
                case PlatformType.Orbis:
                    return @"$(SCE_ORBIS_SDK_DIR)\host_tools\bin\orbis-ld.exe";
				default:
					throw new NotImplementedException();
			}
		}

        public static IDebugFileMapping GetFileMappingForProject(ProjectFile project, Workspace workSpace)
        {
            switch (project.platform)
            {
                case PlatformType.Orbis:
                    {
						var OrbisSDKRoot = Environment.GetEnvironmentVariable( "SCE_ORBIS_SDK_DIR" );
						if (!string.IsNullOrEmpty(OrbisSDKRoot) && Directory.Exists(OrbisSDKRoot))
                        {
							var outputDir = GetOutputDirFullPath(project.configuration, project.platform);
							var fileMapping = new Ps4FileMapping(outputDir, "configuration");
							fileMapping.AddOverlay(
								Ps4FileMapping.Overlay.Type.Opaque, 0,
								Path.Combine(OrbisSDKRoot, @"target\sce_module"),
								"sce_module");

                            fileMapping.AddOverlay(
								Ps4FileMapping.Overlay.Type.Opaque, 64,
								Path.Combine(OrbisSDKRoot, @"target\prx"),
								"prx");
                            return fileMapping;
                        }
                    }
                    break;
                case PlatformType.Durango:
//                    var fileMapping = new XboxOneFileMapping();
                    break;
            }
            return null;
        }
	}
}