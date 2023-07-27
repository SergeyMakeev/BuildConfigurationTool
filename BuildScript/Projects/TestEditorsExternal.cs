using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class TestEditorsExternal : BaseCSharpLibrary
	{
		public TestEditorsExternal( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "TestEditorsExternal";

			AddProjectFiles();

			DependsOn<EditorControls>();

			ReferenceAssembly( "nunit.framework", @"%(VendorsDir)NUnit\bin\nunit.framework.dll" );
		}
	}
}