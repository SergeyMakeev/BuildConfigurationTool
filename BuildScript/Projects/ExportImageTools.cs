using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ExportImageTools : BaseCppLibrary
	{
        public ExportImageTools(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;
			if (platform == PlatformType.Win32 || platform == PlatformType.Win64)
			{
				UseThirdParty<Squish>();
            }
			else
			{
				excludeFromSolution = true;
			}

			AddProjectFiles();

			DependsOn<Tools>();
            DependsOn<IO>();
            DependsOn<Image>();
		}
	}
}