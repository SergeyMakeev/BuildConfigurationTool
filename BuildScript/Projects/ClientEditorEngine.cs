using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClientEditorEngine : BaseCliLibrary
	{
		public ClientEditorEngine( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<ClientTools>();
			DependsOn<Main>();
			DependsOn<GameStateDebug>();
			DependsOn<Export>();
			DependsOn<ClientEditorNative>();
			DependsOn<ParticleFX>();
			DependsOn<SceneExport>();
			DependsOn<GIPrecompute>();
			DependsOn<ExportImageTools>();

			ReferenceAssembly( "System.Core" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );

			Define( "CAN_USE_WINDOWS_ONLY" );
		}
	}
}