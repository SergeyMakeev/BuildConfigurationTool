using BCT.Source;
using BCT.Source.Model;

using BCT.BuildScript.BaseProjects;

namespace BCT.BuildScript.Projects
{
	public class Skyforge : BaseCppExecutable
	{
		public Skyforge( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.APPLICATION;
			
			AddPlatformSpecificProjectFiles(platform);
			AddPlatformSpecificProjectResources(platform);

			DependsOn<Main>();
			DependsOn<Application>();

            if (platform == PlatformType.Orbis)
            {
                Library("-lSceRandom_stub_weak");
            }

			if (configuration.target != Configuration.Target.FINALRELEASE)
			{
				DependsOn<GameStateDebug>();
			}

			if (configuration.target == Configuration.Target.FINALRELEASE && configuration.enableProfiling)
			{
				targetName = "Skyforge_Profile";
			}

			fileMapping = Utilites.GetFileMappingForProject(this, workSpace);
		}
	}
}