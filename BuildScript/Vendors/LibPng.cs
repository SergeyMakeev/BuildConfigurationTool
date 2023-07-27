using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
    public class LibPng : ThirdParty
    {
        public LibPng(ProjectFile project, PlatformType platform, Configuration configuration)
            : base(project, platform, configuration)
        {

            project.IncludePath("%(VendorsDir)Scaleform/3rdParty/libpng-1.5.13/");

            string oLibPostfix = "";
            string oLibCfgPostfix = "";

            switch (platform)
            {
                case PlatformType.Win32:
                    oLibPostfix = "Win32/Msvc10/";
                    break;
                case PlatformType.Win64:
                    oLibPostfix = "x64/Msvc10/";
                    break;
                case PlatformType.Orbis:
                    oLibPostfix = "Orbis/Msvc11/";
                    break;
                case PlatformType.Durango:
                    oLibPostfix = "XboxOne/XDK/Msvc14/";
                    break;
                default:
                    throw new NotSupportedException("Unknown platform");
            }

            switch (configuration.target)
            {
                case Configuration.Target.DEBUG:
                    oLibCfgPostfix = "Debug";
                    break;
                case Configuration.Target.RELEASE:
                    oLibCfgPostfix = "Release";
                    break;
                case Configuration.Target.FINALRELEASE:
                    oLibCfgPostfix = "Release";
                    break;
                default:
                    throw new NotSupportedException("Unknown configuration target");
            }

            string otherLibPart = string.Format("Lib/{0}{1}", oLibPostfix, oLibCfgPostfix);

            project.LibrariesPath("%(VendorsDir)Scaleform/3rdParty/libpng-1.5.13/" + otherLibPart);

            project.Library("libpng");
        }
    }
}