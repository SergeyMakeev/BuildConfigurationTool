using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Umbra : ThirdParty
	{
		public Umbra( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)umbra/interface" );

			var configName = configuration.UseDebugVendors() ? "_d" : "";

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)umbra/lib/win32" );
					project.Library( string.Format( "umbraob32{0}", configName ) );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)umbra/lib/win64" );
					project.Library( string.Format( "umbraob64{0}", configName ) );
					break;
				}
				default:
					throw new NotSupportedException();
			}
		}
	}
}