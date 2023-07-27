using System;
using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Platform : BaseCppLibrary
	{
		public Platform( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.FOUNDATION;
			if ( platform == PlatformType.Durango )
			{
				enableExceptionHandling = true;
			}

			AddPlatformSpecificProjectFiles( platform );
			AddPlatformSpecificProjectResources( platform );
			switch ( platform )
			{
				case PlatformType.Win32:
				case PlatformType.Win64:
				{
					UseThirdParty<CrashRpt>();
					UseThirdParty<LibTom>();
					UseThirdParty<D3D9>();
					UseThirdParty<AmdDisplayLibrary>();
					break;
				}

				case PlatformType.Durango:
				{
					UseThirdParty<LibTom>();
					break;
				}
			}

			if (platform == PlatformType.Orbis)
			{
				Library("-lSceCommonDialog_stub_weak");
				Library("-lSceMsgDialog_stub_weak");
				Library("-lSceSecure_stub_weak");
				Library("-lSceRtc_stub_weak");

				Library("libSceCoredump_stub_weak");
				Library("libSceGnmDriver_stub_weak");
				Library("libSceGnm");
				Library("libSceGnmx");
				Library("-lSceSystemService_stub");

				if (configuration.developerClient )
				{
					Library("-lSceMat_stub_weak");
					Library("-lSceDbg_stub_weak");
				}
				
				//TODO: fix me
				AdditionalCompilerOptions.Add("-Wno-deprecated-declarations");
			}
		}
	}
}