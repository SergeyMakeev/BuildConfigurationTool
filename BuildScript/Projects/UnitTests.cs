using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;
using BCT.Source;

namespace BCT.BuildScript.Projects
{
	public class UnitTests : BaseCppExecutable
	{
		public UnitTests(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base(workSpace, platform, configuration)
		{
			layer = Layer.APPLICATION;
			applicationKind = ApplicationKind.CONSOLE_APPLICATION;
			
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
				Library("-lSceSecure_stub_weak");
                Library("-lSceRandom_stub_weak");
				Library("-lSceSysmodule_stub_weak");
				Library("-lSceNpCommon_stub_weak");
				Library("-lSceNpManager_stub_weak");
				Library("-lSceNpAuth_stub_weak");
				Library("-lSceUserService_stub_weak");
				Library("-lSceNet_stub_weak");
				Library("-lSceNetCtl_stub_weak");
				Library("-lSceHttp_stub_weak");
				Library("-lSceSsl_stub_weak");
                Library("-lSceNpWebApi_stub_weak");
			}

            if (platform.IsWindows() || platform == PlatformType.Durango)
            {
                Library("ws2_32");
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
			DependsOn<Terrain>();
			DependsOn<Replication>();
			DependsOn<Sound>();
			DependsOn<GameSound>();
			DependsOn<Collision>();
			DependsOn<GameMechanics>();
			DependsOn<MorphemeAnimation>();
			DependsOn<Scene>();
			DependsOn<Map>();
			DependsOn<VisualConstructor>();
			DependsOn<Net>();
			DependsOn<GMUIHelper>();
			DependsOn<Slon>();
			DependsOn<VisualScripts>();

			DependsOn<RenderUtilsD3D9>();
			DependsOn<RenderUtilsGnm>();
			DependsOn<RenderUtilsD3D12X>();

			DependsOn<RenderD3D9>();
			DependsOn<RenderGnm>();
			DependsOn<RenderD3D12>();

            DependsOn<Net>();
			
			UseThirdParty<TaskScheduler>();

			fileMapping = Utilites.GetFileMappingForProject(this, workSpace);

/*			if ( platform == PlatformType.Durango )
			{
				//TODO: fix me
				Exclude("NetListenerImplTests.cpp");
				Exclude("BigMessageHelperTests.cpp");
				Exclude("HighEndSeasonTests.cpp");
                Exclude("DummyProtocolContext.cpp");
            }*/
			
		}
	}
}