using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class LatencyMeter : BaseCppLibrary
	{
		public LatencyMeter( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<Global>();
			if ( ( platform != PlatformType.Win32 ) && ( platform != PlatformType.Win64 ) )
			{
				excludeFromSolution = true;
			}
		}
	}
}