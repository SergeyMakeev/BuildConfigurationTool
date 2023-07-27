using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class LibTom : ThirdParty
	{
		public LibTom( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			string platformPathPart;
			switch ( platform )
			{
				case PlatformType.Win32:
					platformPathPart = "";
					break;
				case PlatformType.Win64:
					platformPathPart = "/x64";
					break;
				case PlatformType.Durango:
					platformPathPart = "/Durango";
					break;
				case PlatformType.Orbis:
					throw new NotSupportedException(); // For orbis lib tom is not supported
				default:
					throw new NotSupportedException();
			}

			project.IncludePath("%(VendorsDir)LibTom/LibTomCrypt/src/headers");
			project.LibrariesPath(string.Format("%(VendorsDir)LibTom/Output{0}", platformPathPart));

			if ( configuration.UseDebugVendors() )
			{
				project.Library("LibTomCrypt_Debug");
				project.Library("LibTomMath_Debug");
			}
			else
			{
				project.Library("LibTomCrypt_Release");
				project.Library("LibTomMath_Release");
			}
		}
	}
}