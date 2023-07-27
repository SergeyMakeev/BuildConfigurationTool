using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class IconCreator : BaseCSharpExecutable
	{
		public IconCreator( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "IconCreator";

			AddProjectFiles();

			DependsOn<ModelEditorDll>();

			ReferenceAssembly( "System.Xaml" );
		}
	}
}