using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class VisualScriptsCli : BaseCliLibrary
	{
		public VisualScriptsCli( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<LoggerCli>();
			DependsOn<VisualScripts>();
			DependsOn<ClientTools>();
			DependsOn<ResourceDB>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<CliLibdbTools>();
		}
	}
}