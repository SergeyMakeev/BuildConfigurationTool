using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Replication : BaseCppLibrary
	{
		public Replication( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

            //пока не поправим в генераторе одинаковые static int __class_guid в каждом cpp
            unityBuildType = UnityBuildType.ALWAYS;

			DependsOn<Replica>();

			if ( platform == PlatformType.Orbis )
			{
				AdditionalCompilerOptions.Add( "-Wno-#pragma-messages" );
			}
		}
	}
}