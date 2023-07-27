using BCT.Source.Model;
using BCT.BuildScript.Projects;

namespace BCT.BuildScript.Solutions
{
	public class Bots : SolutionFile
	{
		public Bots()
		{
			AddConfiguration(ConfigurationFactory.DEBUG);
			AddConfiguration(ConfigurationFactory.RELEASE);

			AddPlatform(PlatformType.Win32);
			AddPlatform(PlatformType.Win64);

			//рутовые проекты солюшена - все зависимости будут добавлены автоматически			
			Project<Bot>();
			Project<DBCore>();
			Project<NSTL>();
			Project<ResourceDBTools>();

		}
	}
}