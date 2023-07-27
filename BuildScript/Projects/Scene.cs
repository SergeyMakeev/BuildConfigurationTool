using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Scene : BaseCppLibrary
	{
		public Scene( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();

			DependsOn<Terrain>();
			DependsOn<Sound>();
			DependsOn<Sky>();
			DependsOn<MorphemeAnimation>();
			DependsOn<ExternalProcesses>();
		}
	}
}