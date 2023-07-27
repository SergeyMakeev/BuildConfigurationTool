using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MorphemeAcXDB : BaseCppLibrary
	{
		public MorphemeAcXDB( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			usePrecompiledHeaders = false;
			skipDefGeneration = true; // Это нужно, чтобы assetCompiler не тащил к себе operator new/delete из MemoryLib

			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/SDK/export/include" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/SDK/assetProcessor/include" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/tinyxml" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMExpression/include");

			AddProjectFiles();

			DependsOn<MorphemeAnimationPlugin>();
			UseThirdParty<Morpheme>();

			if ( configuration.UseDebugVendors())
			{
				Library( "XMD_debug" );
				Library( "NMTL_debug" );
			}
			else
			{
				Library( "XMD" );
				Library( "NMTL" );
			}

			Define( "ACPLUGIN_XDB_EXPORTS" );
			Define( "NM_TARGET_BIGENDIAN=0" );
			Define( "NM_ENABLE_EXCEPTIONS=0" );
			Undef( "WIN32_LEAN_AND_MEAN" );
			if ( platform == PlatformType.Win64 )
				Define( "_WIN64" );
            if (configuration.target == Configuration.Target.RELEASE || configuration.target == Configuration.Target.FINALRELEASE)
                Define("MR_DISABLE_OUTPUT_DEBUGGING");
		}
	}
}