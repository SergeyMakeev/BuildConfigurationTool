using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Vivox : ThirdParty
	{
		public Vivox( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)Vivox/include" );

			switch ( platform )
			{
			case PlatformType.Win32:
				project.LibrariesPath( "%(VendorsDir)Vivox/bin/win32" );
				project.Library( "vivoxsdk" );
				break;
			case PlatformType.Win64:
				project.LibrariesPath( "%(VendorsDir)Vivox/bin/win64" );
				project.Library( "vivoxsdk_x64" );
				break;
			case PlatformType.Orbis:
				project.LibrariesPath( "%(VendorsDir)Vivox/bin/orbis" );
				project.Library( "vivoxsdk" );
				project.Library( "-lSceNetCtl_stub_weak" );
				project.Library( "-lSceHttp_stub_weak" );
				project.Library( "-lSceSsl_stub_weak" );
				project.Library( "-lSceHmac" );
				project.Library( "-lSceSha1" );
				project.Library( "-lSceSha256" );
				break;
			case PlatformType.Durango:
				project.LibrariesPath( "%(VendorsDir)Vivox/bin/durango" );
				project.Library( configuration.UseDebugVendors() ? "vivoxsdkDebug1" : "vivoxsdk" );
				project.Library( "ws2_32" );
				break;
			}
		}
	}
}
