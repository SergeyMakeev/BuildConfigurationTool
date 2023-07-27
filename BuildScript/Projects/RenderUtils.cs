using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RenderUtils : BaseCppLibrary
	{
		public RenderUtils( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_ABSTRACTION;

			AddProjectFiles();
			DependsOn<Logger>();
			DependsOn<Tools>();
			DependsOn<Platform>();
		}
	}
}