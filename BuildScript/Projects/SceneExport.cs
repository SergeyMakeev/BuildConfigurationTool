using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class SceneExport : BaseCliLibrary
	{
		public SceneExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();
			AddProjectResources();

			Files( "%(ClientDir)SceneExport/ModelExport.inl" );

			DependsOn<Animation>();
			DependsOn<BinaryLayout>();
			DependsOn<ClientEditorNative>();
			DependsOn<ClientTools>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<Collision>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<Export>();
			DependsOn<Global>();
			DependsOn<LoggerCli>();
			DependsOn<NDbTypes>();
			DependsOn<RenderCommon>();
			DependsOn<ResourceDBNative>();
			DependsOn<Tools>();
			DependsOn<Scene3D>();
			DependsOn<ShaderExport>();
			DependsOn<EditorLauncher>();

			UseThirdParty<D3D9>();
			UseThirdParty<FCollada>();

			UseThirdParty<FBX>();
			Define( "FBXSDK_SHARED" );

			ReferenceAssembly( "mscorlib" );
			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.Web.Extensions" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "CompilerFrontend", "%(VendorsDir)ShaderCompiler/Bin/CompilerFrontend.dll" );
			ReferenceAssembly( "CompilerIPC", "%(VendorsDir)ShaderCompiler/Bin/CompilerIPC.dll" );
			ReferenceAssembly( "CompilerIPC", "%(VendorsDir)ShaderCompiler/ThirdParty/NLog/net40/NLog.dll" );

		}
	}
}