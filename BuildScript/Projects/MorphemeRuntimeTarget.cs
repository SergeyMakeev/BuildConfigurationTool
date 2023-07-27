using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MorphemeRuntimeTarget : BaseCppExecutable
	{
		public MorphemeRuntimeTarget( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = ApplicationKind.CONSOLE_APPLICATION;
			layer = Layer.TOOLS;

			usePrecompiledHeaders = false;

			AddProjectFiles();
			DependsOn<MorphemeAnimationPlugin>( true );

			UseThirdParty<Morpheme>();
			IncludePath( location );
			IncludePath( location + "NoPhysics" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/tools/assetCompiler/Core/include/core" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/utils/comms2/include" );
			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMExpression/include");

			if ( configuration.UseDebugVendors() )
				Library( "morphemeComms2_tool_debug" );
			else
				Library( "morphemeComms2_tool" );
			Library( "WS2_32" );

			Undef( "WIN32_LEAN_AND_MEAN" );
			if ( platform == PlatformType.Win64 )
                Define("_WIN64");
            if (configuration.target == Configuration.Target.DEBUG)
                Define("_ITERATOR_DEBUG_LEVEL=1");
            if (configuration.target == Configuration.Target.RELEASE || configuration.target == Configuration.Target.FINALRELEASE)
                Define("MR_DISABLE_OUTPUT_DEBUGGING");

			enableExceptionHandling = true;
		}
	}
}