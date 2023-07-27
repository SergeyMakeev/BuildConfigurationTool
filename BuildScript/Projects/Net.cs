using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Net : BaseCppLibrary
	{
		public Net( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_ABSTRACTION;

			if ( platform.IsWindows() )
			{
				UseThirdParty<Starforce>();
				UseThirdParty<OpenSSL>();
                UseThirdParty<LibTom>();
                Library("ws2_32");
			}
            else if (platform == PlatformType.Orbis)
            {
                Library("-lSceNet_stub_weak");
                Library("-lSceRtc_stub_weak");
            }
            else if ( platform == PlatformType.Durango )
            {
                Library("bcrypt");
            }

			AddPlatformSpecificProjectFiles(platform);
            Files("%(ClientDir)Net/NetInitializer.h");
            Files("%(ClientDir)Net/NetInitializer.cpp");

            if (platform == PlatformType.Durango)
            {
                Files("%(ClientDir)Net/Windows/ConnectionContext.cpp");
                Files("%(ClientDir)Net/Windows/ConnectionContext.h");
                Files("%(ClientDir)Net/Windows/ConnectionControlState.h");
                Files("%(ClientDir)Net/Windows/ConnectionImpl.cpp");
                Files("%(ClientDir)Net/Windows/ConnectionImpl.h");
                Files("%(ClientDir)Net/Windows/ConnectionNetState.cpp");
                Files("%(ClientDir)Net/Windows/ConnectionNetState.h");
                Files("%(ClientDir)Net/Windows/DuplexQueue.cpp");
                Files("%(ClientDir)Net/Windows/DuplexQueue.h");
                Files("%(ClientDir)Net/Windows/EventPool.h");
                Files("%(ClientDir)Net/Windows/IocpQueue.cpp");
                Files("%(ClientDir)Net/Windows/IocpQueue.h");
                Files("%(ClientDir)Net/Windows/IocpUtils.cpp");
                Files("%(ClientDir)Net/Windows/IocpUtils.h");
                Files("%(ClientDir)Net/Windows/NetAddressResolver.cpp");
                Files("%(ClientDir)Net/Windows/NetAddressResolver.h");
                Files("%(ClientDir)Net/Windows/NetListenerEvent.h");
                Files("%(ClientDir)Net/Windows/NetListenerImpl.cpp");
                Files("%(ClientDir)Net/Windows/NetListenerImpl.h");
                Files("%(ClientDir)Net/Windows/SocketUtils.cpp");
                Files("%(ClientDir)Net/Windows/SocketUtils.h");
                Files("%(ClientDir)Net/Windows/ThreadUtils.cpp");
                Files("%(ClientDir)Net/Windows/ThreadUtils.h");
            }

			DependsOn<Tools>();
			DependsOn<JavaSaver>();

            Define("NET_PROJECT");
		}
	}
}