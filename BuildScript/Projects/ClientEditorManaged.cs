using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClientEditorManaged : BaseCSharpLibrary
	{
		public ClientEditorManaged( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ClientEditorManaged";

			AddProjectFiles();

			DependsOn<ClientTools>();
			DependsOn<ClientEditorBridge>( true );
			DependsOn<Export>( true );
			DependsOn<ResourceDBUtils>();
			DependsOn<EditorControlsLite>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Data" );
		}
	}
}