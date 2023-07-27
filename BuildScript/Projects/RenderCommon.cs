using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RenderCommon : BaseCppLibrary
	{
		public RenderCommon( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_ABSTRACTION;

			DependsOn<NDbTypes>();
			DependsOn<LibDBLoader>();
			//DependsOn<GameTools>();
			//DependsOn<CommonInit>();
			if (platform == PlatformType.Win32 || platform == PlatformType.Win64)
			{
				DependsOn<DataProvider>(); // для загрузки Material Layers / Shader cache
				DependsOn<ClrGhost>();
			}
			
			UseThirdParty<TaskScheduler>();
			UseThirdParty<CrnDecomp>();

			AddProjectFiles();
		}
	}
}