using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameEvents : BaseCppLibrary
	{
		public GameEvents( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<Global>();
		}
	}
}