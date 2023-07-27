using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DesignersDreamDatabase : BaseCSharpLibrary
	{
		public DesignersDreamDatabase( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "DesignersDreamDatabase";

			AddProjectFiles();

			DependsOn<DBGeneratedClientClasses>();
			DependsOn<DesignersDreamModel>();

			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );

			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Drawing" );
		}
	}
}