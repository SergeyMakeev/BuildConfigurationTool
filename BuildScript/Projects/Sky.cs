using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Sky : BaseCppLibrary
	{
		public Sky( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
            if (configuration.enableMutableLibDB)
            {
                Define("USE_SIMUL_EXPORT");
                UseThirdParty<Simul>();
            }

			AddProjectFiles();

			DependsOn<Scene3D>();
		}
	}
}