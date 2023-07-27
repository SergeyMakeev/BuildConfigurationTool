using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DebugGUI : BaseCSharpLibrary
	{
		public DebugGUI( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DebugGUI";

			AddProjectFiles();

			DependsOn<EditorControlsLite>();
			DependsOn<EditorControls>();
			DependsOn<DebugInfoCli>();

			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xaml" );

			ReferenceAssembly( "DynamicDataDisplay", @"%(VendorsDir)WheresMyMemory\Vendors\Microsoft.Research\DynamicDataDisplay.dll" );
			ReferenceAssembly( "DynamicDataDisplay.Controls",
												 @"%(VendorsDir)WheresMyMemory\Vendors\Microsoft.Research\DynamicDataDisplay.Controls.dll" );
		}
	}
}