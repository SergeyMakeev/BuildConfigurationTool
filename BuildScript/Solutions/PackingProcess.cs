using BCT.BuildScript.Projects;
using BCT.Source.Model;

namespace BCT.BuildScript.Solutions
{
	public class PackingProcess : SolutionFile
	{
		public PackingProcess()
		{
			AddConfiguration( ConfigurationFactory.DEBUG );
			AddConfiguration( ConfigurationFactory.RELEASE );

			AddPlatform( PlatformType.Win64 );

			Project<ResourceDBTools>();
			Project<EditorCLI>();
		}
	}
}