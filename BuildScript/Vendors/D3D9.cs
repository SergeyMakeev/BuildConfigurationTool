using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class D3D9 : ThirdParty
	{
		public D3D9( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)d3d9/lib/x86" );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)d3d9/lib/x64" );
					break;
				}
				
			}
			if (platform == PlatformType.Win32 || platform == PlatformType.Win64)
			{
				project.IncludePath("%(VendorsDir)d3d9/Include");

				project.Library("d3d9");
				project.Library("d3dx9");
				project.Library("dxguid");
				project.Library("DxErr");
			}
		}
	}
}