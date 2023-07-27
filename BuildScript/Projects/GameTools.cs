using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameTools : BaseCppLibrary
	{
		public GameTools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddPlatformSpecificProjectFiles( platform );

			DependsOn<Tools>();
			DependsOn<NDbTypes>();
			DependsOn<RenderCommon>(); // использует GlobalVars оттуда
		}
	}
}