using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ModelEditorDll : BaseCSharpLibrary
	{
		public ModelEditorDll( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ModelEditorDll";
			ApplicationIcon = "F1Edit.ico";

			AddProjectFiles();

			DependsOn<EditorControlsLite>();
			DependsOn<EditorControls>();
			DependsOn<ClientEditorBridge>();
			DependsOn<ClientEditorEngine>();
			DependsOn<MapEditorDll>();
			DependsOn<NaviMapExport>();

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

			ReferenceAssembly( "ActiproSoftware.Docking.Wpf", @"%(VendorsDir)WPFControls\ActiproSoftware.Docking.Wpf.dll" );
			ReferenceAssembly( "ActiproSoftware.Shared.Wpf", @"%(VendorsDir)WPFControls\ActiproSoftware.Shared.Wpf.dll" );
			ReferenceAssembly( "ActiproSoftware.Themes.Office.Wpf", @"%(VendorsDir)WPFControls\ActiproSoftware.Themes.Office.Wpf.dll" );
			ReferenceAssembly( "GalaSoft.MvvmLight.Extras.WPF4", @"%(VendorsDir)Mvvm Light Toolkit\WPF4\GalaSoft.MvvmLight.Extras.WPF4.dll" );
			ReferenceAssembly( "GalaSoft.MvvmLight.WPF4", @"%(VendorsDir)Mvvm Light Toolkit\WPF4\GalaSoft.MvvmLight.WPF4.dll" );
			ReferenceAssembly( "GongSolutions.Wpf.DragDrop", @"%(VendorsDir)gong-wpf-dragdrop\GongSolutions.Wpf.DragDrop\bin\Release\NET4\GongSolutions.Wpf.DragDrop.dll" );
			ReferenceAssembly( "Hessiancsharp", @"%(VendorsDir)HessianCSharp\bin\Debug\Hessiancsharp.dll" );
			ReferenceAssembly( "MySql.Data", @"%(VendorsDir)MySQL\MySql.Data.dll" );
			ReferenceAssembly( "PropertyChangedNotificator", @"%(VendorsDir)PropertyChangedNotificator\Bin\PropertyChangedNotificator.dll" );
			ReferenceAssembly( "System.Windows.Interactivity", @"%(VendorsDir)Mvvm Light Toolkit\WPF4\System.Windows.Interactivity.dll" );
			ReferenceAssembly( "WPFToolkit.Extended", @"%(VendorsDir)WPFToolKit\WPFToolkit.Extended.dll" );
		}
	}
}