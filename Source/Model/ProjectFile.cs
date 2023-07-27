using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace BCT.Source.Model
{
    public class GlobalFilesEnumeratorCache
    {
        public class CacheEntryKey
        {
            string parentDir;
            List<string> fileMasks;

            public CacheEntryKey(string _parentDir, List<string> _fileMasks)
            {
                parentDir = _parentDir;
                fileMasks = _fileMasks;
            }

            public override bool Equals(object obj)
            {
                CacheEntryKey other = obj as CacheEntryKey;

                if (string.Compare(parentDir, other.parentDir, StringComparison.Ordinal) != 0)
                {
                    return false;
                }

                if (fileMasks.Count != other.fileMasks.Count)
                {
                    return false;
                }

                for (int i = 0; i < fileMasks.Count; i++)
                {
                    if (string.Compare(fileMasks[i], other.fileMasks[i], StringComparison.Ordinal) != 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            public override int GetHashCode()
            {
                return fileMasks.Count ^ parentDir.GetHashCode();
            }
        }


        static GlobalFilesEnumeratorCache instance = null;

        Dictionary<CacheEntryKey, IEnumerable<string>> cache = new Dictionary<CacheEntryKey, IEnumerable<string>>();

        public IEnumerable<string> Get(string _parentDir, List<string> _fileMasks)
        {
            CacheEntryKey key = new CacheEntryKey(_parentDir, _fileMasks);

            IEnumerable<string> res;
            if (cache.TryGetValue(key, out res))
            {
                return res;
            }

            return null;
        }

        public void Add(string _parentDir, List<string> _fileMasks, IEnumerable<string> _result)
        {
            CacheEntryKey key = new CacheEntryKey(_parentDir, _fileMasks);
            cache.Add(key, _result);
        }

        public static GlobalFilesEnumeratorCache GetInstance()
        {
            if (instance == null)
            {
                instance = new GlobalFilesEnumeratorCache();
            }
            return instance;
        }

    }

	public class ProjectRefs
	{
		public ProjectFile project;
		public int referencesCount;

		public ProjectRefs( ProjectFile _p )
		{
			project = _p;
			referencesCount = 1;
		}
	}

	public class CSharpReferencedProject : IComparable<CSharpReferencedProject>
	{
		public string name;
		public ProjectFile project;

		public int CompareTo( CSharpReferencedProject other )
		{
			return string.Compare( name, other.name, StringComparison.Ordinal );
		}
	}

	public class CSharpLibrary : IComparable<CSharpLibrary>
	{
		public string assemblyPath;
		public string name;

		public CSharpLibrary( string name, string assemblyPath = null )
		{
			this.name = name;
			this.assemblyPath = assemblyPath;
		}

		public int CompareTo( CSharpLibrary other )
		{
			return string.Compare( name, other.name, StringComparison.Ordinal );
		}
	}

	// ReSharper disable InconsistentNaming
	public enum Layer
	{
		FOUNDATION,
		RESOURCE_TYPES,
		PLATFORM_ABSTRACTION,
		ENGINE,
		PROJECT_SPECIFIC,
		APPLICATION,
		TOOLS,

		PLATFORM_IMPLEMENTATION
	}

	// ReSharper restore InconsistentNaming

	public class CustomBuildFile
	{
		public string command;
		public string name;
		public string outputs;
	}

    public class ProjectFile : IComparable<ProjectFile>
	{
		readonly Configuration _configuration;

		readonly PlatformType _platform;
		readonly List<CustomBuildFile> customBuildFiles = new List<CustomBuildFile>();
		readonly List<string> defines = new List<string>();
		readonly SortedSet<string> delayLoaded = new SortedSet<string>();

		readonly SortedSet<Tuple<string, ProjectFile>> dependsOnLibraries = new SortedSet<Tuple<string, ProjectFile>>();

		readonly List<Regex> excludes = new List<Regex>();

		readonly List<string> disabledClangWarning = new List<string>();

		readonly List<string> fileLinks = new List<string>();
		readonly List<string> fileMasks = new List<string>();
        readonly List<Regex> fileMasksIgnoreUB = new List<Regex>();
		readonly List<string> includeDirs = new List<string>();

		readonly SortedSet<string> librariesWithoutExt = new SortedSet<string>();

		readonly SortedSet<string> librariesDirs = new SortedSet<string>();
		readonly HashSet<Type> privateReferencesTypes = new HashSet<Type>();

		readonly SortedSet<CSharpLibrary> referencedAssemblies = new SortedSet<CSharpLibrary>();
		readonly SortedSet<CSharpReferencedProject> referencedCSharpProjects = new SortedSet<CSharpReferencedProject>();
        readonly SortedSet<ProjectFile> referencedCppProjects = new SortedSet<ProjectFile>();

		readonly List<ReferenceDesc> references = new List<ReferenceDesc>();

		readonly List<Type> referencesThirdParty = new List<Type>();
		readonly List<Type> referencesTypes = new List<Type>();
		readonly List<string> resourceFileMasks = new List<string>();
		readonly List<ProjectFile> uselessDepends = new List<ProjectFile>();
        readonly SortedSet<SDKReference> sdkReferences = new SortedSet<SDKReference>();

		string _intermediateDirectory = "Temp/";

		public string _location;

		string _outputDirectory = "./";

		public string applicationDefinition;
		public ApplicationKind applicationKind = ApplicationKind.CONSOLE_APPLICATION;
		public string assemblyName;
        public readonly string projectName;
        public readonly string projectGuid;
		public string targetName;
        private string projectFullPath;

		public bool bufferSecurityCheck = false;
		public CharacterSet characterSet = CharacterSet.UNICODE;
		public bool disableOptimization = true;
		public bool enableExceptionHandling = false;
        public bool excludeFromSolution = false;
		public bool generateDebugInformation = true;
		public bool incrementalLinking = true;
		public bool largeAddressAware = false;
		public bool useManifest = false;

        public bool filesRecuriveSearch = true;

		public Layer layer = Layer.PROJECT_SPECIFIC;
		public string preBuildEvent = "";
		public string postBuildEvent = "";
		public string preLinkEvent = "";

		public string rootNamespace;

		public bool skipDefGeneration = false;
		// Не создавать .def-файл для Dll'ки (так что ее функции не линкуются автоматом в зависящие от нее Dll'ки)

		public bool stringPooling = true;

        private Language _language;
        public Language language
        {
            get { return _language; }
            protected set
            {
                _language = value;
                if (_language == Language.C_SHARP)
                    unityBuildType = UnityBuildType.NEVER;
            }
        }

		// unity билда
        private readonly bool enableUnityBuild;
        private UnityBuildType _unityBuildType;
        
        public int MaxFilesInOneUnity { get; protected set; }
        public bool UseUnityBuild { get; private set; }
        
        public UnityBuildType unityBuildType
        {
            get { return _unityBuildType; }
            protected set
            {
                _unityBuildType = value;
                defines.Remove(UnityBuild.UnityBuildDefine);
                UseUnityBuild = UnityBuild.IsUnityBuildNeeded(_unityBuildType, enableUnityBuild);
                if (UseUnityBuild)
                    defines.Add(UnityBuild.UnityBuildDefine);
            }
        }
		public bool treatWarningsAsErrors = true;
		public bool usePrecompiledHeaders = true;
		public WholeProgramOptimization wholeProgramOptimization = WholeProgramOptimization.NONE;

        public IDebugFileMapping fileMapping;

		protected Workspace workSpace;

        public ProjectFile( Workspace _workSpace, PlatformType platform, Configuration configuration )
		{
			workSpace = _workSpace;
			_platform = platform;
			_configuration = configuration;
            
            language = Language.CPP;
            
			//Exclude("*.svn/*");
            projectName = GetType().Name;
						targetName = projectName;
            location = "%(ClientDir)" + projectName + "/";
            assemblyName = projectName;

            //generate guid from project name (persistent)
            projectGuid = Utilites.GetGuidFromString(projectName);

            //setup unity build
            enableUnityBuild = _workSpace.IsCommandLineOptionExist("unityBuild");
            Log.VerboseInfo(string.Format("project {0}, platform {1}, config {2}", GetType().Name, platform, configuration));
            
            unityBuildType = UnityBuildType.DEFINED_BY_USER;
            MaxFilesInOneUnity = UnityBuild.MaxFilesInOneUnityDefault;

            AdditionalCompilerOptions = new List<string>();
		}

		public string location { get { return _location; } set { _location = Utilites.FixupTrailingSlash( Utilites.FixupSlashes( value ) ); } }

		//ro references
		public List<ReferenceDesc> References { get { return references; } }

        public SortedSet<string> LibrariesWithoutExt { get { return librariesWithoutExt; } }

        public SortedSet<string> LibrariesDirs { get { return librariesDirs; } }

		public IEnumerable<string> DisabledClangWarning { get { return disabledClangWarning; } }

        public List<string> IncludeDirs { get { return includeDirs; } }

		public List<Type> ReferencesThirdParty { get { return referencesThirdParty; } }

		//ro platform
		public PlatformType platform { get { return _platform; } }

		//ro config
		public Configuration configuration { get { return _configuration; } }

		public SortedSet<CSharpLibrary> ReferencedAssemblies { get { return referencedAssemblies; } }
        public SortedSet<CSharpReferencedProject> ReferencedCSharpProjects { get { return referencedCSharpProjects; } }
        public SortedSet<ProjectFile> ReferencedCppProjects { get { return referencedCppProjects; } }
        public List<string> Defines { get { return defines; } }
        public List<string> AdditionalCompilerOptions { get; protected set; }


        public int CompareTo(ProjectFile other)
        {
            return string.Compare(assemblyName, other.assemblyName, StringComparison.Ordinal);
        }

        public string ApplicationIcon { get; set; }

		public string outputDirectory
		{
			get { return _outputDirectory; }
			set { _outputDirectory = Utilites.FixupTrailingSlash( Utilites.FixupSlashes( value ) ); }
		}
        public string outputDirectoryRelativePath
        {
            get { return GetRelativePath(Utilites.GetTargetDirectory() + outputDirectory); }
        }
        public string intermediateDirectory
		{
			get { return _intermediateDirectory; }
			set { _intermediateDirectory = Utilites.FixupTrailingSlash( Utilites.FixupSlashes( value ) ); }
		}
        public string intermediateDirectoryRelativePath
        {
            get { return GetRelativePath(Utilites.GetTargetDirectory() + intermediateDirectory); }
        }
        public string PreBuildEvent
        {
            get { return workSpace.ResolveMacroVariables(preBuildEvent); }
        }
        public string PostBuildEvent
        {
            get { return workSpace.ResolveMacroVariables(postBuildEvent); }
        }
        public string PreLinkEvent
        {
            get { return workSpace.ResolveMacroVariables(preLinkEvent); }
        }

        public string GetRelativePath(string path)
        {
            return Utilites.RelativePath(ProjectFullPath, workSpace.ResolveMacroVariables(path));
        }

        public IEnumerable<string> GetIncludeDirs(string parentDir)
		{
			var tmp = new List<string>( includeDirs );
			for ( var i = 0; i < tmp.Count; i++ )
				tmp[i] = Utilites.RelativePath( parentDir, workSpace.ResolveMacroVariables( tmp[i] ) );
			return tmp;
		}

        public IEnumerable<string> GetLibraryDirs(string parentDir)
		{
			var tmp = new List<string>( librariesDirs );
			for ( var i = 0; i < tmp.Count; i++ )
				tmp[i] = Utilites.RelativePath( parentDir, workSpace.ResolveMacroVariables( tmp[i] ) );
            return tmp;
		}

		public List<ProjectFile> GetUselessDepends()
		{
			return uselessDepends;
		}

        public IEnumerable<SDKReference> GetSdkReferences()
        {
            return sdkReferences;
        }

        public bool HasSdkReferences()
        {
            return sdkReferences.Count > 0;
        }

		public IEnumerable<string> GetDependsOnLibraries(PlatformType _pltfrm, Configuration _config)
		{
			var tmp = new List<string>();

			foreach (Tuple<string, ProjectFile> dependsProject in dependsOnLibraries)
			{
				if (dependsProject.Item2.excludeFromSolution == false)
				{
					tmp.Add(workSpace.ResolveMacroVariables(dependsProject.Item1));
				}
				else
				{
					Console.WriteLine("Prj {0} exclude reference to {1}. Config {2}, Platform {3}", GetType().Name, dependsProject.Item2.GetType().Name, _config.ToString(), _pltfrm.ToString());
				}
			}

			return tmp;
		}

		public int GetDependsOnLibrariesCount()
		{
			return dependsOnLibraries.Count;
		}

        public IEnumerable<string> GetLibraries()
		{
            var libExt = platform == PlatformType.Orbis ? ".a" : ".lib";
			var tmp = new List<string>( librariesWithoutExt );
			for (var i = 0; i < tmp.Count; i++)
			{
				//быстрый хак, что бы собирать под Orbis, системные либы указываются в виде -lSceSysmodule_stub_weak
				//и если к ним дописывать расширение, то ничего не линкуется
				if (tmp[i].StartsWith("-l") == false)
				{
					tmp[i] = workSpace.ResolveMacroVariables(tmp[i]) + libExt;
				}
			}
            return tmp;
		}

		public int GetLibrariesCount()
		{
			return librariesWithoutExt.Count;
		}

		public int GetDependsOnCount()
		{
			return referencesTypes.Count;
		}

		public bool HasAdditionalDependencies
		{
				get { return GetLibrariesCount() > 0 || GetDependsOnLibrariesCount() > 0; }
		}

		public IEnumerable<string> GetAdditionalDependencies(PlatformType _pltfrm, Configuration _config)
		{
			var additionalDependencies = GetLibraries().Concat(GetDependsOnLibraries(_pltfrm, _config));
			return additionalDependencies;
		}

		//TODO: return IEnumerable<string>
		public string GetDelayLoadedList()
		{
				return string.Join(";", delayLoaded);
		}

		public string GetGuid()
		{
            return projectGuid;
        }
       
        public string ProjectFullPath
        {
            get
            {
                if ( projectFullPath == null )
                {
                    projectFullPath = location;
                    if (string.IsNullOrEmpty(projectFullPath))
                        projectFullPath = Utilites.GetTargetDirectory() + projectName + "/";
                    projectFullPath = Utilites.FixupSlashes( workSpace.ResolveMacroVariables(projectFullPath) );
                }
                return projectFullPath;
            }
		}

        //TODO: объединить с GetFiles, а пока копипаста
		public IEnumerable<string> GetResourceFiles( string parentDir )
		{
            IEnumerable<string> cachedResult = GlobalFilesEnumeratorCache.GetInstance().Get(parentDir, resourceFileMasks);
            if (cachedResult != null)
            {
                return cachedResult;
            }

			var files = new List<string>();
			foreach ( var resFileMask in resourceFileMasks )
			{
                var filesByMask = Utilites.GetFilesByMask(resFileMask, filesRecuriveSearch);
                foreach (var file in filesByMask)
                {
                    var closure = file;
                    var excludeFile = excludes.Any(x => x.IsMatch(closure));

                    if (file.Contains(".svn/") || file.Contains(".svn\\"))
                    {
                        excludeFile = true;
                    }

                    if (!excludeFile)
                    {
                        var relativePath = Utilites.RelativePath(parentDir, file);
                        files.Add(relativePath);
                    }
                }
			}

			files.Sort();

            GlobalFilesEnumeratorCache.GetInstance().Add(parentDir, resourceFileMasks, files);
			return files;
		}

		public List<CustomBuildFile> GetCustomBuild()
		{
			return customBuildFiles;
		}

		public IEnumerable<string> GetFiles( string parentDir )
		{
            IEnumerable<string> cachedResult = GlobalFilesEnumeratorCache.GetInstance().Get(parentDir, fileMasks);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            SortedSet<string> files = new SortedSet<string>();

			foreach ( var fileMask in fileMasks )
			{
                var filesByMask = Utilites.GetFilesByMask(fileMask, filesRecuriveSearch);
				foreach ( var file in filesByMask )
				{
					var closure = file;
					var excludeFile = excludes.Any( x => x.IsMatch( closure ) );

					if ( file.Contains( ".svn/" ) || file.Contains( ".svn\\" ) )
					{
						// Специальный способ отсечь свн копии файлов, если делать общим способом, то все тормозит...
						excludeFile = true;
					}

					if ( !excludeFile )
					{
						var relativePath = Utilites.RelativePath( parentDir, file );
						files.Add( relativePath );
					}
				}
			} //mask iterator

            foreach(string fileName in fileLinks)
            {
                files.Add(fileName);
            }
            
			//files.AddRange( fileLinks );
			//files.Sort();
            GlobalFilesEnumeratorCache.GetInstance().Add(parentDir, fileMasks, files);
			return files;
		}

		public void CustomBuildResource( string file, string command, string outputs )
		{
			customBuildFiles.Add( new CustomBuildFile
														{
															name = workSpace.ResolveMacroVariables( file ),
															command = workSpace.ResolveMacroVariables( command ),
															outputs = workSpace.ResolveMacroVariables( outputs )
														} );
		}

		public void Files( string fileMask )
		{
			var fMask = workSpace.ResolveMacroVariables( fileMask );
			fileMasks.Add( fMask );
		}

        public List<Regex> UnityBuildIngoreList { get { return fileMasksIgnoreUB; } }

        public void UnityBuildIgnoreFiles(string fileMask)
        {
            var fileMaskMacroResolved = workSpace.ResolveMacroVariables(fileMask);
            var fileMaskRegex = UnityBuild.GetIgnoreFileMaskRegex(fileMaskMacroResolved);
            fileMasksIgnoreUB.Add(fileMaskRegex);
        }
        
		public void AddFileLink( string path )
		{
			fileLinks.Add( path );
		}

		public void Resources( string fileMask )
		{
			var fMask = workSpace.ResolveMacroVariables( fileMask );
			resourceFileMasks.Add( fMask );
		}

		public void Exclude( string fileMask )
		{
			var exclude = Utilites.ConvertMaskToRegex( workSpace.ResolveMacroVariables( fileMask ) );
			excludes.Add( exclude );
		}

		public void IncludePath( string dir )
		{
			includeDirs.Add( Utilites.FixupTrailingSlash( dir ) );
		}

		public void LibrariesPath( string dir )
		{
			librariesDirs.Add( Utilites.FixupTrailingSlash( dir ) );
		}

		public void ReferenceAssembly( CSharpLibrary assembly )
		{
			referencedAssemblies.Add( assembly );
		}

		public void ReferenceAssembly( string name, string assemblyPath = null )
		{
			ReferenceAssembly( new CSharpLibrary( name, assemblyPath ) );
		}

		string TrimEnd(string input, string suffixToRemove)
		{
			if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove))
			{
				return input.Substring(0, input.Length - suffixToRemove.Length);
			}
			
			return input;
		}

		public void Library( string lib )
		{
			lib = TrimEnd(lib, ".lib");
			lib = TrimEnd(lib, ".a");
			librariesWithoutExt.Add( lib );
		}

        public void SdkReference(SDKReference sdkReference)
        {
            sdkReferences.Add( sdkReference );
        }

		public void DelayLoaded( string dllName )
		{
			if ( !dllName.EndsWith( ".dll" ) )
				dllName = dllName + ".dll";

			delayLoaded.Add( dllName );
		}

	    public void DisableClangWarning( string error )
	    {
		    disabledClangWarning.Add( error );
	    }

		public void Define( string define )
		{
			defines.Add(workSpace.ResolveMacroVariables(define) );
		}

		public void Undef( string define )
		{
			defines.Remove( define );
		}

		public void UseThirdParty<T>() where T: ThirdParty
		{
			if ( !referencesThirdParty.Contains( typeof(T) ) )
				referencesThirdParty.Add( typeof(T) );
		}

		public void DependsOn<T>( bool privateReference = false ) where T: ProjectFile
		{
			if ( !referencesTypes.Contains( typeof(T) ) )
				referencesTypes.Add( typeof(T) );

			if ( privateReference )
				privateReferencesTypes.Add( typeof(T) );
		}

		public bool BuildProjectReferencies( Workspace workspace, SolutionFile solution )
		{
			foreach ( var referenceType in referencesTypes )
			{
				var reference = solution.GetProjectInstance( workspace, solution.GetPlatform(), solution.GetConfiguration(), referenceType, false );
				if ( reference == null )
				{
					solution.AddProject( referenceType );

					//Log.Error(string.Format("Invalid reference from {0} to {1}. Project {1} not in solution",  this.GetType().Name, referenceType.Name));
				}
				else
				{
					var _ref = new ReferenceDesc
										 {
											 project = reference,
											 privateReference = privateReferencesTypes.Contains( referenceType )
										 };
					references.Add( _ref );
					Log.VerboseInfo( string.Format( " -ref: {0} DependsOn {1}", GetType().Name, reference.GetType().Name ) );

					//libraries.Add("$(SolutionDir)" + reference.GetLibraryFullPath());
				}
			}

			return true;
		}

		public string GetOutputFilePath()
		{
			string ext;
			if ( applicationKind == ApplicationKind.OBJECT_LIST )
				ext = ".olst";
			else if ( platform == PlatformType.Orbis )
				ext = "_stub.a";
			else
				ext = ".lib";

			return intermediateDirectory + GetType().Name + ext;
		}

		public bool CheckCircularDependenciesAndBuildDependsOnLibrariesList(PlatformType platform, Configuration configuration)
		{
			//TODO: надо проверять через все варианты конфигураций.
			//т.к. можно сделать DependsOn в одной и в другой конфигурации непротиворечивыми, но внутри солюшена нет разделения
			//и они там мержаться, поэтому может быть кольцевая зависимость в зависимостях внутри солюшена (и неясно как оно это прожует)

			var visitedRefs = new Dictionary<Type, ProjectRefs>();

			var refList = new List<ProjectFile>();

			var currentProject = this;

			// Add private references
			foreach ( var reference in currentProject.references )
			{
				ProjectRefs tmp;
				if ( reference.privateReference && !visitedRefs.TryGetValue( reference.project.GetType(), out tmp ) )
					visitedRefs.Add( reference.project.GetType(), new ProjectRefs( reference.project ) );
			}

			while ( currentProject != null )
			{
				ProjectRefs tmp;
				if ( !visitedRefs.TryGetValue( currentProject.GetType(), out tmp ) )
					visitedRefs.Add( currentProject.GetType(), new ProjectRefs( currentProject ) );
				else
					tmp.referencesCount++;

				foreach ( var reference in currentProject.references )
				{
					if ( reference.privateReference )
						continue;

					if ( reference.project == this )
					{
						Log.Error( string.Format( "Circular dependency detected. Circular reference from project {0} to {1}", currentProject.GetType().Name,
																			GetType().Name ) );
						return false;
					}

					if ( !visitedRefs.TryGetValue( reference.project.GetType(), out tmp ) )
						refList.Add( reference.project );
					else
						tmp.referencesCount++;
				}

				if ( refList.Count == 0 )
					currentProject = null;
				else
				{
					var lastIndex = refList.Count - 1;
					currentProject = refList[lastIndex];
					refList.RemoveAt( lastIndex );
				}
			}

			visitedRefs.Remove( GetType() );
			foreach ( var reference in visitedRefs )
			{
				var referenceProject = reference.Value.project;
				if ( referenceProject.language == Language.CPP )
				{
					if (referenceProject.configuration != configuration || referenceProject.platform != platform)
					{
						throw new Exception("Bad logic!");
					}

					if (referenceProject.excludeFromSolution == false)
					{
						dependsOnLibraries.Add(new Tuple<string, ProjectFile>("$(SolutionDir)" + referenceProject.GetOutputFilePath(), referenceProject));
					}
					else
					{
						Console.WriteLine("Project {0} exclude reference to {1}. Config {2}, Platform {3}", GetType().Name, referenceProject.GetType().Name, configuration.ToString(), platform.ToString());
					}

					if ( applicationKind != ApplicationKind.STATIC_LIBRARY && applicationKind != ApplicationKind.OBJECT_LIST )
					{
						if ( referenceProject.applicationKind == ApplicationKind.STATIC_LIBRARY || referenceProject.applicationKind == ApplicationKind.OBJECT_LIST )
						{
							// Copy vendors dependencies from static libraries
							foreach ( var thirdPartyLib in referenceProject.librariesWithoutExt )
								Library( thirdPartyLib );

							foreach ( var thirdPartyLibDir in referenceProject.librariesDirs )
								LibrariesPath( thirdPartyLibDir );
						}
					}

                    referencedCppProjects.Add(referenceProject);
				}
				else
				{
					var csRefProj = new CSharpReferencedProject
													{
														project = reference.Value.project,
														name = reference.Value.project.GetType().Name
													};

					referencedCSharpProjects.Add( csRefProj );
				}
			}

			
			foreach ( var reference in references )
			{
                if ( applicationKind == ApplicationKind.CONSOLE_APPLICATION || applicationKind == ApplicationKind.WINDOWED_APPLICATION )
                {
                    foreach (var sdkReference in reference.project.sdkReferences)
                    {
                        SdkReference(sdkReference);
                    }
                }

                //смотрим текущие референсы, если они еще где то подключены, вносим их в список бесполезных
				var visitedRef = visitedRefs[reference.project.GetType()];
				if ( visitedRef.referencesCount > 1 )
					uselessDepends.Add( reference.project );
			}

			return true;
		}

		public struct ReferenceDesc
		{
			public ProjectFile project;
			public bool privateReference;
		};
	}
}