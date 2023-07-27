using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class IO : BaseCppLibrary
	{
		public IO( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.FOUNDATION;

			if (platform == PlatformType.Durango || platform == PlatformType.Orbis)
			{
				excludeFromSolution = true;
			}

			UseThirdParty<ZLib>();

			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<DataProvider>();
			DependsOn<ClrGhost>();
		}
	}
}