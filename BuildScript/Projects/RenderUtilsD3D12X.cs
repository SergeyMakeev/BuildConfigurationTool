using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RenderUtilsD3D12X : BaseCppLibrary
	{
        public RenderUtilsD3D12X(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;
			
			AddProjectFiles();
			DependsOn<BinaryLayout>();

            if (platform == PlatformType.Win64 || platform == PlatformType.Durango)
            {
                IncludePath("%(VendorsDir)DurangoPsoBaker/");
                if (platform == PlatformType.Win64)
                    Library("%(VendorsDir)DurangoPsoBaker/x64/Release/PsoBaker.lib");
                else
                {
                    if (configuration.UseDebugVendors())
                        Library("%(VendorsDir)DurangoPsoBaker/Durango/Debug/PsoBaker.lib");
                    else
                        Library("%(VendorsDir)DurangoPsoBaker/Durango/Release/PsoBaker.lib");
                }
            }
		}
	}
}