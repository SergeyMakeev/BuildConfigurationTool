using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ParticleFX : BaseCppLibrary
	{
		public ParticleFX( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<Scene3D>();
			DependsOn<VisualScripts>();
			DependsOn<ClientEditorNative>();

			IncludePath( "%(ClientDir)ParticleFX" );
		}
	}
}