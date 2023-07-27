using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class AmdDisplayLibrary : ThirdParty
	{
		public AmdDisplayLibrary( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.IncludePath( "%(VendorsDir)AMD Display Library/include" );
					break;
				}
				case PlatformType.Win64:
				{
					project.IncludePath( "%(VendorsDir)AMD Display Library/include" );
					break;
				}
				default:
				{
					break;
				}
			}		
		}
	}
}