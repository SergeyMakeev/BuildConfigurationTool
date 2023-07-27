using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DataProvider : BaseCppLibrary
	{
		public DataProvider( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.FOUNDATION;

			if ( platform == PlatformType.Durango )
			{
				enableExceptionHandling = true;
			}

			UseThirdParty<ZLib>();
			UseThirdParty<Starforce>();

			filesRecuriveSearch = false;
			AddProjectFiles();
			AddPlatformSpecificProjectFiles(platform);

			DependsOn<Tools>();
			DependsOn<Global>();

			if (platform == PlatformType.Orbis)
			{
				Library("-lScePlayGo_stub_weak");
			}

		}
	}
}