using BCT.BuildScript.Projects;
using BCT.Source.Model;

namespace BCT.BuildScript.Solutions
{
	public class GamePacked : SolutionFile
	{
		public GamePacked()
		{
			AddConfiguration(ConfigurationFactory.DEBUG_PACKED );
			AddConfiguration(ConfigurationFactory.RELEASE_PACKED);
			AddConfiguration(ConfigurationFactory.FINALRELEASE);
			AddConfiguration(ConfigurationFactory.FINALRELEASE_PROFILE);

			AddPlatform( PlatformType.Win32 );
			AddPlatform( PlatformType.Win64 );
			AddPlatform( PlatformType.Orbis );

			//TODO: временно, чтобы не ломать солюшен тем, у кого нет Durango XDK
			if ( !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable( "DurangoXDK" )) )
			{
				AddPlatform( PlatformType.Durango );
			}

			//рутовые проекты солюшена - все зависимости будут добавлены автоматически			
			Project<Skyforge>();

			Project<Root>();
			Project<NSTL>();

			// Для того, чтобы добавить форсировать добавление проектов в солюшин, хотя эти проекты в солюшене выключены из сборки во всех конфигурациях
			Project<ClrGhost>();
			Project<EnlightenGIRuntime>();
		}
	}
}