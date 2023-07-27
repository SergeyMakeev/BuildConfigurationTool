using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class OpenSSL : ThirdParty
	{
		public OpenSSL( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)OpenSSL/include" );
			project.LibrariesPath( "%(VendorsDir)OpenSSL/lib" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.Library( "libeay32" );
					project.Library( "ssleay32" );
					break;
				}
				case PlatformType.Win64:
				{
					project.Library( "libeay64" );
					project.Library( "ssleay64" );
					break;
				}
				default:
					throw new NotSupportedException();
			}
		}
	}
}