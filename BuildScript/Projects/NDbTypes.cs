using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class NDbTypes : BaseCppLibrary
	{
		public NDbTypes( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.RESOURCE_TYPES;
			unityBuildType = UnityBuildType.ALWAYS;

			Files("%(ClientDir)NDbTypes/*.cpp");
			Files("%(ClientDir)NDbTypes/stdafx.*");
			Files("%(ClientDir)NDbTypes/Files.h");

			IncludePath( "%(ClientDir)NDbTypes" );

			if (platform == PlatformType.Orbis)
			{
				AdditionalCompilerOptions.Add("-Wno-#pragma-messages");
			}

			DependsOn<LibDB>();
		}
	}
}