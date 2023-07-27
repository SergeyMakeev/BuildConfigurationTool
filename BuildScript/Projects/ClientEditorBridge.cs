using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClientEditorBridge : BaseCliLibrary
	{
		public ClientEditorBridge( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<GameBase>();
			DependsOn<ClientTools>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<CliLibdbTools>();
			DependsOn<SceneExport>();
			DependsOn<ClientEditorNative>();
			DependsOn<ParticleFX>();
			DependsOn<Main>();

			ReferenceAssembly( "System.Drawing" );
		}
	}
}