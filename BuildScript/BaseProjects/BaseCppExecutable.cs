using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCppExecutable : BaseCppProject
	{
		protected BaseCppExecutable( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
            if (platform == PlatformType.Durango)
            {
                enableExceptionHandling = true;
            }


			applicationKind = ApplicationKind.WINDOWED_APPLICATION;

			if ( platform == PlatformType.Win32 )
				largeAddressAware = true;

			skipDefGeneration = true;
			useManifest = true;
		}
	}
}