using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DBGeneratedServerClasses : BaseCSharpLibrary
	{
		public DBGeneratedServerClasses( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DBGeneratedServerClasses";
			assemblyName = "DBGeneratedServerClasses-$(Configuration)";
		    
            

			AddProjectFiles();

			DependsOn<DBCore>();
			DependsOn<LoggerCli>();
			DependsOn<ResourceDB>();
			DependsOn<ClientTools>();
			DependsOn<ResourceDBNative>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<DBGeneratedClientClasses>();
		}
	}
}