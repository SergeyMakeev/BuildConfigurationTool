using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Starforce : ThirdParty
	{
		public Starforce( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			if (platform == PlatformType.Win32 || platform == PlatformType.Win64)
			{
				project.IncludePath("%(VendorsDir)StarForce/SDK");
				switch (platform)
				{
					case PlatformType.Win32:
						{
							project.LibrariesPath("%(VendorsDir)StarForce/SDK");
							break;
						}
					case PlatformType.Win64:
						{
							project.LibrariesPath("%(VendorsDir)StarForce/SDK/x64");
							break;
						}
				}

				project.Library("vepaxeba");
			}

		}
	}
}