using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class NaviMapExport : BaseCliLibrary
	{
		public NaviMapExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<ExportImageTools>();
			DependsOn<Export>();
			DependsOn<RenderCommon>();

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Xml" );

			ReferenceAssembly( "ICSharpCode.SharpZipLib", @"%(VendorsDir)SharpZipLib\ICSharpCode.SharpZipLib.dll" );
		}
	}
}