using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditedTerrain : BaseCliLibrary
	{
		public EditedTerrain( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<Export>();
			DependsOn<ClientEditorEngine>();
			DependsOn<RenderCommonExport>();
			DependsOn<GameStateDebug>();
			DependsOn<EditorLauncher>();

			ReferenceAssembly( "System.Drawing" );

			UseThirdParty<D3D9>();
		}
	}
}