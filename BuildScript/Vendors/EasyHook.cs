using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class EasyHook : ThirdParty
	{
		public EasyHook( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)EasyHook/Public" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)EasyHook/Public/Lib/x86" );
					project.Library( "EasyHook32" );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)EasyHook/Public/Lib/x64" );
					project.Library( "EasyHook64" );
					break;
				}
				//default:
				//	throw new NotSupportedException();
			}
		}
	}
}