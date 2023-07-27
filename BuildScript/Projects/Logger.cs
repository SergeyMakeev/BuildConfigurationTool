using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;
using BCT.BuildScript.Vendors;

namespace BCT.BuildScript.Projects
{
	public class Logger : BaseCppLibrary
	{
		public Logger( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.FOUNDATION;

			//не знаю как более умно из FR выключить
			if (configuration.GetTargetConfigurationName().Contains("FinalRelease"))
			{
				excludeFromSolution = true;
			}

			Define("CLIENT_SRC_ROOT=%(ClientDir)");

			AddProjectFiles();

			AddProjectResources();

			DependsOn<Platform>();

			//для парсинга конфигов
			UseThirdParty<PugiXML>();

		}
	}
}