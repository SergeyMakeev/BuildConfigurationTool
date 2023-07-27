using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class CliLibdbTools : BaseCliLibrary
	{
		public CliLibdbTools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<ClientTools>();
			DependsOn<ResourceDBNative>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<LoggerCli>();
			DependsOn<NDbTypes>();
			DependsOn<BinaryLayout>();


			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Core" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Xml.Linq" );
		}
	}
}