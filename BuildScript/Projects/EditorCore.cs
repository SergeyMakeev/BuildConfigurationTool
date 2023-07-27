using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditorCore : BaseCSharpLibrary
	{
		public EditorCore( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "EditorCore";

			AddProjectFiles();

			// !!! Не добавляйте сюда никаких зависимостей, кроме системных !!!
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "PresentationFramework.Aero" );
		}
	}
}