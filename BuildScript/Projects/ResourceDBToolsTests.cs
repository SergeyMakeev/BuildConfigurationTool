using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDBToolsTests : BaseCSharpLibrary
	{
		public ResourceDBToolsTests( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ResourceDBToolsTests";

			AddProjectFiles();

			DependsOn<DBGeneratedClientClasses>();
			DependsOn<TestFramework>();
			DependsOn<CliLibdbTools>();
			DependsOn<ResourceDBTools>();

			ReferenceAssembly( "nunit.framework", @"%(VendorsDir)NUnit\bin\nunit.framework.dll" );
		}
	}
}