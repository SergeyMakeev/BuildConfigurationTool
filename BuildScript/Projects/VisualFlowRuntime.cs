using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class VisualFlowRuntime : BaseCppLibrary
	{
		public VisualFlowRuntime( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();

			//если эти файлы добавлять в UB, будет internal compiler error на vs2010
			UnityBuildIgnoreFiles("ScriptHolder.cpp");
			UnityBuildIgnoreFiles("SplineStorageTable.cpp");

			DependsOn<NDbTypes>();
			DependsOn<GameTools>();
			DependsOn<DataProvider>();
			DependsOn<RenderCommon>();
		}
	}
}