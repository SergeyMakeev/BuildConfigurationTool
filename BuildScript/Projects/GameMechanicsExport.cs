using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameMechanicsExport : BaseCliLibrary
	{
		public GameMechanicsExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<Export>();
		}
	}
}