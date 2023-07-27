using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDbWcfService : BaseCSharpLibrary
	{
		public ResourceDbWcfService( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "Editor.Wcf";

			AddProjectFiles();

			DependsOn<DBGeneratedClientClasses>();
			DependsOn<CinematicEditorDll>();
			DependsOn<MayaRawExport>();
			DependsOn<ClientEditorBridge>();

			ReferenceAssembly( "System.ServiceModel" );
		}
	}
}