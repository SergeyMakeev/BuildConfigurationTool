using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
    public class ScaleformLib : ThirdParty
    {
        public ScaleformLib(ProjectFile project, PlatformType platform, Configuration configuration)
            : base(project, platform, configuration)
        {
            project.IncludePath("%(VendorsDir)Scaleform/Include");
            project.IncludePath("%(VendorsDir)Scaleform/Src");
            project.IncludePath("%(VendorsDir)Scaleform/Src/GFx");

			string sLibPostfix = "";	// scaleform lib Win32/x64/
			string zLibPostfix = "";	// zlib 
			string oLibPostfix = "";	// scaleform jpeg, png, pcre, etc...

			string sCfgPostfix = "";	// scaleform debugopt/debug/release/shipping
			string oLibCfgPostfix = "";	// zlib debug/release

			switch (platform)
			{
				case PlatformType.Win32:
					sLibPostfix = oLibPostfix = "Win32/Msvc10/";
					break;
				case PlatformType.Win64:
					sLibPostfix = oLibPostfix = "x64/Msvc10/";
					zLibPostfix = "x64/";
					break;
				case PlatformType.Orbis:
					sLibPostfix = "PS4/Msvc11/";
					oLibPostfix = "Orbis/Msvc11/";
					zLibPostfix = "Orbis/";
					break;
				case PlatformType.Durango:
					sLibPostfix = "XboxOne/XDK/Msvc14/";
					oLibPostfix = "XboxOne/XDK/Msvc14/";
					zLibPostfix = "Durango/";
					break;
				default:
					throw new NotSupportedException("Unknown platform");
			}

			switch (configuration.target)
			{
				case Configuration.Target.DEBUG:
					sCfgPostfix = "DebugOpt";
					oLibCfgPostfix = "Debug";
					break;
				case Configuration.Target.RELEASE:
					sCfgPostfix = "Release";
					oLibCfgPostfix = "Release";
					break;
				case Configuration.Target.FINALRELEASE:
					sCfgPostfix = "Shipping";
					oLibCfgPostfix = "Release";
					break;
				default:
					throw new NotSupportedException("Unknown configuration target");
			}

			string scaleformLibPart = string.Format("Lib/{0}{1}", sLibPostfix, sCfgPostfix);
			string otherLibPart = string.Format("Lib/{0}{1}", oLibPostfix, oLibCfgPostfix);

			project.LibrariesPath("%(VendorsDir)Scaleform/" + scaleformLibPart);
			project.LibrariesPath("%(VendorsDir)Scaleform/3rdParty/jpeg-8d/" + otherLibPart);
			project.LibrariesPath("%(VendorsDir)Scaleform/3rdParty/libpng-1.5.13/" + otherLibPart);
			project.LibrariesPath("%(VendorsDir)Scaleform/3rdParty/pcre/" + otherLibPart);
			project.LibrariesPath("%(VendorsDir)Scaleform/3rdParty/expat-2.1.0/" + otherLibPart);
			
			project.LibrariesPath(string.Format("%(VendorsDir)zlib/lib/{0}{1}", zLibPostfix, oLibCfgPostfix));

			switch (platform)
			{
				case PlatformType.Win32:
				case PlatformType.Win64:
					project.Library("Winmm");
					project.Library("libgfxplatform_d3d9");
					project.Library("libgfxrender_d3d9");
					project.Library("zlibstat");
					project.Library("libgfxime");
					project.Library("imm32");
					break;
				case PlatformType.Durango:
					project.Library("libgfxplatform_xboxone");
					project.Library("libgfxrender_d3d12");
					project.Library("d3d12_x.lib");
					project.Library("PIXEvt.lib");
					project.Library("xg_x.lib");
					project.Library("zlibxbox");
					break;
				case PlatformType.Orbis:
					project.Library("-lSceGnf");
					project.Library("-lSceGnm");
					project.Library("-lSceGnmx");
				 	project.Library("-lSceGnmDriver_stub_weak");
				 	project.Library("-lScePm4Dump");
				 	project.Library("-lSceShaderBinary");
				 	project.Library("-lSceGpuAddress");
				 	project.Library("-lSceSysmodule_stub_weak");

					project.Library("libgfxplatform_ps4");
					project.Library("libgfxrender_ps4");
					project.Library("libgfxshaders");

					project.Library("zliborbis");
					break;
			}

			project.Library("libjpeg");
			project.Library("libpng");
			project.Library("expat");
			project.Library("pcre");

			project.Library("libgfx");
			project.Library("libgfxexpat");
			project.Library("libgfx_air");
			project.Library("libgfx_as3");
        }
    }
}