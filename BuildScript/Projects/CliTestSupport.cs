using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class CliTestSupport : BaseCliLibrary
	{
		public CliTestSupport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<LibDB>();
			DependsOn<LoggerCli>();
			DependsOn<ClientTools>();
			DependsOn<DBCore>();
			DependsOn<Main>();
		}
	}
}