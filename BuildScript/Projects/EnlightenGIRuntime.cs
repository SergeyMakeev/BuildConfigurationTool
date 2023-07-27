using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EnlightenGIRuntime : BaseCppLibrary
	{
		public EnlightenGIRuntime( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if (platform == PlatformType.Durango || platform == PlatformType.Orbis || !configuration.enableMutableLibDB)
			{
				excludeFromSolution = true;
			}

			layer = Layer.ENGINE;

			enableExceptionHandling = true;

			UseThirdParty<Enlighten>();

			AddProjectFiles();
			
			DependsOn<NDbTypes>();
			Define("_ALLOW_ITERATOR_DEBUG_LEVEL_MISMATCH");
		}
	}
}