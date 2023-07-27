using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class CrnLib : ThirdParty
	{
		public CrnLib(ProjectFile project, PlatformType platform, Configuration configuration)
			: base(project, platform, configuration)
		{

			project.IncludePath("%(VendorsDir)CrnLib/inc/");
			string devName = "VC9";

			string oLibDirPlatform = "";
			string oLibDirCfg = "";
			string oLibNamePart = "";

			switch (platform)
			{
				case PlatformType.Win32:
					oLibDirPlatform = "Win32";
					break;
				case PlatformType.Win64:
					oLibDirPlatform = "x64";
					break;
				default:
					throw new NotSupportedException("Unknown platform");
			}

			switch (configuration.target)
			{
				case Configuration.Target.DEBUG:
					oLibDirCfg = "Debug";
					oLibNamePart = "D";
					break;
				case Configuration.Target.RELEASE:
					oLibDirCfg = "Release";
					break;
				default:
					throw new NotSupportedException("Unknown configuration target");
			}

			string otherLibDir = string.Format("{0}/{1}/{2}/", devName, oLibDirCfg, oLibDirPlatform);
			project.LibrariesPath("%(VendorsDir)CrnLib/lib/" + otherLibDir);

			string otherLibName = string.Format("crnlib{0}_{1}_{2}", oLibNamePart, oLibDirPlatform, devName);
			project.Library(otherLibName);
		}
	}
}