using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Simul : ThirdParty
	{
		public Simul( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.IncludePath("%(VendorsDir)SimulSDK");
					if ( configuration.UseDebugVendors() )
					{
						project.LibrariesPath( "%(VendorsDir)SimulSDK/Simul/lib/Win32/VC10/Static Debug" );
						project.Library( "SimulBase_MDd" );
						project.Library( "SimulClouds_MDd" );
						project.Library( "SimulMath_MDd" );
						project.Library( "SimulSky_MDd" );
						project.Library( "SimulMeta_MDd" );
						project.Library( "SimulGeometry_MDd" );
						project.Library( "SimulCamera_MDd" );
					}
					else
					{
						project.LibrariesPath( "%(VendorsDir)SimulSDK/Simul/lib/Win32/VC10/Static Release" );
						project.Library( "SimulBase_MD" );
						project.Library( "SimulClouds_MD" );
						project.Library( "SimulMath_MD" );
						project.Library( "SimulSky_MD" );
						project.Library( "SimulMeta_MD" );
						project.Library( "SimulGeometry_MD" );
						project.Library( "SimulCamera_MD" );
					}
					break;
				}
				case PlatformType.Win64:
				{
          project.IncludePath("%(VendorsDir)SimulSDK");
					if ( configuration.UseDebugVendors() )
					{
						project.LibrariesPath( "%(VendorsDir)SimulSDK/Simul/lib/x64/VC10/Static Debug" );
						project.Library( "SimulBase_MDd" );
						project.Library( "SimulClouds_MDd" );
						project.Library( "SimulMath_MDd" );
						project.Library( "SimulSky_MDd" );
						project.Library( "SimulMeta_MDd" );
						project.Library( "SimulGeometry_MDd" );
						project.Library( "SimulCamera_MDd" );
					}
					else
					{
						project.LibrariesPath( "%(VendorsDir)SimulSDK/Simul/lib/x64/VC10/Static Release" );
						project.Library( "SimulBase_MD" );
						project.Library( "SimulClouds_MD" );
						project.Library( "SimulMath_MD" );
						project.Library( "SimulSky_MD" );
						project.Library( "SimulMeta_MD" );
						project.Library( "SimulGeometry_MD" );
						project.Library( "SimulCamera_MD" );
					}
					break;
				}
				default:
					break;
			}
		}
	}
}