using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{


	////////////////////////////////////////////////////////////////////////////////////////////////
	public class RenderD3D12 : BaseCppLibrary
	{
		public RenderD3D12(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;
            if (platform != PlatformType.Durango)
            {
                excludeFromSolution = true;
            }
            else
            {
                enableExceptionHandling = true;
            }

			UseThirdParty<TaskScheduler>();

			AddProjectFiles();
			DependsOn<GameTools>();
			DependsOn<NDbTypes>();
			DependsOn<RenderCommon>();
			DependsOn<Tools>();

			IncludePath("%(VendorsDir)DurangoPsoBaker/");

			if (platform == PlatformType.Durango)
			{
				Library("d3d12_x.lib");
				Library("xg_x.lib");
				Library("pixEvt.lib");
                Library("mfplat.lib");
                Library("mfuuid.lib");
                Library("d3d11_x.lib");
                Library("mfreadwrite.lib");
			}
		}
	}


}
