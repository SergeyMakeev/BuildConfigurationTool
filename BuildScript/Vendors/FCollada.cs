using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class FCollada : ThirdParty
	{
		public FCollada( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)FCollada" );
			project.IncludePath( "%(VendorsDir)FCollada/LibXML/include" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)FCollada/Output" );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)FCollada/Output/x64" );
					break;
				}
				default:
					throw new NotSupportedException();
			}

			project.Library( configuration.UseDebugVendors() ? "FColladaSUD_MTD" : "FColladaSUR_MTD" );
		}
	}
}