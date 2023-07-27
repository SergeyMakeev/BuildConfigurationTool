using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class SerializationTest : BaseCppExecutable
	{
		public SerializationTest( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;
			applicationKind = ApplicationKind.CONSOLE_APPLICATION;

			AddProjectFiles();

			DependsOn<JavaSaver>();
			DependsOn<Replica>();
			DependsOn<Platform>();

			Define( "CAN_USE_WINDOWS_ONLY" );
		}
	}
}