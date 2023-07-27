using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ExportEditor : BaseCSharpLibrary
	{
		public ExportEditor( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ExportEditor";

			AddProjectFiles();

			DependsOn<DBGeneratedClientClasses>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<ResourceDBUtils>();
			DependsOn<EditorLauncher>();
			DependsOn<RenderCommonExport>();

			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "WindowsFormsIntegration" );
		}
	}
}