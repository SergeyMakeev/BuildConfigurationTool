using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Microsoft.CSharp;
using System.Reflection;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;


namespace BCT.Source
{
    public interface IMake
    {
        bool Build(CommandLineOptions options);
    }

    public class BuildScriptRunner
    {
        private static readonly string[] referencedAssemblies =
        {
            //"mscorlib.dll",
            "System.dll"
            //"System.Core.dll",
            //"System.Data.Linq.dll",
            //"System.Reflection.dll"
        };


        readonly CommandLineOptions commandLineOptions;
        private Assembly compiledAssembly;
        private string[] buildScripts = {};
        

        public BuildScriptRunner(CommandLineOptions _commandLineOptions)
        {
            commandLineOptions = _commandLineOptions;
        }

        public int Run()
        {
            if (!LocateBuildScripts())
                return 1;

            if (!LoadBuildScripts())
                return 2;

            IMake buildScriptMake = (IMake)compiledAssembly.CreateInstance("BCT.BuildScript.Make");
            if (buildScriptMake == null)
            {
                Log.Error("Main class 'BCT.BuildScript.Make' not found in build scripts!");
                return 3;
            }

            Log.Info( "Build start..." );
            bool success = buildScriptMake.Build(commandLineOptions);

			ulong warnings = Log.WarningCount;
	        if ( warnings > 0 )
				Log.Warning( "[ {0} WARNING ]", warnings );

			ulong errors = Log.ErrorCount;
	        if ( errors > 0 )
		        Log.Error( "[ {0} ERRORS ]", errors );

            if ( success )
            {
                Log.Success("[ BUILD COMPLETE ]");    
            }
            else
            {
                Log.Error("[ BUILD FAILED ]");
                return 4;
            }
            return 0;
        }

        private bool LookupBuildScriptsInCSprojFile()
        {
            string csprojFilePath = null;
            string buildConfigurationToolRoot = Debugger.IsAttached ? 
                Path.GetDirectoryName(new StackTrace(true).GetFrame(1).GetFileName()) : 
                Path.Combine(Utilites.GetCurrentDirectory(), "BuildConfigurationTool");
            
            while (buildConfigurationToolRoot != null)
            {
                csprojFilePath = Path.Combine(buildConfigurationToolRoot, "BuildScript", "BuildScript.csproj");
                if (File.Exists(csprojFilePath))
                    break;
                buildConfigurationToolRoot = Path.GetDirectoryName(buildConfigurationToolRoot);
            }
            if (buildConfigurationToolRoot == null)
            {
                Log.Error("Can't locate BuildScript.csproj for build scripts loading!");
                return false;
            }

            string buildScriptsRoot = Path.Combine(buildConfigurationToolRoot, "BuildScript");
            Log.Info(string.Format("Load build scripts from BuildScript.csproj located in folder:{0}", buildScriptsRoot));

            bool hasError = false;
            var buildScriptsList = new List<string>();

            try
            {
                var csproj = new XPathDocument(csprojFilePath);
                XPathNavigator navigator = csproj.CreateNavigator();
                var manager = new XmlNamespaceManager(navigator.NameTable);
                manager.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

                XPathExpression query = navigator.Compile(@"/ns:Project/ns:ItemGroup/ns:*[@Include]");
                query.SetContext(manager);

                XPathNodeIterator nodes = navigator.Select(query);
                while (nodes.MoveNext())
                {
                    string buildScriptFilePath = nodes.Current.GetAttribute("Include", manager.DefaultNamespace);
                    string buildAction = nodes.Current.Name;
                    if ( !buildScriptFilePath.EndsWith( ".cs" ) )
                        continue;

                    if (buildAction == "Compile")
                    {
                        buildScriptFilePath = Path.Combine(buildScriptsRoot, buildScriptFilePath);
                        if (File.Exists(buildScriptFilePath))
                        {
                            buildScriptsList.Add(buildScriptFilePath);
                        }
                        else
                        {
                            hasError = true;
                            Log.Error(string.Format("Build script '{0}' not found", buildScriptFilePath));
                        }
                    }
                    else
                    {   
                        Log.Warning(string.Format("'{0}' has wrong build action - '{1}'", buildScriptFilePath, buildAction));
                    }
                }
            }
            catch (Exception ex)
            {
                hasError = true;
                Log.Error("Can't parse BuildConfigurationTool.csproj, error:");
                Log.Error(ex.Message);
            }
            buildScripts = buildScriptsList.ToArray();
            return !hasError;
        }

        private bool LocateBuildScripts()
        {
            string buildScriptsPath = commandLineOptions.GetOptionValue( "buildScriptsPath", string.Empty );
            if (!string.IsNullOrEmpty(buildScriptsPath))
            {
                buildScriptsPath = Path.GetFullPath( buildScriptsPath );
                if (!Directory.Exists( buildScriptsPath))
                {
                    Log.Error(string.Format("Build script path '{0}' does not exists!", buildScriptsPath));
                    return false;
                }
                Log.Info( string.Format("Load build scripts from folder: {0}", buildScriptsPath) );
                buildScripts = Directory.GetFiles(buildScriptsPath, "*.cs", SearchOption.AllDirectories);
                return true;
            }
            bool result = LookupBuildScriptsInCSprojFile();
            return result;
        }

        private bool LoadBuildScripts()
        {
            if (buildScripts.Length == 0)
            {
                Log.Error("Can't find any build script files, exit with error!");
                return false;
            }
            Log.Success(string.Format("Found {0} build script files, loading...", buildScripts.Length));
            using (var compiler = new CSharpCodeProvider())
            {
                var compilerParameters = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = Debugger.IsAttached

                };
                compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies);
                compilerParameters.ReferencedAssemblies.Add(typeof(Program).Assembly.Location);

                CompilerResults compilerResults = compiler.CompileAssemblyFromFile(compilerParameters, buildScripts);
                if ( !compilerResults.Errors.HasErrors && !compilerResults.Errors.HasWarnings )
                {
                    compiledAssembly = compilerResults.CompiledAssembly;
                    return true;
                }
                
                int count = 0;
                int errors = 0;
                int warnings = 0;
                foreach (CompilerError compilerError in compilerResults.Errors)
                {
                    Log.Info(string.Format("-----------------------------[ {0} ]----------------------------->", ++count));
                    if (compilerError.IsWarning)
                    {
                        Log.Warning(compilerError.ToString());
                        warnings++;
                    }
                    else
                    {
                        Log.Error(compilerError.ToString());
                        errors++;
                    }
                }
                Log.Error(string.Format("- Build scripts compiled with - {0} error(s){1}!", errors, warnings > 0 ? string.Format(" and {0} warrning(s)", warnings): ""));
            }
            return false;
        }
    }
}
