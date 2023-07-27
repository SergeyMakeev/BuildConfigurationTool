using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDBNative : BaseCliLibrary
	{
		public ResourceDBNative( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DB";

			AddProjectFiles();

			DependsOn<Global>();
			DependsOn<ResourceDB>();
			DependsOn<Tools>();
			DependsOn<LoggerCli>();
			DependsOn<ClientTools>();
			DependsOn<NDbTypes>();
			DependsOn<LibDBLoader>();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}