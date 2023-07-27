using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class UITools : BaseCSharpExecutable
	{
		public UITools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration, ApplicationKind.CONSOLE_APPLICATION )
		{
			rootNamespace = "UITools";

			AddProjectFiles();

			DependsOn<Export>();
			DependsOn<ClientEditorEngine>();
			DependsOn<ClientEditorManaged>();
			DependsOn<EditorLauncher>();

			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Drawing" );
		}
	}
}