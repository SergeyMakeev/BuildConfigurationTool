using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GameState : BaseCppLibrary
	{
		public GameState( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			workSpace.ExecutePrebuildCommand("%(BinDir)CodeGenerators/AsGlue.exe", "%(ClientDir)GameState/AsGlue.cfg  --clear-glue", "%(ClientDir)GameState");

			UseThirdParty<PCRE>();

			if (platform.IsWindows())
			{
				UseThirdParty<GameCenter>();
				UseThirdParty<Steam>();
			}
			
			filesRecuriveSearch = false;
			AddProjectFiles();
			AddPlatformSpecificProjectFiles(platform);

			Files( "%(ClientDir)GameState/AsGlue.cfg" );
			Files( "%(ClientDir)GameState/*.twin" );

			DependsOn<CinematicCore>();
			DependsOn<VisualScripts>();
			DependsOn<UI>();
			DependsOn<LibDBLoader>();
			DependsOn<Application>();
			DependsOn<Image>();
			DependsOn<VoiceChat>();

			if ( platform == PlatformType.Durango )
            {
            	enableExceptionHandling = true;
                SdkReference(SDKReference.XboxServices);
                Library("ixmlhttprequest2.lib");
            }
			
			if (platform == PlatformType.Orbis)
			{
				Library("libSceImeDialog_stub_weak");
				Library("libSceNpProfileDialog_stub_weak");
				Library("libSceHttp_stub_weak");
				Library("libSceSsl_stub_weak");
				Library("libSceCommonDialog_stub_weak");
				Library("libSceNpCommerce_stub_weak");
				
				//TODO: fix me
				AdditionalCompilerOptions.Add("-Wno-deprecated-declarations");
			}
		}
	}
}