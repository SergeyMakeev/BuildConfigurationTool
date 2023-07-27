using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Enlighten : ThirdParty
	{
		public Enlighten( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			if (platform == PlatformType.Durango || platform == PlatformType.Orbis || !configuration.enableMutableLibDB)
			{
				// ничего не делаем (в FR нет никакого энлайтена)
			} else
			{
				project.IncludePath( "%(VendorsDir)Enlighten/Src/EnlightenAPI/Include" );
				project.IncludePath( "%(VendorsDir)Enlighten/Src/EnlightenAPI/LibSrc" );

				
				string platformName = platform.ToString();
				string vsName = VSVersion.CurrentVersion.ShortName;
				
				if ( VSVersion.CurrentVersion == VSVersion.Vs2010)
				{
					vsName = "2010";
				} 
				else  if ( VSVersion.CurrentVersion == VSVersion.Vs2012)
				{
					vsName = "2012";
				}
                else if (VSVersion.CurrentVersion == VSVersion.Vs2015)
                {
                    vsName = "2015";
                }
				
				project.LibrariesPath(string.Format("%(VendorsDir)Enlighten/Src/EnlightenAPI/Lib/{0}_{1}", platformName, vsName).ToString());
				
				if ( configuration.UseDebugVendors() )
				{
					project.Library( "Enlighten2_d" );
					project.Library( "EnlightenBake_d" );
					project.Library( "EnlightenHLRT_d" );
					project.Library( "EnlightenPrecomp2_d" );
					project.Library( "GeoBase_d" );
					project.Library( "GeoCore_d" );
					project.Library( "GeoRayTrace_d" );
					project.Library( "IntelTBB_d" );
					project.Library( "ZLib_d" );
				}
				else
				{
					project.Library( "Enlighten2" );
					project.Library( "EnlightenBake" );
					project.Library( "EnlightenHLRT" );
					project.Library( "EnlightenPrecomp2" );
					project.Library( "GeoBase" );
					project.Library( "GeoCore" );
					project.Library( "GeoRayTrace" );
					project.Library( "IntelTBB" );
					project.Library( "ZLib" );
				}
			}


		}
	}
}