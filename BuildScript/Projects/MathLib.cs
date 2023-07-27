using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MathLib : BaseCppLibrary
	{
		public MathLib( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.FOUNDATION;

			AddProjectFiles();

			DependsOn<Logger>();
		}
	}
}