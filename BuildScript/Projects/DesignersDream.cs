using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DesignersDream : BaseCSharpExecutable
	{
		public DesignersDream( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DesignersDream";

			AddProjectFiles();

			applicationDefinition = "App.xaml";
			ApplicationIcon = "ddIcon.ico";

			DependsOn<EditorControlsLite>();

			DependsOn<EditorControls>();
			DependsOn<DesignersDreamDatabase>();
			DependsOn<ClientEditorEngine>();
			DependsOn<CinematicEditorDll>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Web.Extensions" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "WindowsFormsIntegration" );

			ReferenceAssembly( "WPFToolkit.Extended", "%(VendorsDir)WPFToolKit/WPFToolkit.Extended.dll" );
		}
	}
}