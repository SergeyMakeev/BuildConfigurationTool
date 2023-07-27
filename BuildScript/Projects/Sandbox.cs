using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;
using BCT.Source;

namespace BCT.BuildScript.Projects
{
	public class Sandbox : BaseCppExecutable
	{
		public Sandbox(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.APPLICATION;
			applicationKind = ApplicationKind.WINDOWED_APPLICATION;
			
			if (configuration.target == Configuration.Target.FINALRELEASE)
			{
				excludeFromSolution = true;
			}

			AddPlatformSpecificProjectFiles(platform);
			AddPlatformSpecificProjectResources(platform);

			if (platform == PlatformType.Orbis)
			{
				Library( "-lSceNpCommon_stub_weak" );
				Library( "-lSceNpManager_stub_weak" );
				Library( "-lSceNpAuth_stub_weak" );
				Library( "-lSceNpWebApi_stub_weak" );
				Library( "-lSceHttp_stub_weak" );
				Library( "-lSceSsl_stub_weak" );
				Library("-lSceSecure_stub_weak");
				Library("-lSceSysmodule_stub_weak");
				Library("-lSceNpCommon_stub_weak");
				Library("-lSceNpManager_stub_weak");
				Library("-lSceNpAuth_stub_weak");
				Library("-lSceUserService_stub_weak");
			}

			DependsOn<Logger>();
			DependsOn<MathLib>();
			DependsOn<Application>();
			DependsOn<Tools>();
			DependsOn<Global>();
			DependsOn<LibDB>();
			DependsOn<NDbTypes>();
			DependsOn<GameEvents>();
			DependsOn<GameTools>();
			DependsOn<Input>();
			DependsOn<RenderCommon>();
			DependsOn<VisualFlowRuntime>();
			DependsOn<VisualObjectRuntime>();
			DependsOn<Animation>();
			DependsOn<GICommon>();
			DependsOn<Scene3D>();
			DependsOn<RenderGnm>();
			DependsOn<GameController>();
			DependsOn<RenderD3D9>();
			DependsOn<RenderD3D12>();
			DependsOn<Terrain>();
			DependsOn<Replication>();
			DependsOn<Sound>();
			DependsOn<Collision>();
			DependsOn<GameMechanics>();
			DependsOn<RenderUtilsGnm>();
            DependsOn<RenderUtilsD3D12X>();
			DependsOn<MorphemeAnimation>();
			DependsOn<Scene>();
			DependsOn<Map>();
			DependsOn<VisualScripts>();
			DependsOn<GameController>();
			DependsOn<SrvConnection>();
			DependsOn<Scaleform>();
			
			UseThirdParty<TaskScheduler>();

			fileMapping = Utilites.GetFileMappingForProject(this, workSpace);
		}
	}
}