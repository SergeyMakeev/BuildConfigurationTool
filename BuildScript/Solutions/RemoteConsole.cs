using BCT.BuildScript.Projects;
using BCT.Source.Model;

namespace BCT.BuildScript.Solutions
{
	public class RemoteConsole : SolutionFile
	{
		public RemoteConsole()
		{
			AddConfiguration(ConfigurationFactory.DEBUG);
			AddConfiguration(ConfigurationFactory.RELEASE);
			AddConfiguration(ConfigurationFactory.DEBUG_PACKED);
			AddConfiguration(ConfigurationFactory.RELEASE_PACKED);

			AddPlatform(PlatformType.Win32);
			AddPlatform(PlatformType.Win64);

			Project<ExternalTools>();
		}
	}
}