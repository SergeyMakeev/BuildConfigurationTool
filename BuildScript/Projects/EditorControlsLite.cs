using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditorControlsLite : BaseCSharpLibrary
	{
		public EditorControlsLite( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			location = "%(ClientDir)EditorControlsLite";

			rootNamespace = "EditorControlsLite";

			AddProjectFiles();
			//Files("EditorIndex/Transport/Contracts.cs");

			ReferenceAssembly( "PresentationCore" );
			ReferenceAssembly( "PresentationFramework" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly( "System.Data.DataSetExtensions" );
			ReferenceAssembly( "Microsoft.CSharp" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Xml" );

			ReferenceAssembly( "System.Windows.Interactivity", "%(VendorsDir)Mvvm Light Toolkit/WPF4/System.Windows.Interactivity.dll" );
			ReferenceAssembly( "WPFToolkit.Extended", "%(VendorsDir)WPFToolKit/WPFToolkit.Extended.dll" );
			ReferenceAssembly( "PropertyChangedNotificator", @"%(VendorsDir)PropertyChangedNotificator\Bin\PropertyChangedNotificator.dll" );

			DependsOn<EditorCore>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<ResourceDBUtils>();
			DependsOn<LoggerCli>();
			DependsOn<EditorLauncher>();
		}
	}
}