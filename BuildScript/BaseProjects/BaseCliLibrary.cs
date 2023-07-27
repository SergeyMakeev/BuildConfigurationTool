using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCliLibrary : BaseCliProject
	{
		protected BaseCliLibrary( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			UseThirdParty<Profiler>();
			UseThirdParty<TaskScheduler>();

			applicationKind = ApplicationKind.SHARED_LIBRARY;

			ReferenceAssembly( "mscorlib" );
			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Core" );
			ReferenceAssembly( "PresentationCore" );
			ReferenceAssembly( "PresentationFramework" );
			ReferenceAssembly( "WindowsBase" );
		}
	}
}