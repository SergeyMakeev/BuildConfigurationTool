using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDBTools : BaseCSharpLibrary
	{
		public ResourceDBTools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ResourceDBTools";

			AddProjectFiles();

			DependsOn<ResourceDBNative>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<ResourceDBUtils>();
			DependsOn<CliLibdbTools>();
			DependsOn<EditorLauncher>();

			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly( "System.Data.DataSetExtensions" );
			ReferenceAssembly( "Microsoft.CSharp" );
			ReferenceAssembly( "System.Xml" );
		}
	}
}