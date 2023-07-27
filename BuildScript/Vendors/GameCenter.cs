using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class GameCenter : ThirdParty
	{
		public GameCenter( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)GameCenter" );
		}
	}
}