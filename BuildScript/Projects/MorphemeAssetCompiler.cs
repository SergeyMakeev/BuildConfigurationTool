using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MorphemeAssetCompiler : BaseCppExecutable
	{
		public MorphemeAssetCompiler( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = ApplicationKind.CONSOLE_APPLICATION;
			layer = Layer.TOOLS;

			usePrecompiledHeaders = false;

			AddProjectFiles();

			DependsOn<MorphemeAnimationPlugin>( true );
			DependsOn<MorphemeAcXDB>( true );

			UseThirdParty<Morpheme>();
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/tools/assetCompiler/Core/include/core" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/SDK/export/include" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/SDK/assetProcessor/include" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/tinyxml" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/qhull/src" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMExpression/include");

			if ( configuration.UseDebugVendors() )
			{
				Library( "XMD_debug" );
				Library( "NMTL_debug" );
				Library( "NMTinyXML_debug" );
				Library( "qhull_tool_debug" );
				Library( "NMNumerics_tool_debug" );
				Library( "morphemeExport_debug" );
				if ( platform == PlatformType.Win64 )
				{
					Library( "morphemeAssetProcessor_target_LE64_debug" );
					Library( "acCore_target_LE64_debug" );
				}
				else
				{
					Library( "morphemeAssetProcessor_target_LE32_debug" );
					Library( "acCore_target_LE32_debug" );
				}
			}
			else
			{
				Library( "XMD" );
				Library( "NMTL" );
				Library( "NMTinyXML" );
				Library( "qhull_tool" );
				Library( "NMNumerics_tool" );
				Library( "morphemeExport" );
				if ( platform == PlatformType.Win64 )
				{
					Library( "morphemeAssetProcessor_target_LE64" );
					Library( "acCore_target_LE64" );
				}
				else
				{
					Library( "morphemeAssetProcessor_target_LE32" );
					Library( "acCore_target_LE32" );
				}
			}
			Library( "shlwapi" );

			Define( "NM_TARGET_BIGENDIAN=0" );
			Undef( "WIN32_LEAN_AND_MEAN" );
			//Define("_CONSOLE");
			if ( platform == PlatformType.Win64 )
				Define( "_WIN64" );
			if ( configuration.target == Configuration.Target.DEBUG )
				Define( "_ITERATOR_DEBUG_LEVEL=1" );
            if (configuration.target == Configuration.Target.RELEASE || configuration.target == Configuration.Target.FINALRELEASE)
                Define("MR_DISABLE_OUTPUT_DEBUGGING");
			enableExceptionHandling = true;
		}
	}
}