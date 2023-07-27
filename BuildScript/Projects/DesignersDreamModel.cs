using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DesignersDreamModel : BaseCSharpLibrary
	{
		public DesignersDreamModel( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DesignersDreamModel";

			AddProjectFiles();

			DependsOn<EditorControls>();
			DependsOn<DesignersDreamGlobals>();

			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}