using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class LoggerCli : BaseCliLibrary
	{
		public LoggerCli( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Logger>();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}