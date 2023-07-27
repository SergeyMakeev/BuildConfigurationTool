using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class UILocalizationCreator : BaseCSharpExecutable
	{
		public UILocalizationCreator( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "UILocalizationCreator";
			ApplicationIcon = "F1Edit.ico";

			AddProjectFiles();

			DependsOn<EditorControls>();
			DependsOn<ClientEditorEngine>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Data" );

			ReferenceAssembly( "ICSharpCode.SharpZipLib", @"%(VendorsDir)SharpZipLib\ICSharpCode.SharpZipLib.dll" );
		}
	}
}