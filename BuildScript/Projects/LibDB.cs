using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class LibDB : BaseCppLibrary
	{
		public LibDB( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_ABSTRACTION;

			AddProjectFiles();

			DependsOn<DataProvider>();
			DependsOn<Global>();
		}
	}
}