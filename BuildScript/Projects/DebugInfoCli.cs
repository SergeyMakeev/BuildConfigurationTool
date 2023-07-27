using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DebugInfoCli : BaseCliLibrary
	{
		public DebugInfoCli( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<GameStateDebug>();
			DependsOn<ResourceDB>();
			DependsOn<ClientTools>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<CliLibdbTools>();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}