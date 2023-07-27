using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ExternalProcesses : BaseCppLibrary
	{
		public ExternalProcesses( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if (platform == PlatformType.Durango || platform == PlatformType.Orbis)
			{
				excludeFromSolution = true;
			}

			layer = Layer.ENGINE;

			AddProjectFiles();

			DependsOn<Tools>();
		}
	}
}