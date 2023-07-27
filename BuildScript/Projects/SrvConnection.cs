using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class SrvConnection : BaseCppLibrary
	{
		public SrvConnection( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			if ( platform.IsWindows() )
			{
				UseThirdParty<Starforce>();
				UseThirdParty<OpenSSL>();
			}

            AddProjectFiles();

            DependsOn<NetProtected>();
            DependsOn<Replication>();
			DependsOn<GameTools>();
			DependsOn<DataProvider>();
		}
	}
}