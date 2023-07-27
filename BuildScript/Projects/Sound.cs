using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Sound : BaseCppLibrary
	{
		public Sound( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			UseThirdParty<FMod>();

			AddProjectFiles();

			DependsOn<VisualObjectRuntime>();
			DependsOn<SpaceIndex>();
		}
	}
}