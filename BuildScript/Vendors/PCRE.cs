using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class PCRE : ThirdParty
	{
		public PCRE( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)PCRE/Build/Lib/Win32/Msvc10/FinalRelease" );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)PCRE/Build/Lib/x64/Msvc10/FinalRelease" );
					break;
				}
				case PlatformType.Orbis:
				{
					project.LibrariesPath("%(VendorsDir)PCRE/Build/Lib/ORBIS/vs2012/FinalRelease");
					break;
				}
				case PlatformType.Durango:
				{
					project.LibrariesPath("%(VendorsDir)PCRE/Build/Lib/Durango/Msvc14/FinalRelease");
					break;
				}
			}

			project.Library( "pcre16" );
		}
	}
}