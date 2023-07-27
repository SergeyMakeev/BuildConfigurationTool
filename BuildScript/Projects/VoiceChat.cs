using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class VoiceChat : BaseCppLibrary
	{
		public VoiceChat( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			UseThirdParty<Vivox>();

			if ( platform == PlatformType.Orbis )
			{
				Library("libSceNpParty_stub_weak");
				Library("libSceAudioIn_stub_weak");
			}

			if ( platform == PlatformType.Durango )
			{
				Define( "ENABLE_VIVOX_WRAPPER" );
				enableExceptionHandling = true;
				SdkReference( SDKReference.XboxServices );
			}

			AddPlatformSpecificProjectFiles( platform );

			DependsOn<GameController>();
			DependsOn<GameEvents>();
			DependsOn<SrvConnection>();
		}
	}
}
