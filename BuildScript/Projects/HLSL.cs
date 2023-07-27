using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class HLSL : BaseCppLibrary
	{
		public HLSL( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = ApplicationKind.UTILITY;

			Files( "%(ClientDir)HLSL/*.hlsl" );
			Files( "%(ClientDir)HLSL/*.h" );
		}
	}
}