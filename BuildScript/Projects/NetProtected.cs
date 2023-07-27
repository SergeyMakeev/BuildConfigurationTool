using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class NetProtected : BaseCppLibrary
	{
		public NetProtected( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;

			if ( platform.IsWindows() )
			{
				UseThirdParty<Starforce>();
				UseThirdParty<OpenSSL>();

                if (configuration.target != Source.Model.Configuration.Target.FINALRELEASE)
                    Define("DISABLE_STARFORCE_PROTECTED_DATA_BLOBS");
			}

			AddProjectFiles();

			DependsOn<Net>();
		}
	}
}