using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class LatencyMeterUI : BaseCSharpExecutable
	{
		public LatencyMeterUI( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "LatencyMeterUI";

			AddProjectFiles();
			applicationDefinition = "App.xaml";

			DependsOn<EditorControlsLite>();

			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Windows.Forms" );
		}
	}
}