using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class NvApi : ThirdParty
	{
		public NvApi( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)NvAPI" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)NvAPI/x86" );
					project.Library( "nvapi" );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)NvAPI/amd64" );
					project.Library( "nvapi64" );
					break;
				}
			//	default:
			//		throw new NotSupportedException();
			}
		}
	}
}