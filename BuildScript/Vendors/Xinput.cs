using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Xinput : ThirdParty
	{
		public Xinput( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.LibrariesPath( "%(VendorsDir)Xinput/x86" );
					break;
				}
				case PlatformType.Win64:
				{
					project.LibrariesPath( "%(VendorsDir)Xinput/x64" );
					break;
				}
				default:
					throw new NotSupportedException();
			}
			
			project.Library( "Xinput9_1_0.lib" );
			
		}
	}
}