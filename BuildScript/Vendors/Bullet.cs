using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Bullet : ThirdParty
	{
		public Bullet( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)Bullet/src" );
			project.LibrariesPath( "%(VendorsDir)Bullet/lib" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					if ( configuration.UseDebugVendors() )
					{
						project.Library( "BulletCollision_vs2010_debug" );
						project.Library( "BulletDynamics_vs2010_debug" );
						project.Library( "BulletSoftBody_vs2010_debug" );
						project.Library( "LinearMath_vs2010_debug" );
					}
					else
					{
						project.Library( "BulletCollision_vs2010" );
						project.Library( "BulletDynamics_vs2010" );
						project.Library( "BulletSoftBody_vs2010" );
						project.Library( "LinearMath_vs2010" );
					}
					break;
				}
				case PlatformType.Win64:
				{
					if (configuration.UseDebugVendors())
					{
						project.Library( "BulletCollision_vs2010_x64_debug" );
						project.Library( "BulletDynamics_vs2010_x64_debug" );
						project.Library( "BulletSoftBody_vs2010_x64_debug" );
						project.Library( "LinearMath_vs2010_x64_debug" );
					}
					else
					{
						project.Library( "BulletCollision_vs2010_x64_release" );
						project.Library( "BulletDynamics_vs2010_x64_release" );
						project.Library( "BulletSoftBody_vs2010_x64_release" );
						project.Library( "LinearMath_vs2010_x64_release" );
					}
					break;
				}
				default:
					throw new NotSupportedException();
			}
		}
	}
}