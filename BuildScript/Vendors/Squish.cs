using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Squish : ThirdParty
	{
		public Squish( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.LibrariesPath( "%(VendorsDir)LibSquish/lib" );

			var configName = configuration.UseDebugVendors() ? "Debug" : "Release";

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.Library( string.Format( "squish_{0}", configName ) );
					break;
				}
				case PlatformType.Win64:
				{
					project.Library( string.Format( "squish_{0}_x64", configName ) );
					break;
				}
				default:
					throw new NotSupportedException();
			}
		}
	}
}