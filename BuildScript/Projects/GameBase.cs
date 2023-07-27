using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;
using BCT.BuildScript.Vendors;

namespace BCT.BuildScript.Projects
{
	public class GameBase : BaseCppLibrary
	{
		public GameBase( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if ( ( platform == PlatformType.Win32 ) || ( platform == PlatformType.Win64 ) )
			{
				UseThirdParty<LibTom>();
			}
			else
			{
				excludeFromSolution = true;
			}

			AddProjectFiles();

			DependsOn<JavaSaver>();
			DependsOn<LatencyMeter>();
			DependsOn<IO>();
			DependsOn<GameTools>();
		}
	}
}