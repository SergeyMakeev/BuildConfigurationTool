using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using BCT.Source.Generators;

namespace BCT.Source.Model
{
    public class ProjectFiles
	{
		public Dictionary<Type, ProjectFile> projectInstances = new Dictionary<Type, ProjectFile>();
	}

	public struct PrebuildCommand
	{
		public string fileName;
		public string arguments;
		public string workingDir;
	}

	public class Workspace
	{
		readonly CommandLineOptions commandLineOptions;

		readonly Dictionary<string, string> macroVariables = new Dictionary<string, string>();

		readonly List<PrebuildCommand> prebuildCommands = new List<PrebuildCommand>();

		readonly Dictionary<KeyValuePair<PlatformType, Configuration>, ProjectFiles> projectConfigurations =
			new Dictionary<KeyValuePair<PlatformType, Configuration>, ProjectFiles>();

		public readonly List<Type> solutions = new List<Type>();

		public Workspace( CommandLineOptions _commandLineOptions )
		{
			commandLineOptions = _commandLineOptions;

            var vsVersionStr = GetCommandLineOption("vsVersion", VSVersion.Default.ToString());
            SetVisualStudioVersion(vsVersionStr);
		}

        private static void SetVisualStudioVersion(string vsVersionStr)
        {
            try
            {
                var vsVersion = VSVersion.SetVersion(vsVersionStr);
                Log.Info(string.Format("Generate projects and solutions for {0}", vsVersion.ProductName));
            }
            catch (NotSupportedException)
            {
                Log.Error("Version '{0}' is not supported!\nUse one of follow supported versions: {1}.",
                    vsVersionStr, string.Join(", ", VSVersion.SupportedVersions));

                throw new BCTInvalidOperation();
            }
        }

		public string GetCommandLineOption( string optionName, string defaultValue )
		{
			return commandLineOptions.GetOptionValue( optionName, defaultValue );
		}

		public bool IsCommandLineOptionExist( string optionName )
		{
			return commandLineOptions.IsOptionExist( optionName );
		}

		public void Solution( Type solutionType )
		{
			if ( solutionType.IsSubclassOf( typeof(SolutionFile) ) == false )
				return;

			solutions.Add( solutionType );
		}

		public void SetVariable( string varName, string value )
		{
		    macroVariables["%(" + varName + ")"] = value;
		}

        public string GetVariable( string varName, string defaultValue = null )
        {
            string tmp;
            if (macroVariables.TryGetValue("%(" + varName + ")", out tmp))
                return tmp;
            return defaultValue;
        }

		public string ResolveMacroVariables( string s )
		{
			if ( s == null )
				return null;

			foreach ( var q in macroVariables )
				s = s.Replace( q.Key, q.Value );

			return s;
		}

		public ProjectFile CreateSharedProjectInstance( PlatformType platform, Configuration configuration, Type projectType )
		{
			var key = new KeyValuePair<PlatformType, Configuration>( platform, configuration );

			ProjectFiles projectFiles;
			if ( projectConfigurations.TryGetValue( key, out projectFiles ) == false )
			{
				projectFiles = new ProjectFiles();
				projectConfigurations.Add( key, projectFiles );
			}

			ProjectFile projectFile;
			if ( projectFiles.projectInstances.TryGetValue( projectType, out projectFile ) )
				return projectFile;

			projectFile = (ProjectFile)Activator.CreateInstance( projectType, this, platform, configuration );
			projectFiles.projectInstances.Add( projectType, projectFile );

			foreach ( var thirdPartyRef in projectFile.ReferencesThirdParty )
				Activator.CreateInstance( thirdPartyRef, projectFile, platform, configuration );

			return projectFile;
		}

