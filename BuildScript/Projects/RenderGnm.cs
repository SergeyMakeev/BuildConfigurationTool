using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{


	////////////////////////////////////////////////////////////////////////////////////////////////
	public class RenderGnm : BaseCppLibrary
	{
		public RenderGnm(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;

			if (platform != PlatformType.Orbis)
			{
				excludeFromSolution = true;
			}

			UseThirdParty<TaskScheduler>();

			AddProjectFiles();
			DependsOn<NDbTypes>();
			DependsOn<RenderCommon>();
			DependsOn<Tools>();
			
			if (platform == PlatformType.Orbis)
			{
				Library( "-lSceGnm" );
				Library( "-lSceGnmDriver_stub_weak" );
				Library( "-lSceGpuAddress" );
				Library( "-lSceShaderBinary" );
				Library( "-lSceVideoOut_stub_weak" );

				if ( configuration.developerClient )
				{
					Library( "-lScePerf_stub_weak" );
					Library( "-lSceDbg_stub_weak" );
				}
			}
		}
	}
}
