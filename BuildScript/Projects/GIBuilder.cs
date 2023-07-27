using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class GIBuilder : BaseCSharpExecutable
	{
		public GIBuilder( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "GIBuilder";

			applicationDefinition = "App.xaml";

			AddProjectFiles();

			DependsOn<ClientToolsCSharp>();
			DependsOn<EditorControlsLite>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Management" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.ServiceModel" );
		}
	}
}