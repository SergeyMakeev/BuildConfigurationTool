using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EnlightenGIPrecompute : BaseCppLibrary
	{
		public EnlightenGIPrecompute( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			DependsOn<EnlightenGIRuntime>();
			AddProjectFiles();

			UseThirdParty<Enlighten>();

			Define("_ALLOW_ITERATOR_DEBUG_LEVEL_MISMATCH");
		}
	}
}