		public bool Build( IGenerator generator )
		{
			var hasErrors = !generator.BeforeBuild( this );

			// Generate all projects and build solutions
			foreach ( var solutionsType in solutions )
			{
				var solutionFile = (SolutionFile)Activator.CreateInstance( solutionsType );
				if ( solutionFile == null )
				{
					hasErrors = true;
					Log.Error( string.Format( "ERROR: Can't create solution '{0}'", solutionsType.Name ) );
					continue;
				}

				if ( solutionFile.Configurations.Count == 0 )
				{
					hasErrors = true;
					Log.Error( string.Format( "ERROR: Solution '{0}' must have at least 1 configuration", solutionsType.Name ) );
					continue;
				}

				if ( solutionFile.Platforms.Count == 0 )
				{
					hasErrors = true;
					Log.Error( string.Format( "ERROR: Solution '{0}' must have at least 1 platform", solutionsType.Name ) );
					continue;
				}

				Log.VerboseInfo( string.Format( "--- solution ---- {0}", solutionsType.Name ) );

				solutionFile.Clear();

				foreach ( var configuration in solutionFile.Configurations )
				{
					foreach ( var platform in solutionFile.Platforms )
					{
						if ( !solutionFile.BuildSpecificConfiguration( this, platform, configuration ) )
							hasErrors = true;
					}
				}

				if ( !solutionFile.Generate( this, generator ) )
					hasErrors = true;
			}

			// Build projects
			{
				var configurations = new Dictionary<Type, List<ProjectFile>>();
				foreach ( var cfg in projectConfigurations )
				{
					foreach ( var project in cfg.Value.projectInstances )
					{
						List<ProjectFile> projects;
						if ( configurations.TryGetValue( project.Key, out projects ) == false )
						{
							projects = new List<ProjectFile>();
							configurations.Add( project.Key, projects );
						}
						projects.Add( project.Value );
					}
				}

                HashSet<string> projectsForBuild = MakeProjectsFilter();
			    foreach ( var projects in configurations )
			    {
                    if (projectsForBuild.Count == 0 || projectsForBuild.Contains(projects.Key.Name.ToLower()))
                        generator.BuildProject(this, projects.Value);
			    }

				if ( !generator.AfterBuild( this, configurations ) )
					hasErrors = true;
			}
			return !hasErrors;
		}

        public HashSet<string> MakeProjectsFilter()
        {
            var projectsForBuild = new HashSet<string>();
            string buildProjects = commandLineOptions.GetOptionValue("buildProjects", string.Empty);
            if (!string.IsNullOrEmpty(buildProjects))
            {
                string[] projects = buildProjects.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                projectsForBuild.UnionWith(projects.ToList());
            }
            return projectsForBuild;
        }

		public void ExecutePrebuildCommand( string fileName, string arguments, string workingDir, int exitCode = 0  )
		{
            if (IsCommandLineOptionExist("skipPrebuildExecute"))
		    {
                Log.Info(string.Format("Skip execute prebuild command:'{0} {1}'", fileName, arguments));
		        return;
		    }
		    var command = new PrebuildCommand
										{
											fileName = ResolveMacroVariables( fileName ),
											arguments = ResolveMacroVariables( arguments ),
											workingDir = ResolveMacroVariables( workingDir )
										};
			if ( prebuildCommands.Contains( command ) )
				return; //Command was already executed

			var startInfo = new ProcessStartInfo( command.fileName, command.arguments )
											{
												WorkingDirectory = command.workingDir,
												CreateNoWindow = false,
												UseShellExecute = false,
												RedirectStandardOutput = false,
												RedirectStandardInput = false,
												RedirectStandardError = false
											};

			var process = Process.Start( startInfo );
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
			if ( process != null )
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
			{
				process.WaitForExit();
				if ( process.ExitCode != exitCode )
				{
					Log.Info( string.Format( "Wrong exit code: file: '{0}', argument='{1}', workingDirectory='{2}', exitCode='{3}'.",
																	 command.fileName, command.arguments, command.workingDir, process.ExitCode ) );
					return;
				}
			}

			prebuildCommands.Add( command );
		}

        public void CleanTargetDirectories()
        {
            if (!IsCommandLineOptionExist("cleanTargetDirectories"))
                return;
            
            foreach ( var cfg in projectConfigurations )
            {
                var platform = cfg.Key.Key;
				var configuration = cfg.Key.Value;
                var outputDirFullPath = Utilites.GetOutputDirFullPath(configuration, platform);
                Utilites.DeleteFolder( outputDirFullPath );
            }
        }
        
		public void CopyExtraBinResources()
		{
            if (IsCommandLineOptionExist("skipCopyExtraBinResources"))
                return;
            
            var sync = IsCommandLineOptionExist("syncExtraBinResources");
            var extraBinResourcesRootFolder = ResolveMacroVariables(@"%(VendorsDir)ExtraBinResources\game");

			foreach ( var cfg in projectConfigurations )
			{
				var platform = cfg.Key.Key;
				var configuration = cfg.Key.Value;
                Utilites.CopyExtraBinResources(extraBinResourcesRootFolder, configuration, platform, sync);
            }
		}

		public bool CheckFirstStart<T>()
		{
			PrebuildCommand cmd = new PrebuildCommand { fileName = typeof(T).FullName };
			if ( prebuildCommands.Contains( cmd ) )
				return false;

			prebuildCommands.Add( cmd );
			return true;
		}
	}
}