using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MorphemeAnimationPlugin : BaseCppLibrary
	{
		public MorphemeAnimationPlugin( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();

			DependsOn<MorphemeAnimation>();
			DependsOn<LibDBLoader>();
            if (configuration.target == Configuration.Target.RELEASE || configuration.target == Configuration.Target.FINALRELEASE)
                Define("MR_DISABLE_OUTPUT_DEBUGGING");
			UseThirdParty<Morpheme>();
		}
	}
}