using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class FBX : ThirdParty
	{
		public FBX( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)FBX/include" );

			string libConfigurationDir = configuration.UseDebugVendors() ? "debug" : "release";

			string platformPathPart;
			switch ( platform )
			{
				case PlatformType.Win32:
					platformPathPart = "x86";
					break;
				case PlatformType.Win64:
					platformPathPart = "x64";
					break;
				default:
					throw new NotSupportedException();
			}

			project.LibrariesPath( string.Format( "%(VendorsDir)FBX/lib/{0}/{1}/{2}", VSVersion.CurrentVersion.ToString(), platformPathPart, libConfigurationDir ) );
			project.Library( "libfbxsdk" );
		}
	}
}