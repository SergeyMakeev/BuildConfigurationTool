using BCT.Source.Model;
using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source;

namespace BCT.BuildScript.Projects
{
	public class Application : BaseCppLibrary
	{
		public Application(Workspace workSpace, PlatformType platform, Configuration configuration)
			: base( workSpace, platform, configuration )
		{
			layer = Layer.APPLICATION;

			if ( platform == PlatformType.Durango )
			{
				enableExceptionHandling = true;
                SdkReference(SDKReference.XboxServices);
			}

			if ( platform == PlatformType.Win32 || platform == PlatformType.Win64 )
			{
				Library( string.Format( "%(ClientDir)Application/Windows/DDK/{0}/hid", Utilites.GetPlatformNameSharp( platform ) ) );
				
				UseThirdParty<D3D9>(); //для дурацкого GetAdapterMonitor из за gfx_adapter_id
				UseThirdParty<Steam>();
			}

			if (platform == PlatformType.Durango)
            {
            	Library("EtwPlus");

                Define("EARLY_ACCESS_SUPPORT");
            }

			if ( platform == PlatformType.Orbis )
			{
				Library("libSceUserService_stub_weak");
				
				Library("-lSceNpCommon_stub_weak");
				Library("-lSceNpManager_stub_weak");
				Library("-lSceNpAuth_stub_weak");
				Library("-lSceNpWebApi_stub_weak");
				Library("-lSceNpToolkit2_stub_weak");
				Library("-lSceAvPlayer_stub_weak");
				Library("-lSceSaveData_stub_weak");

                AdditionalCompilerOptions.Add("-Wno-deprecated-declarations");
			}

			DependsOn<Platform>();
			DependsOn<Logger>();
			DependsOn<Tools>();
			DependsOn<GameTools>();
            DependsOn<DataProvider>();
			DependsOn<Replica>();
            DependsOn<RenderD3D12>();

            if (platform == PlatformType.Durango)
            {
                DependsOn<Net>();
            }

            AddPlatformSpecificProjectFiles( platform );
			AddPlatformSpecificProjectResources( platform );



            
		}
	}
}