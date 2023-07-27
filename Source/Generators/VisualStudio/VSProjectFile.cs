using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using BCT.Source.Model;
using BCT.Source.Generators.VisualStudio.ProjectStructure;


namespace BCT.Source.Generators.VisualStudio
{
    // ReSharper disable InconsistentNaming
    internal abstract class VSProjectFile : VSXmlFile
    // ReSharper restore InconsistentNaming
    {
        private readonly ProjectFile currentProject;
        
        private static bool endableCodeAnalysis = false;

        VSProjectFile(ProjectFile project) : base("Build")
        {
            currentProject = project;
        }

        public static VSProjectFile CreateInstance(Workspace workspace, ProjectFile project)
        {
            switch (project.language)
            {
                case Language.CPP_CLI:
                case Language.CPP:
                    if (endableCodeAnalysis == false)
                        endableCodeAnalysis = workspace.IsCommandLineOptionExist("endableCodeAnalysis");
                    return new VCXProjectFile(project);
                case Language.C_SHARP:
                    return new CSProjectFile(project);
            }
            throw new NotSupportedException();
        }

        public override string FileFullPath
        {
            get { return GetProjectFileFullPath( currentProject ); }
        }

        public abstract void Generate(List<ProjectFile> projectConfigurations);

        #region
        private void SetReferencedAssembliesItemGroup()
        {
            if ( currentProject.language == Language.CPP || currentProject.ReferencedAssemblies.Count == 0)
                return;

            //TODO: похоже на какой-то костыль
            projectRoot.CreateTargetElement("GenerateTargetFrameworkMonikerAttribute");

            var itemGroup = projectRoot.CreateItemGroupElement();
            foreach (var library in currentProject.ReferencedAssemblies)
            {
                var item = itemGroup.AddItem("Reference", library.name);
                if (!string.IsNullOrEmpty(library.assemblyPath))
                    item.AddMetadata("HintPath", currentProject.GetRelativePath(library.assemblyPath));
            }
        }

        private void SetReferencedCSharpProjects()
        {
            if ( currentProject.ReferencedCSharpProjects.Count == 0)
                return;
            var referencedCSharpProjects = projectRoot.CreateItemGroupElement();
            foreach (var csProj in currentProject.ReferencedCSharpProjects)
            {
                var refProjectFullName = GetProjectFileFullPath(csProj.project);
                var item = referencedCSharpProjects.AddItem("ProjectReference", currentProject.GetRelativePath(refProjectFullName));
                item.AddMetadata("Project", string.Format("{{{0}}}", csProj.project.GetGuid()));
                //TODO: можно убрать проверку ни на что особо не влияет
                if (currentProject.language == Language.C_SHARP) 
                    item.AddMetadata("Name", csProj.name);
            }
        }

        private void SetResourceItemGroup(ICollection<FileDesc> resourceFiles)
        {
            if (resourceFiles.Count == 0)
                return;

            var resourceItemGroup = projectRoot.CreateItemGroupElement();
            foreach (var resFileName in resourceFiles)
            {
                Utilites.FileType type;
                if (resFileName.fileName.EndsWith(".appxmanifest"))
                    type = Utilites.FileType.APPXMANIFEST;
                else if (resFileName.fileName.EndsWith(".png"))
                    type = Utilites.FileType.IMAGE;
                else
                    type = Utilites.FileType.RESX;
                var includeType = currentProject.language == Language.C_SHARP ? "Resource": GetResourceIncludeType(type);

                if (resFileName.config.excludedConfigurations.Any())
                {
                    var resItem = resourceItemGroup.AddItem(includeType, resFileName.fileName);
                    foreach (var excludedProject in resFileName.config.excludedConfigurations)
                    {
                        var resItemMeta = resItem.AddMetadata("ExcludedFromBuild", "true");
                        resItemMeta.Condition = ConfigurationAndPlatformEqualCondition(excludedProject);
                    }
                }
                else
                {
                    resourceItemGroup.AddItem(includeType, resFileName.fileName);
                }
            }
            //TODO: нужно только для форматирования, можно убрать
            if (!resourceItemGroup.hasItems)
                resourceItemGroup.xmlElement.Value = "\n  ";
        }

        private void SetSourceFilesItemGroup(ICollection<FileDesc> sourceFiles)
        {
            var projectDirName = currentProject.projectName;
            var sourceFilesItemGroup = projectRoot.CreateItemGroupElement();
            foreach (var projectFileName in sourceFiles)
            {
                var fileType = Utilites.GetFileType(projectFileName.fileName);
                var stringFileType = currentProject.language == Language.C_SHARP ? 
                    GetIncludeTypeFromFileTypeCSharp(fileType, projectFileName.fileName == currentProject.applicationDefinition) : 
                    GetIncludeTypeFromFileType(fileType);

                var sourceFile = sourceFilesItemGroup.AddItem(stringFileType, projectFileName.fileName);

                if (projectFileName.fileName[0] == '.' && currentProject.language == Language.C_SHARP)
                {
                    var virtualPath = projectFileName.fileName.Replace("..\\", "");
                    sourceFile.AddMetadata("Link", virtualPath);
                }

                if (fileType == Utilites.FileType.XAML)
                {
                    sourceFile.AddMetadata("Generator", "MSBuild:Compile");
                    sourceFile.AddMetadata("SubType", "Designer");
                }

                if (fileType == Utilites.FileType.CS_SOURCE)
                {
                    var designerCs = new FileDescComparator(projectFileName.fileName.Replace(".cs", ".Designer.cs"));
                    if (sourceFiles.Any(designerCs.IsEqual))
                        sourceFile.AddMetadata("SubType", "Form");
                }

                if (fileType == Utilites.FileType.CS_SOURCE || fileType == Utilites.FileType.RESX)
                {
                    if (projectFileName.fileName.EndsWith(".xaml.cs"))
                    {
                        var xamlName = projectFileName.fileName.Substring(0, projectFileName.fileName.Length - 3);
                        var xamlComp = new FileDescComparator(xamlName);
                        if (sourceFiles.Any(xamlComp.IsEqual))
                            sourceFile.AddMetadata("DependentUpon", Path.GetFileName(xamlName));
                    }

                    string[] dependsCsList = { ".Designer.cs", ".resx" };
                    foreach (var dependsCs in dependsCsList)
                    {
                        if (projectFileName.fileName.EndsWith(dependsCs))
                        {
                            var csName = projectFileName.fileName.Substring(0, projectFileName.fileName.Length - dependsCs.Length) + ".cs";
                            var csComp = new FileDescComparator(csName);
                            if (sourceFiles.Any(csComp.IsEqual))
                                sourceFile.AddMetadata("DependentUpon", Path.GetFileName(csName));
                        }
                    }
                }
             
                if (fileType == Utilites.FileType.SOURCE)
                {
                    var objFileOutput = GetObjectFileOutput(projectDirName, projectFileName.fileName, currentProject.language);
                    if (Utilites.IsCppSourceFile(projectFileName.fileName))
                    {
                        if (!string.IsNullOrEmpty(objFileOutput))
                            sourceFile.AddMetadata("ObjectFileName", string.Format("$(IntDir){0}/", objFileOutput));
                        if (currentProject.language == Language.CPP_CLI)
                        {
                            sourceFile.AddMetadata("CompileAsManaged", "false");
                            sourceFile.AddMetadata("PrecompiledHeader", "NotUsing");
                        }
                    }

                    //cxx файлы для платформы Durango компилируем как WinRT
                    if (Utilites.IsCppCliSourceFile(projectFileName.fileName))
                    {
                        foreach (var includedProject in projectFileName.config.includedConfigurations)
                        {
                            if (includedProject.platform == PlatformType.Durango)
                            {
                                var compileAsWinRT = sourceFile.AddMetadata("CompileAsWinRT", "true");
                                compileAsWinRT.Condition = ConfigurationAndPlatformEqualCondition(includedProject);
                                var precompiledHeader = sourceFile.AddMetadata("PrecompiledHeader", "NotUsing");
                                precompiledHeader.Condition = ConfigurationAndPlatformEqualCondition(includedProject);
                            }
                        }
                    }
                }

                foreach (var excludedProject in projectFileName.config.excludedConfigurations)
                {
                    var excludedFromBuild = sourceFile.AddMetadata("ExcludedFromBuild", "true");
                    excludedFromBuild.Condition = ConfigurationAndPlatformEqualCondition(excludedProject);
                }

                if (Utilites.IsPrecompiledHeaderFile(projectFileName.fileName))
                {
                    foreach (var includedProject in projectFileName.config.includedConfigurations)
                    {
                        if (includedProject.usePrecompiledHeaders)
                        {
                            var precompiledHeader = sourceFile.AddMetadata("PrecompiledHeader", "Create");
                            precompiledHeader.Condition = ConfigurationAndPlatformEqualCondition(includedProject);
                        }
                    }
                }
                if (!sourceFile.hasMetadata && currentProject.language != Language.C_SHARP)
                    sourceFile.xmlElement.Value = "\n    ";
            }
            if (!sourceFilesItemGroup.hasItems)
                sourceFilesItemGroup.xmlElement.Value = "\n  ";
        }

        private void SetSdkReferences(IEnumerable<ProjectFile> projectConfigurations)
        {
            var projSdkReference = new SortedSet<Tuple<string, string>>();
            foreach (var project in projectConfigurations)
            {
                foreach (var sdkReference in project.GetSdkReferences())
                {
                    projSdkReference.Add(new Tuple<string, string>(Utilites.GetPlatformName(project.platform), sdkReference.GetIdentity()));
                }
            }
            
            if (projSdkReference.Count == 0)
                return;

            foreach (var sdkReference in projSdkReference)
            {
                var refItemGroup = projectRoot.CreateItemGroupElement();
                refItemGroup.Condition = string.Format("'$(Platform)'=='{0}'", sdkReference.Item1);
                refItemGroup.AddItem("SDKReference", sdkReference.Item2);
            }
            
        }
        #endregion
        
        #region
        public static string GetProjectFileRelativePath(ProjectFile project)
        {
            return Utilites.RelativePath(Utilites.GetTargetDirectory(), GetProjectFileFullPath(project));
        }

        public static string GetProjectFileFullPath(ProjectFile project)
        {
            var ext = project.language == Language.C_SHARP ? ".csproj" : ".vcxproj";
            var projectFileFullPath = project.ProjectFullPath + project.projectName;
            if ( VSVersion.CurrentVersion > VSVersion.Vs2010 )
                projectFileFullPath += "_" + VSVersion.CurrentVersion;
            projectFileFullPath += ext;
            return projectFileFullPath;
        }

        private static string GetFullConfigurationName(ProjectFile project)
        {
            var platformName = project.language == Language.C_SHARP 
                ? Utilites.GetPlatformNameSharp(project.platform) 
                : Utilites.GetPlatformName(project.platform);
            return string.Format("{0}|{1}", Utilites.GetConfigurationName(project.configuration), platformName);
        }

        private static string ConfigurationAndPlatformEqualCondition(ProjectFile project)
        {
            return string.Format("'$(Configuration)|$(Platform)'=='{0}'", GetFullConfigurationName(project));
        }

        private static string GetIncludeTypeFromFileType(Utilites.FileType fileType)
        {
            switch (fileType)
            {
                case Utilites.FileType.SOURCE:
                    return "ClCompile";
                case Utilites.FileType.HEADER:
                    return "ClInclude";
                case Utilites.FileType.APPXMANIFEST:
                    return "AppxManifest";
            }
            return "None";
        }
       
        private static string GetIncludeTypeFromFileTypeCSharp(Utilites.FileType fileType, bool isApplicationDefinition)
        {
            switch (fileType)
            {
                case Utilites.FileType.CS_SOURCE:
                    return "Compile";
                case Utilites.FileType.XAML:
                    return isApplicationDefinition
                                     ? "ApplicationDefinition"
                                     : "Page";
                case Utilites.FileType.DLL:
                case Utilites.FileType.RESX:
                    return "EmbeddedResource";
                case Utilites.FileType.APPXMANIFEST:
                    return "AppxManifest";
            }
            return "None";
        }
       
        private static string GetResourceIncludeType(Utilites.FileType fileType)
        {
            if (fileType == Utilites.FileType.APPXMANIFEST)
                return "AppxManifest";
            if (fileType == Utilites.FileType.IMAGE)
                return "Image";
            return "ResourceCompile";
        }
        
        private static string GetTargetMachineType(PlatformType platform)
        {
            switch (platform)
            {
                case PlatformType.Win32:
                    return "MachineX86";
                case PlatformType.Durango:
                case PlatformType.Win64:
                    return "MachineX64";
            }
            throw new NotSupportedException();
        }
       
        private static string GetObjectFileOutput(string projectDirName, string projectFileName, Language language)
        {
            var subDir = Path.GetDirectoryName(projectFileName) ?? string.Empty;
            var subDirStartIndex = subDir.LastIndexOf(projectDirName, StringComparison.Ordinal);
            if (subDirStartIndex != -1)
            {
                subDir = subDirStartIndex + projectDirName.Length < subDir.Length
                                     ? subDir.Substring(subDirStartIndex + projectDirName.Length + 1)
                                     : "";
            }
            if (language == Language.CPP_CLI)
            {
                if (!string.IsNullOrEmpty(subDir))
                    return string.Format("native/{0}", subDir);
                return "native";
            }
            if (language == Language.CPP)
            {
                if (!string.IsNullOrEmpty(subDir))
                {
                    //нужно задать имя папки куда складывать объектники, т.к. имена могут пересекаться
                    return subDir;
                }
            }
            return null;
        }

        private static string StringListProperty(params object[] args)
        {
            var strings = new List<string>();
            foreach (var arg in args)
            {
                var str = arg as string;
                if (str != null)
                {
                    strings.Add(str);
                }
                else
                {
                    var props = arg as IEnumerable<string>;
                    if (props == null)
                        throw new FormatException();
                    strings.AddRange(props);
                }
            }
            return string.Join(";", strings);
        }
        #endregion

        // ReSharper disable InconsistentNaming
        internal sealed class VCXProjectFile : VSProjectFile
        // ReSharper restore InconsistentNaming
        {
            private readonly List<PropertyGroupElement> projectGlobalsPropertyGroups;
            private readonly List<PropertyGroupElement> configurationPropertyGroups;
            private readonly List<ImportGroupElement> propertySheetsImportGroup;
            private readonly List<PropertyGroupElement> configurationAdditionalPropertyGroups;
            private readonly List<ItemDefinitionGroupElement> itemDefinitionGroups;
            
            public VCXProjectFile(ProjectFile project) : base(project)
            {
                projectGlobalsPropertyGroups = new List<PropertyGroupElement>();
                configurationPropertyGroups = new List<PropertyGroupElement>();
                propertySheetsImportGroup = new List<ImportGroupElement>();
                configurationAdditionalPropertyGroups = new List<PropertyGroupElement>();
                itemDefinitionGroups = new List<ItemDefinitionGroupElement>();
            }

            public override void Generate(List<ProjectFile> projectConfigurations)
            {
                SetProjectConfigurationsProperties( projectConfigurations );

                projectRoot.AppendElements( projectGlobalsPropertyGroups );

                projectRoot.CreateImportElement("$(VCTargetsPath)\\Microsoft.Cpp.Default.props");

                SetProjectDebuggerProperties( projectConfigurations );

                projectRoot.AppendElements( configurationPropertyGroups );
                
                projectRoot.CreateImportElement("$(VCTargetsPath)\\Microsoft.Cpp.props");
                
                var extensionSettings = projectRoot.CreateImportGroupElement();
                extensionSettings.Label = "ExtensionSettings";
                if ( projectConfigurations.Any( x => x.platform == PlatformType.Orbis ) )
                    AddOrbisSpecificImports(extensionSettings,
                        "$(VCTargetsPath)\\BuildCustomizations\\OrbisWavePsslc.props",
                        "$(VCTargetsPath)\\BuildCustomizations\\SCU.props");
                else
                    extensionSettings.xmlElement.Value = "\n  ";

                projectRoot.AppendElements( propertySheetsImportGroup );

                var userMacrosGroup = projectRoot.CreatePropertyGroupElement();
                userMacrosGroup.Label = "UserMacros";

                projectRoot.AppendElements(configurationAdditionalPropertyGroups );

                projectRoot.AppendElements( itemDefinitionGroups );
                
                SetReferencedAssembliesItemGroup();

                SetReferencedCSharpProjects();

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для файлов с кодом
                //////////////////////////////////////////////////////////////////////////////////////////////////                
                var files = Utilites.GetSourceFiles(projectConfigurations);
                SetSourceFilesItemGroup(files);

                //////////////////////////////////////////////////////////////////////////////////////////////////
                SetSdkReferences(projectConfigurations);

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для файлов с ресурсами
                //////////////////////////////////////////////////////////////////////////////////////////////////
                var resourceFiles = Utilites.GetResourceFiles(projectConfigurations, currentProject.ProjectFullPath);
                SetResourceItemGroup(resourceFiles);
                
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для пост билд степов
                //////////////////////////////////////////////////////////////////////////////////////////////////
                var customFiles = Utilites.GetCustomBuildResource(projectConfigurations);
                SetCustomBuildItemGroup(customFiles);

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для ссылок на проект
                //////////////////////////////////////////////////////////////////////////////////////////////////
                SetReferencesItemGroup(projectConfigurations);

                projectRoot.CreateImportElement("$(VCTargetsPath)\\Microsoft.Cpp.targets");

                var extensionTargets = projectRoot.CreateImportGroupElement();
                extensionTargets.Label = "ExtensionTargets";
                if (projectConfigurations.Any(x => x.platform == PlatformType.Orbis))
                    AddOrbisSpecificImports(extensionTargets,
                        "$(VCTargetsPath)\\BuildCustomizations\\OrbisWavePsslc.targets",
                        "$(VCTargetsPath)\\BuildCustomizations\\SCU.targets");
                else
                    extensionTargets.xmlElement.Value = "\n  ";
            }

            private static void AddOrbisSpecificImports(ImportGroupElement importGroupElement, params string[] imports)
            {
                foreach ( var el in imports.Select( importGroupElement.AddImport ) )
                {
                    el.Condition = "'$(Platform)'=='ORBIS'";
                }
            }

            private void SetProjectConfigurationsProperties(ICollection<ProjectFile> projectConfigurations)
            {
                var projectConfigurationsItemGroup = projectRoot.CreateItemGroupElement();
                projectConfigurationsItemGroup.Label = "ProjectConfigurations";

				#region ItemGroup ProjectConfigurations

	            var allPlatformNames = new HashSet<string>();
	            foreach ( var q in projectConfigurations )
					allPlatformNames.Add( Utilites.GetPlatformName( q.platform ) );

				var allConfigNames = new HashSet<string>();
				foreach ( var q in projectConfigurations )
					allConfigNames.Add( Utilites.GetConfigurationName( q.configuration ) );

	            foreach ( string platform in allPlatformNames )
		            foreach ( string config in allConfigNames )
		            {
			            projectConfigurationsItemGroup
				            .AddItem( "ProjectConfiguration", string.Format( "{0}|{1}", config, platform ),
				                      new MetadataElement( "Configuration", config ),
				                      new MetadataElement( "Platform", platform ) );
		            }

				#endregion

                foreach (var project in projectConfigurations)
                {
                    #region PropertyGroup Globals
                    var globalPropertyGroup = new PropertyGroupElement
                    {
                        Condition = ConfigurationAndPlatformEqualCondition(project),
                        Label = "Globals"
                    };
                    globalPropertyGroup.AddProperty("ProjectGuid", string.Format("{{{0}}}", currentProject.GetGuid()));
                    if (currentProject.language == Language.CPP_CLI)
                    {
                        globalPropertyGroup.AddProperty("TargetFrameworkVersion", "v4.0");
                        globalPropertyGroup.AddProperty("Keyword", "ManagedCProj");
                    }
                    else
                    {
                        globalPropertyGroup.AddProperty("Keyword", "Win32Proj");
                    }
                    globalPropertyGroup.AddProperty("RootNamespace", string.IsNullOrEmpty(currentProject.rootNamespace) ? currentProject.projectName : currentProject.rootNamespace);
                    globalPropertyGroup.AddProperty("AssemblyName", currentProject.assemblyName);

                    if (project.platform == PlatformType.Durango)
                    {
                        globalPropertyGroup.AddProperty("DefaultLanguage", "en-US");
                        globalPropertyGroup.AddProperty("ApplicationEnvironment", "title");
                        globalPropertyGroup.AddProperty("TargetRuntime", "Native");
                    }
                    projectGlobalsPropertyGroups.Add(globalPropertyGroup);
                    #endregion

                    #region PropertyGroup Configuration
                    var configurationPropertyGroup = new PropertyGroupElement
                    {
                        Condition = ConfigurationAndPlatformEqualCondition(project),
                        Label = "Configuration"
                    };
                   
                    switch (project.applicationKind)
                    {
                        case ApplicationKind.CONSOLE_APPLICATION:
                        case ApplicationKind.WINDOWED_APPLICATION:
                            configurationPropertyGroup.AddProperty("ConfigurationType", "Application");
                            break;
                        case ApplicationKind.SHARED_LIBRARY:
                        case ApplicationKind.OBJECT_LIST:
                            configurationPropertyGroup.AddProperty("ConfigurationType", "DynamicLibrary");
                            break;
                        case ApplicationKind.STATIC_LIBRARY:
                            configurationPropertyGroup.AddProperty("ConfigurationType", "StaticLibrary");
                            break;
                        case ApplicationKind.UTILITY:
                            configurationPropertyGroup.AddProperty("ConfigurationType", "Utility");
                            break;
                    }
                    configurationPropertyGroup.AddProperty("UseDebugLibraries", project.disableOptimization ? "true" : "false");

                    if (project.language == Language.CPP_CLI)
                        configurationPropertyGroup.AddProperty("CLRSupport", "true");


                    configurationPropertyGroup.AddProperty("PlatformToolset", currentProject.platform == PlatformType.Orbis ? "Clang" : VSVersion.CurrentVersion.PlatformToolset);
                    configurationPropertyGroup.AddProperty("WholeProgramOptimization", project.wholeProgramOptimization == WholeProgramOptimization.NONE ? "false" : "true");
                    configurationPropertyGroup.AddProperty("CharacterSet", project.characterSet == CharacterSet.UNICODE ? "Unicode" : "MultiByte"); //MultiByte / Unicode

/*
                    if ( project.platform == PlatformType.Orbis && project.configuration == Configuration.DEBUG)
                    {
                            configurationPropertyGroup.AddProperty("UndefinedBehavior", "All"); //-fsanitize=undefined
                    }
*/
                    configurationPropertyGroups.Add(configurationPropertyGroup);
                    #endregion

                    #region ImportGroupElement PropertySheets
                    var propertySheetsImport = new ImportGroupElement
                    {
                        Condition = ConfigurationAndPlatformEqualCondition(project),
                        Label = "PropertySheets"
                    };

                    var localAppDataPlatform = propertySheetsImport.AddImport("$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props");
                    localAppDataPlatform.Condition = "exists('$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props')";
                    localAppDataPlatform.Label = "LocalAppDataPlatform";

                    foreach (var sdkReference in project.GetSdkReferences().Where(sdkReference => sdkReference.HasExternalProperty)) 
                    {
                        propertySheetsImport.AddImport(sdkReference.GetPropertySheetsFilePath());
                    }

                    propertySheetsImportGroup.Add(propertySheetsImport);
                    #endregion

                    #region AdditionalPropertyGroup
                    var configurationAdditionalPropertyGroup = new PropertyGroupElement
                    {
                        Condition = ConfigurationAndPlatformEqualCondition(project)
                    };
                    configurationAdditionalPropertyGroup.AddProperty("LinkIncremental", project.incrementalLinking ? "true" : "false");
                    
                    var intermediateDir = project.intermediateDirectoryRelativePath;
                    var outputDirectory = project.outputDirectoryRelativePath;
                    var outputDir = project.applicationKind == ApplicationKind.STATIC_LIBRARY || project.applicationKind == ApplicationKind.OBJECT_LIST
                        ? intermediateDir
                        : outputDirectory;
                    configurationAdditionalPropertyGroup.AddProperty("OutDir", outputDir);
                    configurationAdditionalPropertyGroup.AddProperty("IntDir", intermediateDir);

                    if (project.platform == PlatformType.Durango)
                    {
                        //TODO: убрать во внешний .prop файл с таким-же условием
                        configurationAdditionalPropertyGroup.AddProperty("LayoutDir", string.Format("{0}Layout\\", intermediateDir));
                    }

                    if (project.applicationKind == ApplicationKind.WINDOWED_APPLICATION || project.applicationKind == ApplicationKind.CONSOLE_APPLICATION)
                    {
                        configurationAdditionalPropertyGroup.AddProperty("TargetName", project.targetName);
                    }

                    if (project.platform == PlatformType.Durango)
                    {
                        //TODO: а нужно ли вообще это?
                        configurationAdditionalPropertyGroup.AddProperty("ExecutablePath", StringListProperty(
                            "$(Console_SdkRoot)bin", 
                            "$(VCInstallDir)bin\\x86_amd64",
                            "$(VCInstallDir)bin", 
                            "$(WindowsSDK_ExecutablePath_x86)", 
                            "$(VSInstallDir)Common7\\Tools\\bin",  
                            "$(VSInstallDir)Common7\\tools",
                            "$(VSInstallDir)Common7\\ide",
                            "$(ProgramFiles)\\HTML Help Workshop",
                            "$(MSBuildToolsPath32)",
                            "$(FxCopDir)",
                            "$(PATH);"));
                        configurationAdditionalPropertyGroup.AddProperty("IncludePath", "$(Console_SdkIncludeRoot)");
                        configurationAdditionalPropertyGroup.AddProperty("ReferencePath", StringListProperty("$(Console_SdkLibPath)", "$(Console_SdkWindowsMetadataPath)"));
                        configurationAdditionalPropertyGroup.AddProperty("LibraryPath", "$(Console_SdkLibPath)");
                        configurationAdditionalPropertyGroup.AddProperty("LibraryWPath", StringListProperty("$(Console_SdkLibPath)", "$(Console_SdkWindowsMetadataPath)"));
                    }

                    configurationAdditionalPropertyGroup.AddProperty("OriginLinkerPath", Utilites.GetLinkerPath(project.platform));
                    
                    var linkerName = project.platform == PlatformType.Durango ? "at-linker-x64.exe" : "at-linker-x86.exe";
                    configurationAdditionalPropertyGroup.AddProperty("LinkToolExe", string.Format("$(SolutionDir)BuildConfigurationTool\\bin\\{0}", linkerName));

                    var useManifest = project.useManifest ? "true" : "false";
                    configurationAdditionalPropertyGroup.AddProperty("GenerateManifest", useManifest);
                    configurationAdditionalPropertyGroup.AddProperty("EmbedManifest", useManifest);

                    if ( project.platform == PlatformType.Orbis )
                    {
                        configurationAdditionalPropertyGroup.AddProperty("PrxStubOutputDir", intermediateDir);
                        if ( endableCodeAnalysis && project.configuration.target == Configuration.Target.DEBUG )
                        {
                            configurationAdditionalPropertyGroup.AddProperty( "RunCodeAnalysisOnce", "true" );
                        }
                    }

                    configurationAdditionalPropertyGroups.Add(configurationAdditionalPropertyGroup);
                    #endregion

                    #region ItemDefinitionGroupElement
                    var itemDefinitionGroupElement = new ItemDefinitionGroupElement
                    {
                        Condition = ConfigurationAndPlatformEqualCondition(project)
                    };

                    #region ItemDefinition ClCompile
                    AddClCompileDefinition( itemDefinitionGroupElement, project );
                    #endregion

                    #region ItemDefinition BuildEvents
                    AddBuildEventsDefinition( itemDefinitionGroupElement, project );
                    #endregion

                    #region ItemDefinition ResourceCompile
                    AddResourceCompileDefinition( itemDefinitionGroupElement );
                    #endregion

                    if (project.applicationKind == ApplicationKind.STATIC_LIBRARY)
                    {
                        #region ItemDefinition Lib
                        AddStaticLibDefinition( itemDefinitionGroupElement, project );
                        #endregion
                    }
                    else
                    {
                        #region ItemDefinition Link
                        AddLinkDefinition( itemDefinitionGroupElement, project );
                        #endregion
                    }
                    itemDefinitionGroups.Add(itemDefinitionGroupElement);
                    #endregion
                }
            }
            
            #region Definitions
            private void AddClCompileDefinition(ItemDefinitionGroupElement itemDefinitionGroupElement, ProjectFile project)
            {
                var clCompile = itemDefinitionGroupElement.AddItemDefinition("ClCompile");

                clCompile.AddMetadata("AdditionalIncludeDirectories", StringListProperty(project.GetIncludeDirs(currentProject.ProjectFullPath), "%(AdditionalIncludeDirectories)"));
                clCompile.AddMetadata("PreprocessorDefinitions", StringListProperty(project.Defines, "%(PreprocessorDefinitions)"));

                var compilerOptions = CompilerOptions.GetOptionsForProject(project);
                foreach (var compilerOption in compilerOptions)
                {
                    clCompile.AddMetadata(compilerOption.name, compilerOption.value);
                }
               
                if (project.AdditionalCompilerOptions.Count > 0)
                {
                    clCompile.AddMetadata("AdditionalOptions", string.Join(" ", project.AdditionalCompilerOptions) + " %(AdditionalOptions)");
                }

                if (!clCompile.hasMetadata)
                    clCompile.xmlElement.Value = "\n    ";
            }

            private void AddBuildEventsDefinition(ItemDefinitionGroupElement itemDefinitionGroupElement, ProjectFile project)
            {
                if (!string.IsNullOrEmpty(project.preBuildEvent))
                {
                    itemDefinitionGroupElement.AddItemDefinition("PreBuildEvent",
                        new MetadataElement("Command", project.PreBuildEvent));
                }
                if (!string.IsNullOrEmpty(project.postBuildEvent))
                {
                    itemDefinitionGroupElement.AddItemDefinition("PostBuildEvent",
                        new MetadataElement("Command", project.PostBuildEvent));
                }
            }

            private void AddResourceCompileDefinition(ItemDefinitionGroupElement itemDefinitionGroupElement)
            {
                itemDefinitionGroupElement.AddItemDefinition("ResourceCompile",
                    new MetadataElement("PreprocessorDefinitions", "\n      "));
            }

            private void AddStaticLibDefinition(ItemDefinitionGroupElement itemDefinitionGroupElement, ProjectFile project)
            {
                var lib = itemDefinitionGroupElement.AddItemDefinition("Lib");
                lib.AddMetadata("TargetMachine", GetTargetMachineType(project.platform));
                lib.AddMetadata("AdditionalOptions", "/ignore:4221 %(AdditionalOptions)");
                if (project.treatWarningsAsErrors)
                    lib.AddMetadata("TreatLibWarningAsErrors", "true");
            }

            private void AddLinkDefinition(ItemDefinitionGroupElement itemDefinitionGroupElement, ProjectFile project)
            {
                if (!string.IsNullOrEmpty(project.preLinkEvent))
                    itemDefinitionGroupElement.AddItemDefinition("PreLinkEvent",
                        new MetadataElement("Command", project.PreLinkEvent));
                
                var link = itemDefinitionGroupElement.AddItemDefinition("Link");
                switch (project.applicationKind)
                {
                    case ApplicationKind.CONSOLE_APPLICATION:
                        link.AddMetadata("SubSystem", "Console");
                        break;
                    case ApplicationKind.WINDOWED_APPLICATION:
                    case ApplicationKind.SHARED_LIBRARY:
                    case ApplicationKind.OBJECT_LIST:
                        link.AddMetadata("SubSystem", "Windows");
                        break;
                }

                if (project.largeAddressAware)
                    link.AddMetadata("LargeAddressAware", "true");


                link.AddMetadata("AdditionalLibraryDirectories", StringListProperty(project.GetLibraryDirs(currentProject.ProjectFullPath)));
                link.AddMetadata("GenerateDebugInformation", project.generateDebugInformation ? "true" : "false");

                var isDebug = project.configuration.target != Configuration.Target.DEBUG ? "true" : "false";
                link.AddMetadata("OptimizeReferences", isDebug);
                link.AddMetadata("EnableCOMDATFolding", isDebug);

                if (project.platform == PlatformType.Durango)
                    link.AddMetadata("GenerateWindowsMetadata", "false");

                var additionalOptions = "/lorig:\"$(OriginLinkerPath)\" ";
                if ( project.language == Language.CPP && !project.skipDefGeneration )
                {
                    if ( project.platform != PlatformType.Orbis )
                        link.AddMetadata( "ModuleDefinitionFile", string.Format( "$(IntDir){0}.def", project.projectName ) );
                    else
                        additionalOptions += string.Format( "$(IntDir){0}.emd ", project.projectName );
                }


                if (project.HasAdditionalDependencies)
                {
					var additionalDependencies = StringListProperty(project.GetAdditionalDependencies(project.platform, project.configuration));
                    //на Durango не надо тащить %(AdditionalDependencies), там в них что то странное (виндовые либы, которых на durango нет)
                    if (project.platform != PlatformType.Durango)
                    {
                        additionalDependencies += ";%(AdditionalDependencies)";
                    }
                    link.AddMetadata( "AdditionalDependencies", additionalDependencies );
                }
                

                if ( project.platform != PlatformType.Orbis )
                {
                    additionalOptions += "/ignore:4248 ";
                }

                if ( project.applicationKind == ApplicationKind.SHARED_LIBRARY )
                    additionalOptions += "/DEFGEN ";

                if ( project.applicationKind == ApplicationKind.OBJECT_LIST )
                    additionalOptions += "/objlist ";

                //TODO: не время пока для этого
//                if ( project.platform == PlatformType.Orbis && project.configuration == Configuration.DEBUG )
//                {
//                    additionalOptions += " -Wl,--addressing=non-aslr";
//                    additionalOptions += " -Wl,-Map=\"" + project.intermediateDirectoryRelativePath + "Symbols.map\"";
//                    additionalOptions += " -Wl,-sn-full-map";
//                }

                link.AddMetadata( "AdditionalOptions", string.Format( "{0}%(AdditionalOptions)", additionalOptions ) );

                //TODO: переделать на StringListProperty
                var delayLoadedList = project.GetDelayLoadedList();
                if (!string.IsNullOrEmpty(delayLoadedList))
                    link.AddMetadata("DelayLoadDLLs", string.Format("{0};%(DelayLoadDLLs)", delayLoadedList));
                
                //TODO: что для Orbis?
                link.AddMetadata("ImportLibrary", "$(IntDir)$(TargetName).lib");

                if (project.treatWarningsAsErrors)
                    link.AddMetadata("TreatLinkerWarningAsErrors", "true");
            }
            #endregion

            private void SetProjectDebuggerProperties(IEnumerable<ProjectFile> projectConfigurations)
            {
                foreach (var project in projectConfigurations)
                {
                    if (!(project.applicationKind == ApplicationKind.WINDOWED_APPLICATION || project.applicationKind == ApplicationKind.CONSOLE_APPLICATION))
                        continue;
                    if (project.configuration.target == Configuration.Target.FINALRELEASE)
                        continue;
                    if (project.platform == PlatformType.Orbis)
                    {
                        var debuggerProperties = projectRoot.CreatePropertyGroupElement();
                        debuggerProperties.Condition = ConfigurationAndPlatformEqualCondition(project);
                        debuggerProperties.Label = "OverrideDebuggerDefaults";

                        debuggerProperties.AddProperty("DebuggerFlavor", "ORBISDebugger");
                        debuggerProperties.AddProperty("LocalDebuggerWorkingDirectory", "$(TargetDir)");

                        var fileMapping = project.fileMapping as Ps4FileMapping;
                        if (fileMapping != null)
                            debuggerProperties.AddProperty("LocalMappingFile", fileMapping.FilePath);
                    }
                    else if (project.platform == PlatformType.Durango)
                    {
                        //not implemented yet
                    }
                }
            }

            private void SetCustomBuildItemGroup(ICollection<CustomBuildDesc> customFiles)
            {
                if (customFiles.Count == 0)
                    return;
                var customBuildItemGroup = projectRoot.CreateItemGroupElement();
                foreach (var customFile in customFiles)
                {
                    var customBuildItem = customBuildItemGroup.AddItem("CustomBuild", customFile.fileName);
                    foreach (var includedConfiguration in customFile.config.includedConfigurations)
                    {
                        var condition = ConfigurationAndPlatformEqualCondition(includedConfiguration);
                        var commandMeta = customBuildItem.AddMetadata("Command", customFile.Command);
                        commandMeta.Condition = condition;
                        var outputsMeta = customBuildItem.AddMetadata("Outputs", customFile.Outputs);
                        outputsMeta.Condition = condition;
                    }
                }
                if (!customBuildItemGroup.hasItems)
                    customBuildItemGroup.xmlElement.Value = "\n    ";
            }

            private void SetReferencesItemGroup(IEnumerable<ProjectFile> projectConfigurations)
            {
                if ( currentProject.language != Language.CPP )
                    return;
                var uniqueReferences = new SortedDictionary<string, ProjectFile>();
                foreach (var proj in projectConfigurations)
                {
                    foreach (var reference in proj.References)
                    {
                        var key = reference.project.GetGuid();
                        ProjectFile tmp;
                        if (!uniqueReferences.TryGetValue(key, out tmp))
                            uniqueReferences.Add(key, reference.project);
                    }
                }

                if (uniqueReferences.Any())
                {
                    var referenceItemGroup = projectRoot.CreateItemGroupElement();
                    foreach (var reference in uniqueReferences)
                    {
                        var referenceFullPath = GetProjectFileFullPath(reference.Value);
                        var referenceRelativePath = currentProject.GetRelativePath(referenceFullPath);
	                    
						var projectReferenceItem = referenceItemGroup.AddItem( "ProjectReference", referenceRelativePath,
							new MetadataElement( "Project", string.Format( "{{{0}}}", reference.Key ) ),
							new MetadataElement( "LinkLibraryDependencies", "false" ));

	                    if ( ( currentProject.applicationKind == ApplicationKind.CONSOLE_APPLICATION || 
							currentProject.applicationKind == ApplicationKind.WINDOWED_APPLICATION )  && 
							reference.Value.language == Language.CPP )
	                    {
							projectReferenceItem.AddMetadata(new MetadataElement("ReferenceOutputAssembly", "false"));
	                    }
							
                    }
                }
            }
        }

        // ReSharper disable InconsistentNaming
        internal sealed class CSProjectFile : VSProjectFile
        // ReSharper restore InconsistentNaming
        {
            public CSProjectFile(ProjectFile project) : base(project) { }

            public override void Generate(List<ProjectFile> projectConfigurations)
            {
                SetGlobalProjectProperties();

                SetProjectConfigurationsProperties( projectConfigurations );

                SetApplicationIcon();

                projectRoot.CreateImportElement("$(MSBuildToolsPath)\\Microsoft.CSharp.targets");

                projectRoot.CreatePropertyGroupElement(new PropertyElement("UseHostCompilerIfAvailable", "False"));

                SetBuildEvents();
                
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для ссылок на проект
                //////////////////////////////////////////////////////////////////////////////////////////////////
                SetReferencedAssembliesItemGroup();
                
                SetReferencedCSharpProjects();

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для файлов с кодом
                //////////////////////////////////////////////////////////////////////////////////////////////////
                var files = Utilites.GetSourceFiles(projectConfigurations);
                SetSourceFilesItemGroup(files);
                
                //////////////////////////////////////////////////////////////////////////////////////////////////
                // Генерация информации для файлов с ресурсами
                //////////////////////////////////////////////////////////////////////////////////////////////////
                var resourceFiles = Utilites.GetResourceFiles(projectConfigurations, currentProject.ProjectFullPath);
                SetResourceItemGroup(resourceFiles);
            }
            
            #region
            private void SetGlobalProjectProperties()
            {
                var propertyGroup = projectRoot.CreatePropertyGroupElement();
                propertyGroup.AddProperty(new PropertyElement("Configuration", "Debug") { Condition = " '$(Configuration)' == '' " });
                propertyGroup.AddProperty(new PropertyElement("Platform", "x86") { Condition = " '$(Platform)' == '' " });
                propertyGroup.AddProperty("ProductVersion", "8.0.30703");
                propertyGroup.AddProperty("SchemaVersion", "2.0");
                propertyGroup.AddProperty("ProjectGuid", string.Format("{{{0}}}", currentProject.GetGuid()));

                switch (currentProject.applicationKind)
                {
                    case ApplicationKind.CONSOLE_APPLICATION:
                        propertyGroup.AddProperty("OutputType", "Exe");
                        break;
                    case ApplicationKind.WINDOWED_APPLICATION:
                        propertyGroup.AddProperty("OutputType", "WinExe");
                        break;
                    case ApplicationKind.SHARED_LIBRARY:
                    case ApplicationKind.STATIC_LIBRARY:
                    case ApplicationKind.OBJECT_LIST:
                        propertyGroup.AddProperty("OutputType", "Library");
                        break;
                }
                propertyGroup.AddProperty("AppDesignerFolder", "Properties");
                propertyGroup.AddProperty("RootNamespace", string.IsNullOrEmpty(currentProject.rootNamespace) ? currentProject.projectName : currentProject.rootNamespace);
                propertyGroup.AddProperty("AssemblyName", currentProject.assemblyName);
                propertyGroup.AddProperty("TargetFrameworkVersion", "v4.0");
                propertyGroup.AddProperty("TargetFrameworkProfile", "");
                propertyGroup.AddProperty("FileAlignment", "512");
            }

            private void SetProjectConfigurationsProperties(IEnumerable<ProjectFile> projectConfigurations)
            {
                foreach (var project in projectConfigurations)
                {
                    var propertyGroup = projectRoot.CreatePropertyGroupElement();
                    propertyGroup.Condition = ConfigurationAndPlatformEqualCondition( project );
                 
                    if (project.generateDebugInformation)
                        propertyGroup.AddProperty("DebugSymbols", "true");

                    if (project.disableOptimization)
                    {
                        propertyGroup.AddProperty("DebugType", "full");
                        propertyGroup.AddProperty("Optimize", "false");
                    }
                    else
                    {
                        propertyGroup.AddProperty("DebugType", "pdbonly");
                        propertyGroup.AddProperty("Optimize", "true");
                    }
                    propertyGroup.AddProperty("PlatformTarget", Utilites.GetPlatformNameSharp(project.platform));
                    propertyGroup.AddProperty("AllowUnsafeBlocks", "true");
                    propertyGroup.AddProperty("OutputPath", project.outputDirectoryRelativePath);
                    propertyGroup.AddProperty("BaseIntermediateOutputPath", project.intermediateDirectoryRelativePath);
                    propertyGroup.AddProperty("IntermediateOutputPath", "$(BaseIntermediateOutputPath)");
                    propertyGroup.AddProperty("DefineConstants", StringListProperty(project.Defines));
                    propertyGroup.AddProperty("ErrorReport", "prompt");
                    propertyGroup.AddProperty("WarningLevel", "4");
                    propertyGroup.AddProperty("UseVSHostingProcess", "false");
                    propertyGroup.AddProperty("AlToolPath", project.platform == PlatformType.Win64 ? "$(SDK40ToolsPath)\\x64": "$(SDK40ToolsPath)");
                }
            }

            private void SetApplicationIcon()
            {
                if (string.IsNullOrEmpty(currentProject.ApplicationIcon))
                    return;
                projectRoot.CreatePropertyGroupElement(new PropertyElement("ApplicationIcon", currentProject.ApplicationIcon));
            }

            private void SetBuildEvents()
            {
                bool projectHasEvents = false;
                var buildEventPropertyGroup = new PropertyGroupElement();
                if (!string.IsNullOrEmpty(currentProject.PreBuildEvent))
                {
                    projectHasEvents = true;
                    buildEventPropertyGroup.AddProperty("PreBuildEvent", currentProject.PreBuildEvent);
                }
                if (!string.IsNullOrEmpty(currentProject.PostBuildEvent))
                {
                    projectHasEvents = true;
                    buildEventPropertyGroup.AddProperty("PostBuildEvent", currentProject.PostBuildEvent);
                }
                if (projectHasEvents)
                    projectRoot.AppendElement(buildEventPropertyGroup);
            }
            #endregion
        }
    }
}
