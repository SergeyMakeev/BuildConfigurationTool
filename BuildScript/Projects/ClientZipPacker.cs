using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClientZipPacker : BaseCppExecutable
	{
		public ClientZipPacker( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;
			applicationKind = ApplicationKind.CONSOLE_APPLICATION;

			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<IO>();
			DependsOn<Application>();

			UseThirdParty<ZLib>();

			Define( "CAN_USE_WINDOWS_ONLY" );
		}
	}
}