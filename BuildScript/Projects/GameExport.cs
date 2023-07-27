using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameExport : BaseCliLibrary
	{
		public GameExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			DependsOn<Export>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<RenderCommonExport>();
			DependsOn<ClientEditorEngine>();
			DependsOn<EditedTerrain>();
			DependsOn<RenderD3D9>();
			DependsOn<Map>();
			DependsOn<GameStateDebug>();
			DependsOn<RenderUtilsGnm>();
			DependsOn<EditorLauncher>();

			AddProjectFiles();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml");
			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly("ICSharpCode.SharpZipLib", @"%(VendorsDir)SharpZipLib\ICSharpCode.SharpZipLib.dll");

			UseThirdParty<TaskScheduler>();
			UseThirdParty<CrnLib>();
		}
	}
}