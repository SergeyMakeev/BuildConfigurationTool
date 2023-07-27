using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ChangelogEditor : BaseCSharpExecutable
	{
		public ChangelogEditor( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ChangelogEditor";

			AddProjectFiles();

			applicationDefinition = "App.xaml";

			DependsOn<EditorControlsLite>();
			DependsOn<EditorControls>();
			DependsOn<DesignersDream>();

			ReferenceAssembly( "System.Xaml" );
		}
	}
}