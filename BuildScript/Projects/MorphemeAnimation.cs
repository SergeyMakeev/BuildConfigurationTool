using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MorphemeAnimation : BaseCppLibrary
	{
		public MorphemeAnimation( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			UseThirdParty<Morpheme>();
			UseThirdParty<TaskScheduler>();

			IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMExpression/include");

			AddProjectFiles();
		        if (configuration.target == Configuration.Target.RELEASE || configuration.target == Configuration.Target.FINALRELEASE)
               			 Define("MR_DISABLE_OUTPUT_DEBUGGING");

			DependsOn<Animation>();
		}
	}
}