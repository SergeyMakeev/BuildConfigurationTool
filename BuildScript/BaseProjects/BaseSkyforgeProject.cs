using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseSkyforgeProject : ProjectFile
	{
		protected BaseSkyforgeProject( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if ( configuration.target == Configuration.Target.DEBUG )
				treatWarningsAsErrors = false;

			//всегда выключаем warningsAsErrors если это задано в командной строке
			if ( workSpace.IsCommandLineOptionExist( "disableTreatWarningsAsErrors" ) )
				treatWarningsAsErrors = false;
			
			if ( workSpace.IsCommandLineOptionExist( "enableMyComDefine" ) )
				Define( "XBOX_MYCOM" );
		}
	}
}