using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClientTools : BaseCliLibrary
	{
		public ClientTools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<LibDB>();
			DependsOn<LoggerCli>();
			DependsOn<ResourceDB>();
			DependsOn<IO>();
			DependsOn<RcsCpp>();
			DependsOn<Application>();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}