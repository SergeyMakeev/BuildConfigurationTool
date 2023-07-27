using System;
using System.Linq;
using System.Collections.Generic;

using BCT.Source.Model;

namespace BCT.Source.Generators
{
    public class GeneratorVisualStudio: IGenerator
    {
        public bool BeforeBuild( Workspace workspace )
        {
            return true;
        }

        public bool BuildProject( Workspace workspace, List<ProjectFile> projectConfigurations )
        {
            var project = projectConfigurations.First();
            var suitableProjectConfigurations = GetSuitableProjectConfigurations(projectConfigurations);
            if ( suitableProjectConfigurations.Count == 0 )
            {
                Log.Warning("Project '{0}' contains incompatible platform(s) with current selected version of Visual Studio;", project.projectName);
                return false;
            }
            using (var projectFile = VisualStudio.VSProjectFile.CreateInstance(workspace, project))
            {
                projectFile.Generate(suitableProjectConfigurations);
                projectFile.Save();
            }
            return true;
        }

        public bool BuildSolution( Workspace workspace, SolutionFile solution, Dictionary<Type, List<ProjectFile>> projectConfigurations )
        {
            var suitableProjectConfigurations = new Dictionary<Type, List<ProjectFile>>();
            foreach ( var projectConfiguration in projectConfigurations )
            {
                var suitableProjectConfiguration = GetSuitableProjectConfigurations(projectConfiguration.Value);
                if ( suitableProjectConfiguration.Count > 0 )
                    suitableProjectConfigurations[projectConfiguration.Key] = suitableProjectConfiguration;
                else
                    Log.Warning("Project '{0}' contains incompatible platform(s) with current selected version of Visual Studio;",
                        projectConfiguration.Value[0].projectName);
            }

            if ( suitableProjectConfigurations.Count == 0 )
            {
                Log.Error("Solution - '{0}' contains projects incompatible with current selected version of Visual Studio;", solution.GetName());
                return false;
            }

            using (var solutionFile = VisualStudio.VSSolutionFile.CreateInstance(workspace, solution))
            {
                solutionFile.Generate(suitableProjectConfigurations);
                solutionFile.Save();
            }
            return true;
        }

        public bool AfterBuild( Workspace workspace, Dictionary<Type, List<ProjectFile>> projectConfigurations )
        {
            GenerateMappingFiles(projectConfigurations);
            return true;
        }

        private static void GenerateMappingFiles(Dictionary<Type, List<ProjectFile>> projectConfigurations)
        {
            var mappingFiles = new HashSet<string>();
            foreach (var projectConfiguration in projectConfigurations)
            {
                foreach (var project in projectConfiguration.Value.Where(p => p.fileMapping != null))
                {
                    var fileMapping = project.fileMapping;
                    if (!mappingFiles.Contains(fileMapping.FilePath))
                    {
                        fileMapping.Write();
                        mappingFiles.Add(fileMapping.FilePath);
                    }
                }
            }
        }

        private static List<ProjectFile> GetSuitableProjectConfigurations( IEnumerable<ProjectFile> projectConfigurations )
        {
            return projectConfigurations.Where(p => VSVersion.CurrentVersion.IsSuitableForPaltform(p.platform)).ToList();
        }
    }
}
