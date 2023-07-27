using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RenderCommonExport : BaseCliLibrary
	{
		public RenderCommonExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<ClientEditorNative>();
			DependsOn<ExportImageTools>();
			DependsOn<Export>();
			DependsOn<DBGeneratedClientClasses>();

			ReferenceAssembly( "System.Drawing" );

			UseThirdParty<ATICubemapGen>();
		}
	}
}