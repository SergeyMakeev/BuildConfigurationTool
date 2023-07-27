using BCT.BuildScript.Projects;
using BCT.Source.Model;

namespace BCT.BuildScript.Solutions
{
	public class GameUnpacked : SolutionFile
	{
		public GameUnpacked()
		{
			AddConfiguration(ConfigurationFactory.DEBUG );
			AddConfiguration(ConfigurationFactory.RELEASE);

			AddPlatform( PlatformType.Win32 );
			AddPlatform( PlatformType.Win64 );

			//рутовые проекты солюшена - все зависимости будут добавлены автоматически			
			Project<Skyforge>();
			Project<DBCore>();
			Project<DebugInfoCli>();
			Project<ShaderExport>();
			Project<EnlightenGIRuntime>();

			Project<RenderD3D9>();
			Project<ResourceDBTools>();
			Project<Sandbox>();
			Project<Root>();
			Project<NSTL>();
		}
	}
}