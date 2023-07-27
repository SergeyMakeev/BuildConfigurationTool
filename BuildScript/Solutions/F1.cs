using System.Reflection;

using BCT.Source.Model;
using BCT.BuildScript.BaseProjects;


namespace BCT.BuildScript.Solutions
{
	public class _F1 : SolutionFile
	{
		public _F1()
		{
			AddConfiguration(ConfigurationFactory.DEBUG );
			AddConfiguration(ConfigurationFactory.RELEASE);

			AddPlatform( PlatformType.Win32 );
			AddPlatform( PlatformType.Win64 );

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ProjectFile)) &&
									!type.Name.Contains("RenderGnm") )		 // временная подпорка, пока не будет починена buildTool
                    AddProject(type);
            }
		}
	}
}