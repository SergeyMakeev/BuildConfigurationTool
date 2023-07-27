using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;
using BCT.BuildScript.Vendors;

namespace BCT.BuildScript.Projects
{
	public class ClientEditorNative : BaseCppLibrary
	{
		public ClientEditorNative( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

            UseThirdParty<D3D9>();

            Library("Vfw32");
            Library("Ole32");

			DependsOn<Scene>();
			DependsOn<VisualScripts>();
			DependsOn<GameState>();
			DependsOn<DebugGeometry>();
            DependsOn<RenderD3D9>();
		}
	}
}