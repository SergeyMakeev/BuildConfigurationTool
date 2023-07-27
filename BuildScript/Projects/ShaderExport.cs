using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ShaderExport : BaseCliLibrary
	{
		public ShaderExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			workSpace.ExecutePrebuildCommand( @"%(CodeToolsDir)ShaderPostBuild\ShaderPostBuild\bin\Release\ShaderPostBuild.exe", "",
																				"%(ClientDir)ShaderExport" );

			AddProjectFiles();
			AddProjectResources();

			Files( "%(ClientDir)ShaderExport/HLSLList.txt" );

			DependsOn<ClientToolsCSharp>();
			DependsOn<ResourceDBNative>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<Global>();
			DependsOn<LoggerCli>();
			DependsOn<NDbTypes>();
			DependsOn<Tools>();
			DependsOn<ClientTools>();
			DependsOn<RenderCommon>();
			DependsOn<Export>();
			DependsOn<ShaderBuilder>( true );
			DependsOn<BinaryLayout>();
			DependsOn<RenderUtils>();
			DependsOn<RenderUtilsGnm>();
            DependsOn<RenderUtilsD3D12X>();
			DependsOn<RenderUtilsD3D9>();

            bool orbisSDKFound = System.Environment.GetEnvironmentVariable("SCE_ORBIS_SDK_DIR") != null;
            if (platform == PlatformType.Win64 && orbisSDKFound)
            {
                Define("ORBIS_SDK_FOUND");
            }

			UseThirdParty<D3D9>();
			UseThirdParty<DiaSdk>();

			ReferenceAssembly( "mscorlib" );
			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "CompilerFrontend", "%(VendorsDir)ShaderCompiler/Bin/CompilerFrontend.dll" );
			ReferenceAssembly( "CompilerIPC", "%(VendorsDir)ShaderCompiler/Bin/CompilerIPC.dll" );
			ReferenceAssembly( "CompilerIPC", "%(VendorsDir)ShaderCompiler/ThirdParty/NLog/net40/NLog.dll" );

			preBuildEvent = @"..\..\..\codetools\ShaderPostBuild\ShaderPostBuild\bin\Release\ShaderPostBuild.exe";
		}
	}
}