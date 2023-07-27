using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditorLauncher : BaseCSharpLibrary
	{
		public EditorLauncher( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "Launcher";

			AddProjectFiles();

			DependsOn<EditorCore>();

			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
		}
	}
}