using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MorphemeAnimationExport : BaseCliLibrary
	{
		public MorphemeAnimationExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();
			Exclude( "MorphemeLogger.cpp" );

			DependsOn<Export>();

			UseThirdParty<Morpheme>();
            if (configuration.target == Configuration.Target.RELEASE || configuration.target == Configuration.Target.FINALRELEASE)
                Define("MR_DISABLE_OUTPUT_DEBUGGING");
			ReferenceAssembly( "System.Xml" );
		}
	}
}