using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GIPrecompute : BaseCppLibrary
	{
		public GIPrecompute( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			DependsOn<ClientEditorNative>();
			DependsOn<EnlightenGIPrecompute>();
			DependsOn<ExternalProcesses>();
			DependsOn<Main>();

			AddProjectFiles();
		}
	}
}