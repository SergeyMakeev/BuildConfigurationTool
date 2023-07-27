using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameController : BaseCppLibrary
	{
		public GameController( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Slon>();
			DependsOn<Input>();
		}
	}
}