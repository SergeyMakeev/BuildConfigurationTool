using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Main : BaseCppLibrary
	{
		public Main( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.APPLICATION;

			UseThirdParty<CrashRpt>();

			switch (platform)
			{
			case PlatformType.Win32:
			case PlatformType.Win64:
				{
					UseThirdParty<Steam>();
					break;
				}
			}

			AddPlatformSpecificProjectFiles( platform );
			AddPlatformSpecificProjectResources( platform );

			DependsOn<GameState>();
		}
	}
}