using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditorMainLauncher : BaseCSharpExecutable
	{
		public EditorMainLauncher( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "MainLauncher";
			location = "%(ClientDir)MainLauncher";
			ApplicationIcon = "network-workgroup.ico";

			AddProjectFiles();

			applicationDefinition = "App.xaml";

			DependsOn<ClientTools>();
			DependsOn<DBCore>();
			DependsOn<EditorControls>();
			DependsOn<DesignersDream>();
			DependsOn<ModelEditorDll>();
			DependsOn<ExportEditor>();
			DependsOn<CinematicEditorDll>();
			DependsOn<ResourceDbWcfClient>();
			DependsOn<ResourceDbWcfService>();
			DependsOn<GameExport>();
			DependsOn<ExternalTools>();

			ReferenceAssembly( "SpeechRecognition", @"%(VendorsDir)SpeechRecognition\SpeechRecognition.dll" );
			ReferenceAssembly( "Hessiancsharp", @"%(VendorsDir)HessianCSharp\bin\Debug\Hessiancsharp.dll" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Runtime.Remoting" );
			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Data.DataSetExtensions" );
			ReferenceAssembly( "UIAutomationClient" );
			ReferenceAssembly( "UIAutomationTypes" );
			ReferenceAssembly( "WindowsFormsIntegration" );
			ReferenceAssembly( "System.Windows.Forms" );
		}
	}
}