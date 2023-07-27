using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class RcsCpp : BaseCppLibrary
	{
		public RcsCpp( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			AddProjectFiles();

			DependsOn<ExternalProcesses>();
			DependsOn<IO>();
			DependsOn<Tools>();
		}
	}
}