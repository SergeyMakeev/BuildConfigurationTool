
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Steam : ThirdParty
	{
		public Steam(ProjectFile project, PlatformType platform, Configuration configuration)
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)SteamSDK/public/steam" );

			switch (platform)
			{
				case PlatformType.Win32:
					project.LibrariesPath("%(VendorsDir)SteamSDK/redistributable_bin");
					project.Library("steam_api");
					break;
				case PlatformType.Win64:
					project.LibrariesPath("%(VendorsDir)SteamSDK/redistributable_bin/win64");
					project.Library("steam_api64");
					break;
			}
		}
	}
}