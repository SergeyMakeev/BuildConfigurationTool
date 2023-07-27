using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class NvPerfKit : ThirdParty
	{
		public NvPerfKit( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)NvPerfKit/inc" );
		}
	}
}