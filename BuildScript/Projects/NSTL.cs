using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class NSTL : BaseCppLibrary
	{
		public NSTL( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = ApplicationKind.UTILITY;

			Files( string.Format( "{0}*.h", location ) );
		}
	}
}