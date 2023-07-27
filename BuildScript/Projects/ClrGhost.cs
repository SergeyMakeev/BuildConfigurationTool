using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ClrGhost : BaseCppLibrary
	{
		public ClrGhost( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if (platform == PlatformType.Durango || platform == PlatformType.Orbis)
			{
				excludeFromSolution = true;
			}

			AddProjectFiles();

			DependsOn<Logger>();
			DependsOn<Tools>();

			Define( "CAN_USE_WINDOWS_ONLY" );
		}
	}
}