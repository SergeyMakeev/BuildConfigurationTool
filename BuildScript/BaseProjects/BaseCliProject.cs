using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCliProject : BaseSkyforgeProject
	{
		protected BaseCliProject( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			language = Language.CPP_CLI;

			if ( !configuration.enableMutableLibDB )
			{
				excludeFromSolution = true;
			}

			Define("ENABLE_MUTABLE_LIBDB");
			Define("DEVELOPERS_CLIENT");

			Define( "WIN32" );
			Define( "WIN32_LEAN_AND_MEAN" );
			Define( "CAN_USE_WINDOWS_ONLY" );

			if ( configuration.target == Configuration.Target.DEBUG )
			{
				Define( "_DEBUG" );
			}

			characterSet = CharacterSet.UNICODE;
			wholeProgramOptimization = WholeProgramOptimization.NONE;

			outputDirectory = Utilites.GetOutputDir( configuration, platform );
			intermediateDirectory = Utilites.GetTempBuildDir( configuration, platform ) + GetType().Name + "\\";

			if ( configuration.target == Configuration.Target.DEBUG )
			{
				disableOptimization = true;
				incrementalLinking = true;
				generateDebugInformation = true;
			}
			else
			{
				disableOptimization = false;
				incrementalLinking = false;
				generateDebugInformation = true;
			}

			IncludePath( "%(ClientDir)" );
			IncludePath( "%(ClientDir)/BuildSources/BinaryLayout/" );
		}

		public void AddProjectFiles()
		{
			Files( string.Format( "{0}*.cpp", location ) );
			Files( string.Format( "{0}*.h", location ) );
			Files( string.Format( "{0}*.hpp", location ) );
			Files( string.Format( "{0}*.cxx", location ) );
			Files( string.Format( "{0}*.hxx", location ) );
		}

		public void AddProjectResources()
		{
			Resources( string.Format( "{0}*.rc", location ) );
		}
	}
}