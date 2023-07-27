using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DBGeneratedClientClasses : BaseCSharpLibrary
	{
		public DBGeneratedClientClasses( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DB";

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