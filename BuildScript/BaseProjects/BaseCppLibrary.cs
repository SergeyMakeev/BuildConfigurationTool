using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCppLibrary : BaseCppProject
	{
		protected BaseCppLibrary( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = (configuration.target == Configuration.Target.FINALRELEASE) ? ApplicationKind.OBJECT_LIST : ApplicationKind.SHARED_LIBRARY;

			if (platform == PlatformType.Orbis)
			{
				applicationKind = ApplicationKind.OBJECT_LIST;
			}

			skipDefGeneration = (applicationKind == ApplicationKind.OBJECT_LIST);
		}
	}
}