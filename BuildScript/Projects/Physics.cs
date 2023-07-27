using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Physics : BaseCppLibrary
	{
		public Physics( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if (platform == PlatformType.Durango || platform == PlatformType.Orbis)
			{
				excludeFromSolution = true;
			}

			layer = Layer.ENGINE;

			UseThirdParty<Bullet>();
			UseThirdParty<Snappy>();

			AddProjectFiles();

			DependsOn<Collision>();
		}
	}
}