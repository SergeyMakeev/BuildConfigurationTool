using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Slon : BaseCppLibrary
	{
		public Slon( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			//из за этих файлов в UB падает компилятор в vs2010
			UnityBuildIgnoreFiles("VisualHolder.cpp");

			Exclude( "%(ClientDir)SLON/UnityBuild.cpp" );
			Exclude( "%(ClientDir)SLON/Private/UnityBuild.cpp" );

			DependsOn<GameTools>();
			DependsOn<Map>();
			DependsOn<GMUIHelper>();
			DependsOn<Input>(); //из за global_app_active
		}
	}
}