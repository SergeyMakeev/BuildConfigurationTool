using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class CrashRpt : ThirdParty
	{
		public CrashRpt( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.IncludePath( "%(VendorsDir)CrashRpt/include" );
					project.LibrariesPath( "%(VendorsDir)CrashRpt/lib" );
					project.Library(configuration.UseDebugVendors() ? "CrashRpt1402d" : "CrashRpt1402");
					break;
				}
				case PlatformType.Win64:
				{
					project.IncludePath( "%(VendorsDir)CrashRpt/include" );
					project.LibrariesPath( "%(VendorsDir)CrashRpt/lib/x64" );
					project.Library(configuration.UseDebugVendors() ? "CrashRpt1402d" : "CrashRpt1402");
					break;
				}
				case PlatformType.Durango:
					break;
			//	default:
			//		throw new NotSupportedException();
			}
		}
	}
}