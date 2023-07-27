using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Animation : BaseCppLibrary
	{
		public Animation( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();

			DependsOn<GameTools>();
			DependsOn<VisualObjectRuntime>();
		}
	}
}