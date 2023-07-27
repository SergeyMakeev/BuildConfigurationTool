using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class HardwareChecker : BaseCppLibrary
	{
		public HardwareChecker( Workspace workSpace, Platform platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			UseThirdParty<CrashRpt>();
		}
	}
    //test
}