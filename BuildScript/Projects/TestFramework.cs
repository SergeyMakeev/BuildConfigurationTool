using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class TestFramework : BaseCSharpLibrary
	{
		public TestFramework( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "TestFramework";

			AddProjectFiles();

			DependsOn<ClientTools>();
			DependsOn<LoggerCli>();
			DependsOn<DBCore>();
			DependsOn<ResourceDB>();
			DependsOn<CliTestSupport>();

			ReferenceAssembly( "nunit.framework", @"%(VendorsDir)NUnit\bin\nunit.framework.dll" );
		}
	}
}