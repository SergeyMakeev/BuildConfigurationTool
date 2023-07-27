using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class UI : BaseCppLibrary
	{
		public UI( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			workSpace.ExecutePrebuildCommand("%(BinDir)CodeGenerators/AsGlue.exe", "%(ClientDir)UI/AsGlue.cfg --clear-glue", "%(ClientDir)UI");

            //из за этих файлов в UB падает компилятор в vs2010
            UnityBuildIgnoreFiles("GraphicsSettingsControl.cpp");

			AddProjectFiles();
			AddPlatformSpecificProjectFiles(platform);

			Files( "%(ClientDir)UI/AsGlue.cfg" );
			Files( "%(ClientDir)UI/*.twin" );

			DependsOn<GameController>();
			DependsOn<AsGenClasses>();
			DependsOn<Input>();
		}
	}
}