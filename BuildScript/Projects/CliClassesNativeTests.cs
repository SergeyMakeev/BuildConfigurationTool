using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class CliClassesNativeTests : BaseCliLibrary
	{
		public CliClassesNativeTests( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<ClientTools>();
			DependsOn<DBGeneratedForTesting>();
			DependsOn<DBCore>();
		}
	}
}