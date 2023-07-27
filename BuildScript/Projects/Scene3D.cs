using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Scene3D : BaseCppLibrary
	{
		public Scene3D( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			UseThirdParty<Snappy>();

			UseThirdParty<TaskScheduler>();

			AddProjectFiles();

			Exclude( "%(ClientDir)Scene3D/imgui/examples/*" );

			DependsOn<Animation>();
			DependsOn<RenderCommon>();
			DependsOn<GICommon>();
			DependsOn<Collision>();
            DependsOn<RenderD3D12>();
			//DependsOn<Physics>();

			if (platform == PlatformType.Win32 || platform == PlatformType.Win64)
			{
				Define( "UMBRA_ENABLED" );
				UseThirdParty<Umbra>();
			}
			
			if (configuration.enableMutableLibDB)
				DependsOn<EnlightenGIRuntime>();
		}
	}
}