using System;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class ZLib : ThirdParty
	{
		public ZLib( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)zlib/include" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( string.Format( "%(VendorsDir)zlib/lib/{0}", Utilites.GetVendorsConfigurationName( configuration ) ) );
					project.Library("zlibstat");
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( string.Format( "%(VendorsDir)zlib/lib/x64/{0}", Utilites.GetVendorsConfigurationName( configuration ) ) );
					project.Library("zlibstat");
					break;
				}
				case PlatformType.Orbis:
				{
					project.LibrariesPath(string.Format("%(VendorsDir)zlib/lib/Orbis/{0}", Utilites.GetVendorsConfigurationName(configuration)));
					project.Library("zliborbis");
					break;
				}
				case PlatformType.Durango:
				{
					project.LibrariesPath(string.Format("%(VendorsDir)zlib/lib/Durango/{0}", Utilites.GetVendorsConfigurationName(configuration)));
					project.Library("zlibxbox");
					break;
				}
			}

			
		}
	}
}