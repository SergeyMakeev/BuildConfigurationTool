using BCT.BuildScript.Projects;
using BCT.Source.Model;

namespace BCT.BuildScript.Solutions
{
	public class Smoke : SolutionFile
	{
		public Smoke()
		{
			AddConfiguration(ConfigurationFactory.DEBUG_PACKED);
			AddConfiguration(ConfigurationFactory.RELEASE_PACKED);
			AddConfiguration(ConfigurationFactory.FINALRELEASE);

			AddPlatform(PlatformType.Win32);
			AddPlatform(PlatformType.Win64);
			AddPlatform(PlatformType.Durango);
			AddPlatform(PlatformType.Orbis);

			Project<UnitTests>();
			Project<Sandbox>();

			Project<Root>();
			Project<NSTL>();
            Project<MorphemeAnimation>();

			// Для того, чтобы добавить форсировать добавление проектов в солюшин, хотя эти проекты в солюшене выключены из сборки во всех конфигурациях
			Project<ClrGhost>();
			Project<EnlightenGIRuntime>();
			Project<Scaleform>();

		}
	}
}