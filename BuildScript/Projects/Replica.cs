using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Replica : BaseCppLibrary
	{
		public Replica( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			unityBuildType = UnityBuildType.ALWAYS;

			DependsOn<NDbTypes>();
			DependsOn<GameEvents>();
			DependsOn<JavaSaver>();
			DependsOn<LibDBLoader>();

			if ( platform == PlatformType.Orbis )
			{
				AdditionalCompilerOptions.Add( "-Wno-#pragma-messages" );
			}
		}
	}
}