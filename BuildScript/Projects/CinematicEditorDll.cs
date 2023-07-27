using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class CinematicEditorDll : BaseCSharpLibrary
	{
		public CinematicEditorDll( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "CE";

			AddProjectFiles();

			DependsOn<ClientEditorManaged>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<Export>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<EditorControls>();
			DependsOn<ResourceDbWcfClient>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Windows.Forms" );

			ReferenceAssembly( "GongSolutions.Wpf.DragDrop",
												 @"%(VendorsDir)gong-wpf-dragdrop\GongSolutions.Wpf.DragDrop\bin\Release\NET4\GongSolutions.Wpf.DragDrop.dll" );
			ReferenceAssembly( "System.Windows.Interactivity", @"%(VendorsDir)Mvvm Light Toolkit\WPF4\System.Windows.Interactivity.dll" );
			ReferenceAssembly( "PropertyChangedNotificator", @"%(VendorsDir)PropertyChangedNotificator\Bin\PropertyChangedNotificator.dll" );
		}
	}
}