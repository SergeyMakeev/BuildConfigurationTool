using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class IC : ThirdParty
	{
		public IC( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)IC" );

			var configName = configuration.UseDebugVendors() ? "Debug" : "Release";
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( string.Format( "%(VendorsDir)IC/{0}", configName ) );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( string.Format( "%(VendorsDir)IC/x64/{0}", configName ) );
					break;
				}
				default:
					throw new NotSupportedException();
			}

			project.Library( "IC" );
		}
	}
}