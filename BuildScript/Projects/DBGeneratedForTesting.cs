using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DBGeneratedForTesting : BaseCSharpLibrary
	{
		public DBGeneratedForTesting( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DBGeneratedForTesting";

			AddProjectFiles();

			DependsOn<ClientTools>();
			DependsOn<ResourceDBNative>();
			DependsOn<DBCore>();
			DependsOn<LoggerCli>();
			DependsOn<ResourceDB>();
			DependsOn<CliNDbHelpers>();
		}
	}
}