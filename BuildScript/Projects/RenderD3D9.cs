using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{


	////////////////////////////////////////////////////////////////////////////////////////////////
	public class RenderD3D9 : BaseCppLibrary
	{
        public RenderD3D9(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;
			if (platform != PlatformType.Win32 && platform != PlatformType.Win64)
			{
				excludeFromSolution = true;
			}

			UseThirdParty<D3D9>();
			UseThirdParty<NvApi>();
			UseThirdParty<EasyHook>();
			UseThirdParty<NvPerfKit>();
			UseThirdParty<AmdDisplayLibrary>();
			UseThirdParty<TaskScheduler>();

			AddProjectFiles();
			DependsOn<GameTools>();
			DependsOn<NDbTypes>();
			DependsOn<RenderCommon>();
			DependsOn<Tools>();
		}
	}


}
