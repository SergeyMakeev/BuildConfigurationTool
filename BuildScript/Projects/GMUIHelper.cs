using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GMUIHelper : BaseCppLibrary
	{
		public GMUIHelper( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			UseThirdParty<PugiXML>();

			DependsOn<DataProvider>();
			DependsOn<VisualConstructor>();
			DependsOn<GameMechanics>();
			DependsOn<ServerConsole>();
		}
	}
}