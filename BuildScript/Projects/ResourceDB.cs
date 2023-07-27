using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDB : BaseCSharpLibrary
	{
		public ResourceDB( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			location = "%(ClientDir)ResourceDB";

			rootNamespace = "ResourceDB";

			AddProjectFiles();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Core" );
			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly( "Microsoft.CSharp" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Xml" );

			DependsOn<LoggerCli>();
			DependsOn<EditorCore>();
		}
	}
}