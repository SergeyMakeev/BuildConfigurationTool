using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ServerConsole : BaseCppLibrary
	{
		public ServerConsole( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if ( configuration.target == Configuration.Target.FINALRELEASE )
			{
				excludeFromSolution = true;
			}

			AddProjectFiles();
			DependsOn<Replica>();
		}
	}
}