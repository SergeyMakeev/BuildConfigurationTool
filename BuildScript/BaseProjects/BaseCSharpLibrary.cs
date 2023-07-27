using System.Collections.Generic;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCSharpLibrary : BaseSkyforgeProject
	{
		protected BaseCSharpLibrary( Workspace workSpace, PlatformType platform, Configuration configuration,
																 IEnumerable<CSharpLibrary> assemblies = null )
			: base( workSpace, platform, configuration )
		{
			language = Language.C_SHARP;

			if (!configuration.enableMutableLibDB)
			{
				excludeFromSolution = true;
			}

			applicationKind = ApplicationKind.SHARED_LIBRARY;

			Define( "TRACE" );

			if ( platform == PlatformType.Win64 )
			{
				Define( "WIN64" );
			}

			outputDirectory = Utilites.GetOutputDir( configuration, platform );
			intermediateDirectory = Utilites.GetTempBuildDir( configuration, platform ) + GetType().Name + "\\";

			if ( configuration.target == Configuration.Target.DEBUG )
			{
				Define( "DEBUG" );
				disableOptimization = true;
				generateDebugInformation = true;
			}
			else
			{
				Define( "NDEBUG" );
				disableOptimization = false;
				generateDebugInformation = true;
			}

			foreach ( var q in assemblies ?? GetAssemblies() )
				ReferenceAssembly( q );
		}

		private static IEnumerable<CSharpLibrary> GetAssemblies()
		{
			yield return new CSharpLibrary( "System" );
			yield return new CSharpLibrary( "System.Core" );
			yield return new CSharpLibrary( "PresentationCore" );
			yield return new CSharpLibrary( "PresentationFramework" );
			yield return new CSharpLibrary( "WindowsBase" );
		}

		public void AddProjectFiles()
		{
			Files( string.Format( "{0}*.cs", location ) );
			Files( string.Format( "{0}*.xaml", location ) );
			Exclude( string.Format( "{0}obj/*", location ) );

			Files( string.Format( "{0}*.resx", location ) );
			Files( string.Format( "{0}*.licx", location ) );
			Resources( string.Format( "{0}*.png", location ) );
			Resources( string.Format( "{0}*.ico", location ) );
			Resources( string.Format( "{0}*.ttf", location ) );
		}
	}
}