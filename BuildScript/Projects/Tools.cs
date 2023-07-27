using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Tools : BaseCppLibrary
	{
		public Tools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.FOUNDATION;

			UseThirdParty<Snappy>();
			UseThirdParty<TaskScheduler>();

			AddProjectFiles();

			DependsOn<MathLib>();
            UseThirdParty<ZLib>();

			if ( platform == PlatformType.Durango )
			{
				Library("ws2_32");
			}
		}
	}
}