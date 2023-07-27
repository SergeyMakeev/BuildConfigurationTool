using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class LibDBLoader : BaseCppLibrary
	{
			public LibDBLoader(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base( workSpace, platform, configuration )
		{
			layer = Layer.APPLICATION;

			AddProjectFiles();

			DependsOn<LibDB>();
			DependsOn<IO>();
			DependsOn<NDbTypes>();
			DependsOn<ClrGhost>();
		}
	}
}