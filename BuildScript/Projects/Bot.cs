using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Bot : BaseCppExecutable
	{
		public Bot( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();
			Files( @"%(ClientDir)json\jsoncpp.cpp" );

			//из за этих файлов в UB падает компилятор в vs2010
			UnityBuildIgnoreFiles("Context.cpp");
			UnityBuildIgnoreFiles("State.cpp");

			applicationKind = ApplicationKind.CONSOLE_APPLICATION;

			DependsOn<Logger>();
			DependsOn<Replica>();
			DependsOn<GameMechanics>();
			DependsOn<SrvConnection>();
			DependsOn<ServerConsole>();
			DependsOn<Application>();

			UseThirdParty<OpenSSL>();

			Define( "CAN_USE_WINDOWS_ONLY" );
		}
	}
}