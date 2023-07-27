using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class UITextureCreator : BaseCSharpExecutable
	{
		public UITextureCreator( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "UITextureCreator";

			AddProjectFiles();

			DependsOn<Export>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<ClientEditorEngine>();
			DependsOn<ClientEditorManaged>();
			DependsOn<EditorLauncher>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}