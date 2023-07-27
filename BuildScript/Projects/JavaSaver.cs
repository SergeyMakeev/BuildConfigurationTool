using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class JavaSaver : BaseCppLibrary
	{
		public JavaSaver( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();

			DependsOn<LibDB>();
			DependsOn<LibDBLoader>();
		}
	}
}