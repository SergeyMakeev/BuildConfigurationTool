using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class ATICubemapGen : ThirdParty
	{
		public ATICubemapGen( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)" );
			project.LibrariesPath( "%(VendorsDir)ATI_CubemapGen/lib" );

			project.Library( configuration.UseDebugVendors()
												 ? string.Format( "CubeMapGenLib-{0}-debug-MTDLL", Utilites.GetPlatformNameSharp( platform ) )
												 : string.Format( "CubeMapGenLib-{0}-MTDLL", Utilites.GetPlatformNameSharp( platform ) ) );
		}
	}
}