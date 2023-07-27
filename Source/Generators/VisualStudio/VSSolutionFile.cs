using System;
using System.Linq;
using System.Collections.Generic;

using BCT.Source.Model;


namespace BCT.Source.Generators.VisualStudio
{
    // ReSharper disable InconsistentNaming
    internal sealed class VSSolutionFile : VSFile
    // ReSharper restore InconsistentNaming
    {
        #region Solution file helpers
        sealed class ConfigurationAndPlatform : IComparable
        {
            public readonly Configuration configuration;
            public readonly PlatformType platform;
            public readonly string platformName;
            public readonly string configurationName;

            public ConfigurationAndPlatform( ProjectFile project )
            {
                platform = project.platform;
                configuration = project.configuration;
                platformName = project.language == Language.C_SHARP ? Utilites.GetPlatformNameSharp(platform) : Utilites.GetPlatformName(platform);
                configurationName = Utilites.GetConfigurationName( configuration );
            }

            public int CompareTo(object obj)
            {
                var other = obj as ConfigurationAndPlatform;
                if ( other == null )
                    return 1;
                int result = configuration.CompareTo(other.configuration);
                return result != 0 ? result : Comparer<object>.Default.Compare(platform, other.platform);
            }

            public override bool Equals(object obj)
            {
                var other = obj as ConfigurationAndPlatform;
                if (other == null)
                    return false;
                return configuration.Equals(other.configuration) && platform.Equals(other.platform);
            }
            
            public override int GetHashCode()
            {
                return CombineHashCodes(configuration.GetHashCode(), platform.GetHashCode());
            }

            private static int CombineHashCodes(int h1, int h2)
            {
                return (h1 << 5) + h1 ^ h2;
            }

        }
        sealed class ConfigurationAndPlatformStringComparer : IComparer<ConfigurationAndPlatform>
        {
            public int Compare(ConfigurationAndPlatform a, ConfigurationAndPlatform b)
            {
                int result = string.CompareOrdinal(a.configuration.ToString(), b.configuration.ToString());
                return result != 0 ? result : string.CompareOrdinal(a.platform.ToString(), b.platform.ToString());
            }
        }

        sealed class ProjectInSolution
        {
            private readonly ProjectFile project;
            public readonly HashSet<string> references;
            public readonly SortedDictionary<ConfigurationAndPlatform, ProjectFile> configurationAndPlatform;
            public readonly string projectTypeGuid;
            public readonly string projectName;
            public readonly string projectFileRelativePath;
            public readonly string projectGuid;

            public ProjectInSolution(SortedDictionary<ConfigurationAndPlatform, ProjectFile> configurationAndPlatform, HashSet<string> references)
            {
                this.references = references;
                this.configurationAndPlatform = configurationAndPlatform;

                project = configurationAndPlatform.Values.First();
                projectTypeGuid = GetProjectTypeGuid(project);
                projectName = project.projectName;
                projectFileRelativePath = GetProjectFileRelativePath(project);
                projectGuid = project.GetGuid();
            }

            public bool HasReferences
            {
                get
                {
                    return references.Count > 0;
                }
            }

            public bool IsReadyForBuild(ConfigurationAndPlatform cap)
            {
                ProjectFile projectFile;
                if ( configurationAndPlatform.TryGetValue( cap, out projectFile ) == false )
                    return false;
                return !(projectFile.excludeFromSolution);
            }

            public bool IsReadyForDeploy(ConfigurationAndPlatform cap)
            {
                ProjectFile projectFile;
                if (configurationAndPlatform.TryGetValue(cap, out projectFile) == false)
                    return false;
                return (projectFile.applicationKind == ApplicationKind.WINDOWED_APPLICATION || projectFile.applicationKind == ApplicationKind.CONSOLE_APPLICATION) && cap.platform == PlatformType.Durango;
            }

            private static string GetProjectTypeGuid(ProjectFile project)
            {
                switch (project.language)
                {
                    case Language.C_SHARP:
                        return "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
                    case Language.CPP_CLI:
                    case Language.CPP:
                        return "8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942";
                }
                throw new NotSupportedException();
            }

            private static string GetProjectFileRelativePath(ProjectFile project)
            {
                return VSProjectFile.GetProjectFileRelativePath(project);
            }
        }
        #endregion

        private readonly SolutionFile solution;

        VSSolutionFile(SolutionFile solution)
        {
            this.solution = solution;
        }

        public static VSSolutionFile CreateInstance(Workspace workspace, SolutionFile solution)
        {
            return new VSSolutionFile(solution);
        }

        public override string FileFullPath
        {
            get
            {
                var solutionName = solution.GetName();
                var filePath =  Utilites.GetTargetDirectory() + solutionName;
                if (VSVersion.CurrentVersion > VSVersion.Vs2010)
                    filePath += "_" + VSVersion.CurrentVersion;
                filePath += ".sln";
                return filePath;
            }
        }

        public void Generate(Dictionary<Type, List<ProjectFile>> projectConfigurations)
        {
            var projectsInSolution = new List<ProjectInSolution>();
            var solutionConfigurationAndPlatforms = new List<ConfigurationAndPlatform>();

            var cmp = new ConfigurationAndPlatformStringComparer();
            foreach (var projects in projectConfigurations)
            {
                var uniqueReferences = new HashSet<string>();
                var configurationAndPlatform = new SortedDictionary<ConfigurationAndPlatform, ProjectFile>(cmp);
                foreach (var project in projects.Value)
                {
                    var cap = new ConfigurationAndPlatform(project);
                    if (!solutionConfigurationAndPlatforms.Contains(cap))
                        solutionConfigurationAndPlatforms.Add(cap);

                    foreach (var projRefGuid in project.References.Select(projRef => projRef.project.GetGuid())) 
                    {
                        uniqueReferences.Add(projRefGuid);
                    }
                    configurationAndPlatform.Add(cap, project);
                }
                var projectInSolution = new ProjectInSolution(configurationAndPlatform, uniqueReferences);
                projectsInSolution.Add(projectInSolution);
            }
            solutionConfigurationAndPlatforms.Sort();

            WriteSolutionFile(projectsInSolution, solutionConfigurationAndPlatforms);
        }

        #region Writers
        private void WriteSolutionFile(List<ProjectInSolution> projectsInSolution, IEnumerable<ConfigurationAndPlatform> solutionConfigurationAndPlatforms)
        {
            WriteHeader();
            WriteProjectConfigurations(projectsInSolution);
            WriteSolutionFolder();
            Write("Global");
            WriteSolutionConfigurationPlatforms(solutionConfigurationAndPlatforms);
            WriteProjectConfigurationPlatforms(projectsInSolution);
            WriteSolutionProperties();
            Write("EndGlobal");
        }

        private void WriteHeader()
        {
            Write("");
            Write("Microsoft Visual Studio Solution File, Format Version {0}", VSVersion.CurrentVersion.SolutionFileVersion);
            Write("# {0}", VSVersion.CurrentVersion.FullName);
            //Write("VisualStudioVersion = {0}");
            //Write("MinimumVisualStudioVersion = {0}");
        }

        private void WriteProjectConfigurations(IEnumerable<ProjectInSolution> projectsInSolution)
        {
            foreach ( var project in projectsInSolution )
            {
                Write("Project(\"{{{0}}}\") = \"{1}\", \"{2}\", \"{{{3}}}\"", 
                    project.projectTypeGuid,
                    project.projectName,
                    project.projectFileRelativePath,
                    project.projectGuid);

                if (project.HasReferences)
                {
                    Write("\tProjectSection(ProjectDependencies) = postProject");
                    foreach (var reference in project.references)
                        Write("\t\t{{{0}}} = {{{0}}}", reference);
                    Write("\tEndProjectSection");
                }
                Write("EndProject");
            }
        }

        private void WriteSolutionFolder(){}

        private void WriteSolutionConfigurationPlatforms(IEnumerable<ConfigurationAndPlatform> solutionConfigurationAndPlatforms)
        {
            Write("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
            foreach (var cap in solutionConfigurationAndPlatforms)
                Write("\t\t{0}|{1} = {0}|{1}", cap.configurationName, cap.platform);
            Write("\tEndGlobalSection");
        }

        private void WriteProjectConfigurationPlatforms(IEnumerable<ProjectInSolution> projectsInSolution)
        {
            Write("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
            foreach (var project in projectsInSolution)
            {
                var projectGuid = project.projectGuid;
                foreach (var cap in project.configurationAndPlatform.Keys)
                {
                    Write("\t\t{{{0}}}.{1}|{2}.ActiveCfg = {1}|{3}", projectGuid, cap.configurationName, cap.platform, cap.platformName);
                    if (project.IsReadyForBuild(cap))
                        Write("\t\t{{{0}}}.{1}|{2}.Build.0 = {1}|{3}", projectGuid, cap.configurationName, cap.platform, cap.platformName);
                    if (project.IsReadyForDeploy(cap))
                        Write("\t\t{{{0}}}.{1}|{2}.Deploy.0 = {1}|{3}", projectGuid, cap.configurationName, cap.platform, cap.platformName);
                }
            }
            Write("\tEndGlobalSection");
        }

        private void WriteSolutionProperties()
        {
            Write("\tGlobalSection(SolutionProperties) = preSolution");
            Write("\t\tHideSolutionNode = FALSE");
            Write("\tEndGlobalSection");
        }
        #endregion
    }
}
