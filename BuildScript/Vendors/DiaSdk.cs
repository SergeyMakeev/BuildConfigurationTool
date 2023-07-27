using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class DiaSdk : ThirdParty
	{
		public DiaSdk( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)DiaSDK/include" );
		}
	}
}