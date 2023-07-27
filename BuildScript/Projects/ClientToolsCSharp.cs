using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClientToolsCSharp : BaseCSharpLibrary
	{
		public ClientToolsCSharp( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			location = "%(ClientDir)ClientToolsCSharp";

			rootNamespace = "ClientToolsCSharp";

			AddProjectFiles();

			DependsOn<LoggerCli>();
			DependsOn<EditorCore>();

			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly( "System.Data.DataSetExtensions" );
		}
	}
}