using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class TestEditors : BaseCSharpLibrary
	{
		public TestEditors( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "TestEditors";

			AddProjectFiles();

			DependsOn<DBGeneratedClientClasses>();
			DependsOn<DBGeneratedServerClasses>();
			DependsOn<EditorControlsLite>();
			DependsOn<EditorControls>();
			DependsOn<TestFramework>();
			DependsOn<CliLibdbTools>();
			DependsOn<MapEditorDll>();

			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "Moq", @"%(VendorsDir)Moq\Moq.dll" );
			ReferenceAssembly( "nunit.framework", @"%(VendorsDir)NUnit\bin\nunit.framework.dll" );
		}
	}
}