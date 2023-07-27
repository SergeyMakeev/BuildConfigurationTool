using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameMechanics : BaseCppLibrary
	{
		public GameMechanics( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			//из за этих файлов в UB падает компилятор в vs2010
			UnityBuildIgnoreFiles("Entity.cpp");
			UnityBuildIgnoreFiles("GameMechanicsBase.cpp");

			DependsOn<Collision>();
			DependsOn<Replication>();
			DependsOn<GameBase>();
			DependsOn<Net>();

			if ( ( platform == PlatformType.Orbis ) || ( platform == PlatformType.Durango ) )
			{
				Define( "GM_HISTORY_SAVER_DISABLED" );
			}
			
			DependsOn<Application>(); //для PC таймчита
		}
	}
}