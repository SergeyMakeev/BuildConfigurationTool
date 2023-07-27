using System;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class FMod : ThirdParty
	{
		public FMod( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			if ( ( platform == PlatformType.Win32 ) || ( platform == PlatformType.Win64 ) )
			{
				project.IncludePath( "%(VendorsDir)FMod/src/fmod4/win/version/api/inc;%(VendorsDir)FMod/src/fmod4/win/version/fmoddesignerapi/api/inc;%(VendorsDir)SoundHistoryPlayer" );
				project.LibrariesPath( @"%(VendorsDir)FMod/src/fmod4/win/version/api/lib" );
				project.LibrariesPath( @"%(VendorsDir)FMod/src/fmod4/win/version/fmoddesignerapi/api/lib" );

				string libSuffix = "";
				if ( platform == PlatformType.Win64 )
					libSuffix = "64";
				var exLibSuffix = "_vc";
				if ( platform == PlatformType.Win64 )
					exLibSuffix = "64_vc";

				if ( configuration.UseDebugVendors() )
				{
					var enableFmodLog = false;
					if ( enableFmodLog )
					{
						//project.Library( "fmod_eventD" + libSuffix );
						project.Library( "fmod_event_netD" + libSuffix );
						project.Library( "fmodexD" + exLibSuffix );
					}
					else
					{
						//project.Library( "fmod_eventR" + libSuffix );
						project.Library( "fmod_event_netR" + libSuffix );
						project.Library( "fmodexR" + exLibSuffix );
					}
					project.Library( string.Format( "%(VendorsDir)SoundHistoryPlayer/Debug/{0}/FMODWrapD", Utilites.GetPlatformName( platform ) ) );
				}
				else if ( configuration.target == Configuration.Target.RELEASE )
				{
					//project.Library( "fmod_eventR" + libSuffix );
					project.Library( "fmod_event_netR" + libSuffix );
					project.Library( "fmodexR" + exLibSuffix );
					project.Library( string.Format( "%(VendorsDir)SoundHistoryPlayer/Release/{0}/FMODWrapR", Utilites.GetPlatformName( platform ) ) );
				}
				else // FINALRELEASE
				{
					project.Library( "fmod_event" + libSuffix );
					project.Library( "fmodex" + exLibSuffix );
					// fmod_event_net.dll запрещена в FR, т.к. нужна только для удаленной отладки event'ов
					project.Library( string.Format( "%(VendorsDir)SoundHistoryPlayer/FinalRelease/{0}/FMODWrap", Utilites.GetPlatformName( platform ) ) );
				}
			}
			else if ( platform == PlatformType.Orbis )
			{
				project.IncludePath( "%(VendorsDir)FMod/src/fmod4/orbis/version/api/inc;%(VendorsDir)FMod/src/fmod4/orbis/version/fmoddesignerapi/api/inc" );
				project.LibrariesPath( @"%(VendorsDir)FMod/src/fmod4/orbis/version/api/lib" );
				project.LibrariesPath( @"%(VendorsDir)FMod/src/fmod4/orbis/version/fmoddesignerapi/api/lib" );

				project.Library("libfmodex");
				project.Library("libfmodevent");

				if (configuration.target != Configuration.Target.FINALRELEASE)
				{
					project.Library("libfmodeventnet");
				}

				project.Library( "libSceAudioOut_stub_weak" );
				project.Library( "libSceAudioIn_stub_weak" );
				project.Library( "libSceAjm_stub_weak" );
				project.Library( "libSceUserService_stub_weak" );
				project.Library( "libSceNet_stub_weak" );
			}
			else if ( platform == PlatformType.Durango )
			{
				project.IncludePath( @"%(VendorsDir)FMod/src/fmod4/durango/version/api/inc;%(VendorsDir)FMod/src/fmod4/durango/version/fmoddesignerapi/api/inc" );
				project.LibrariesPath( @"%(VendorsDir)FMod/src/fmod4/durango/version/api/lib" );
				project.LibrariesPath( @"%(VendorsDir)FMod/src/fmod4/durango/version/fmoddesignerapi/api/lib" );

				project.Library( "acphal.lib" );
				project.Library( "xaudio2.lib" );
				project.Library( "mmdevapi.lib" );

				if ( configuration.target == Configuration.Target.FINALRELEASE )
				{
					project.Library( "fmodex_static" );
					project.Library( "fmod_event_static" );
				}
				else if ( configuration.target == Configuration.Target.DEBUG )
				{
                    project.Library("fmodexD_static");
                    project.Library("fmod_eventD_static");
                    project.Library("fmod_event_netD_static");	
				}
                else
                {
                    project.Library("fmodex_static");
                    project.Library("fmod_event_static");
                    project.Library("fmod_event_net_static");
                }
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}
}