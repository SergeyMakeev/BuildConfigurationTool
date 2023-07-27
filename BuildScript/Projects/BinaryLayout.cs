using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class BinaryLayout : BaseCppLibrary
	{
		public BinaryLayout( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			workSpace.ExecutePrebuildCommand( "%(BinDir)CodeGenerators/BinaryLayout.exe",
																				"-workDir:%(ClientDir)/BinaryLayout/ -saveDir:%(ClientDir)BuildSources/BinaryLayout/", "%(BinDir)CodeGenerators/");

			layer = Layer.RESOURCE_TYPES;

			AddProjectFiles();

			Files( "%(ClientDir)BuildSources/BinaryLayout/*.h" );
			Files( "%(ClientDir)BuildSources/BinaryLayout/Includers/LayoutIncluder*.cpp" );

			DependsOn<Tools>();
		}
	}
}