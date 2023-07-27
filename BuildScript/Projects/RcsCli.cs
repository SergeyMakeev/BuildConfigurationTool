using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RcsCli : BaseCliLibrary
	{
		public RcsCli( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<RcsCpp>();
			DependsOn<LibDB>();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}