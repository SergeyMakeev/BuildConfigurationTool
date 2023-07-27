using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class VisualObjectRuntime : BaseCppLibrary
	{
		public VisualObjectRuntime( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();

			DependsOn<GameTools>();
			DependsOn<VisualFlowRuntime>();
			DependsOn<RenderCommon>();
		}
	}
}