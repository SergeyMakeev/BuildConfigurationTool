using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDbWcfClient : BaseCSharpLibrary
	{
		public ResourceDbWcfClient( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "Editor.Wcf";

			AddProjectFiles();

			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Xml.Linq" );
			ReferenceAssembly( "System.Data.DataSetExtensions" );
		}
	}
}