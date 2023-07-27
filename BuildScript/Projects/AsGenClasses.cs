using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class AsGenClasses : BaseCppLibrary
	{
		public AsGenClasses( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			workSpace.ExecutePrebuildCommand("%(BinDir)CodeGenerators/AsGenClasses.exe", "%(ClientDir)AsGenClasses/AsGenClasses.cfg --clear",
																				"%(ClientDir)AsGenClasses" );

            unityBuildType = UnityBuildType.NEVER;

			AddProjectFiles();

			Files( "%(ClientDir)BuildSources/AsGenClasses/gen/AsGlobalInclude_*.cpp" );
			Files( "%(ClientDir)BuildSources/AsGenClasses/gen/AsGenFactory*.cpp" );
			Files( "%(ClientDir)BuildSources/AsGenClasses/gen/AsGameMessages*.cpp" );
			Files( "%(ClientDir)BuildSources/AsGenClasses/gen/AsGlobalEnumConverters.cpp" );
			Files("%(ClientDir)BuildSources/AsGenClasses/gen/AsClassIdToReplicaId.cpp");			
			Files( "%(ClientDir)BuildSources/AsGenClasses/gen/AsAbonents.cpp" );

			DependsOn<Scaleform>();
			DependsOn<SrvConnection>();
			DependsOn<GMUIHelper>();
			
			preBuildEvent = @"
				if exist ""$(ProjectDir)DefBuildIgnores.txt"" goto copy_ignores
				if not exist ""$(IntDir)DefBuildIgnores.txt"" goto end
				del /q ""$(IntDir)DefBuildIgnores.txt""
				goto end
				:copy_ignores
				copy ""$(ProjectDir)DefBuildIgnores.txt"" ""$(IntDir)DefBuildIgnores.txt""
				:end
            ";
		}
	}
}