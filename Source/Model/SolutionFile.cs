using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using BCT.Source.Generators;

namespace BCT.Source.Model
{
	public class SolutionConfiguration
	{
		public Dictionary<Type, ProjectFile> projectInstances = new Dictionary<Type, ProjectFile>();
	}

	public class SolutionFile
	{
		readonly List<Configuration> configurations = new List<Configuration>();
		readonly List<PlatformType> platforms = new List<PlatformType>();

		readonly List<Type> projects = new List<Type>();

		readonly Dictionary<KeyValuePair<PlatformType, Configuration>, SolutionConfiguration> solutionConfigurations =
			new Dictionary<KeyValuePair<PlatformType, Configuration>, SolutionConfiguration>();

		Configuration activeConfiguration;
		PlatformType activePlatform;

		public List<PlatformType> Platforms { get { return platforms; } }
		public List<Configuration> Configurations { get { return configurations; } }
		public List<Type> Projects { get { return projects; } }

		public string GetName()
		{
			return GetType().Name;
		}

		public void Clear()
		{
			solutionConfigurations.Clear();
		}

		protected void Project<T>() where T: ProjectFile
		{
			var projectType = typeof(T);
			AddProject( projectType );
		}

		public void AddProject( Type projectType )
		{
			if ( projectType.IsSubclassOf( typeof(ProjectFile) ) == false )
				return;

			projects.Add( projectType );
		}

		protected void AddConfiguration( Configuration configuration )
		{
			configurations.Add( configuration );
		}

		protected void AddPlatform( PlatformType platform )
		{
			platforms.Add( platform );
		}

		public ProjectFile GetProjectInstance( Workspace workSpace, PlatformType platform, Configuration configuration, Type projectType,
																					 bool createIfNeed )
		{
			var key = new KeyValuePair<PlatformType, Configuration>( platform, configuration );

			SolutionConfiguration solutionConfig;
			if ( solutionConfigurations.TryGetValue( key, out solutionConfig ) == false )
			{
				if ( !createIfNeed )
					return null;
				solutionConfig = new SolutionConfiguration();
				solutionConfigurations.Add( key, solutionConfig );
			}

			ProjectFile projectFile;
			if ( solutionConfig.projectInstances.TryGetValue( projectType, out projectFile ) )
				return projectFile;

			if ( createIfNeed == false )
				return null;

			projectFile = workSpace.CreateSharedProjectInstance( platform, configuration, projectType );
			solutionConfig.projectInstances.Add( projectType, projectFile );

			return projectFile;
		}

		public PlatformType GetPlatform()
		{
			return activePlatform;
		}

		public Configuration GetConfiguration()
		{
			return activeConfiguration;
		}

		public bool Generate( Workspace workSpace, IGenerator generator )
		{
			Directory.CreateDirectory( Utilites.GetTargetDirectory() );

			var projectConfigurations = new Dictionary<Type, List<ProjectFile>>();

			foreach ( var cfg in solutionConfigurations )
			{
				foreach ( var project in cfg.Value.projectInstances )
				{
					List<ProjectFile> projectsLocal;
					if ( !projectConfigurations.TryGetValue( project.Key, out projectsLocal ) )
					{
						projectsLocal = new List<ProjectFile>();
						projectConfigurations.Add( project.Key, projectsLocal );
					}
					projectsLocal.Add( project.Value );
				}
			}

			generator.BuildSolution( workSpace, this, projectConfigurations );

			return true;
		}

		public bool BuildSpecificConfiguration( Workspace workSpace, PlatformType platform, Configuration configuration )
		{
			var hasErrors = false;

			activePlatform = platform;
			activeConfiguration = configuration;

			var configurationProjects = new List<ProjectFile>();

			var startProject = 0;

			while ( true )
			{
				for ( var i = startProject; i < projects.Count; i++ )
				{
					var projectFile = GetProjectInstance( workSpace, activePlatform, activeConfiguration, projects[i], true );

					configurationProjects.Add( projectFile );
				}

				startProject = projects.Count;

				// --- этот шаг может добавить элементов в projects
				// --- begin ---
				foreach ( var project in configurationProjects )
				{
					if ( !project.BuildProjectReferencies( workSpace, this ) )
						hasErrors = true;
				}
				// --- end ---

				//новых проектов не добавлялось - можно закончить цикл
				if ( startProject == projects.Count )
					break;

				//добавились новые проекты, повторим все для свежедобавленных проектов
			}

			foreach ( var project in configurationProjects )
			{
				if ( !project.CheckCircularDependenciesAndBuildDependsOnLibrariesList(platform, configuration) )
					hasErrors = true;
			}

			return !hasErrors;
		}
	}
}