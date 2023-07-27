using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditorCLI : BaseCSharpExecutable
	{
		public EditorCLI( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration, ApplicationKind.CONSOLE_APPLICATION )
		{
			rootNamespace = "EditorCLI";

			AddProjectFiles();

			DependsOn<EditorLauncher>();
			DependsOn<ResourceDB>();
			DependsOn<ResourceDbWcfClient>();
			DependsOn<ResourceDbWcfService>( true );
			DependsOn<ClientEditorManaged>( true );
			DependsOn<ClientEditorEngine>( true );

			ReferenceAssembly( "EPPlus", "%(VendorsDir)WheresMyMemory/Vendors/EPPlus/EPPlus.dll" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xaml" );
		}
	}
}