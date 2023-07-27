using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class CliClassesTests : BaseCSharpLibrary
	{
		public CliClassesTests( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "CliClassesTests";

			AddProjectFiles();

			DependsOn<DBGeneratedForTesting>();

			DependsOn<TestFramework>();
			DependsOn<CliClassesNativeTests>();

			ReferenceAssembly( "nunit.framework", @"%(VendorsDir)NUnit\bin\nunit.framework.dll" );
		}
	}
}