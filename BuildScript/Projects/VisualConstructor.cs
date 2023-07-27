using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class VisualConstructor : BaseCppLibrary
	{
		public VisualConstructor( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Replica>();
			DependsOn<Scene3D>();
		}
	}
}