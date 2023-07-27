using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DesignersDreamGlobals : BaseCSharpLibrary
	{
		public DesignersDreamGlobals( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DesignersDreamGlobals";

			AddProjectFiles();

			DependsOn<EditorControlsLite>();

			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}