using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameStateDebug : BaseCppLibrary
	{
		public GameStateDebug(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base( workSpace, platform, configuration )
		{
			if (configuration.target == Configuration.Target.FINALRELEASE)
			{
				excludeFromSolution = true;
			}

			AddProjectFiles();

      DependsOn<Image>();
			DependsOn<GameState>();
			DependsOn<Main>();
			DependsOn<ServerConsole>();
			DependsOn<DebugGeometry>();
		}
	}
}