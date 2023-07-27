using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Export : BaseCliLibrary
	{
		public Export( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<CliLibdbTools>();
			DependsOn<DataProvider>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<ExternalProcesses>();
			DependsOn<LibDB>();
			DependsOn<RcsCli>();
			DependsOn<ResourceDB>();
			DependsOn<ResourceDBNative>();
			DependsOn<ResourceDBUtils>();
			DependsOn<DataProvider>();

			ReferenceAssembly( "mscorlib" );
			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );

			CustomBuildResource( "ExportErrors.default.resx",
													 "xcopy \"%(FullPath)\" \"$(TargetDir)\" /Y",
													 "$(TargetDir)%(Identity)" );
		}
	}
}