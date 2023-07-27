using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RenderUtilsGnm : BaseCppLibrary
	{
		public RenderUtilsGnm(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;
			
			AddProjectFiles();
			DependsOn<BinaryLayout>();

			bool orbisSDKFound = System.Environment.GetEnvironmentVariable("SCE_ORBIS_SDK_DIR") != null;
			if (orbisSDKFound)
			{
				Define("ORBIS_SDK_FOUND");
			}

			if (platform == PlatformType.Orbis)
			{
				Library("-lSceDbg_stub_weak");
				
				Library("-lSceShaderBinary");
				Library("-lSceGnmDriver_stub_weak");
				if (configuration.UseDebugVendors())
				{
					Library("-lSceGnmx_debug");
					Library("-lSceGnm_debug");
				}
				else
				{
					Library("-lSceGnmx");
					Library("-lSceGnm");
				}
			}
			else if (platform == PlatformType.Win64 && orbisSDKFound)
			{
                // если на компе(который билдит) стоит orbisSDK:
                if ( System.Environment.GetEnvironmentVariable( "SCE_ORBIS_SDK_DIR" ) == null )
                {
									excludeFromSolution = true;
                }
                else
                {
                    IncludePath(@"$(SCE_ORBIS_SDK_DIR)\target\include_common\");
                    Library(@"$(SCE_ORBIS_SDK_DIR)\host_tools\lib\libSceGnm.lib");
                    Library(@"$(SCE_ORBIS_SDK_DIR)\host_tools\lib\libSceGnmx.lib");
										Library(@"$(SCE_ORBIS_SDK_DIR)\host_tools\lib\libSceGpuAddress.lib");
                }
			}
		}
	}
}