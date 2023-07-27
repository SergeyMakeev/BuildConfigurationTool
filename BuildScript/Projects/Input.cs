using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;
using BCT.Source;

namespace BCT.BuildScript.Projects
{
	public class Input : BaseCppLibrary
	{
		public Input( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_ABSTRACTION;

			if ( platform == PlatformType.Durango )
			{
				enableExceptionHandling = true;
			}
			
			if ( platform == PlatformType.Win32 || platform == PlatformType.Win64 )
			{
				Library( string.Format( "%(VendorsDir)Xinput/{0}/Xinput9_1_0", Utilites.GetPlatformNameSharp( platform ) ) );
			}
			
			if ( platform == PlatformType.Orbis )
			{
				Library("libScePad_stub_weak");
			}

			DependsOn<Tools>();
			DependsOn<Global>();
			DependsOn<Application>();
			DependsOn<DataProvider>();//Где то пролез инклуд DataProvider, и не линкуется FilePath, надо выпилить и убрать зависимость

			if ( platform == PlatformType.Win32 || platform == PlatformType.Win64 )
			{
				DependsOn<NDbTypes>();
			}
			
			AddPlatformSpecificProjectFiles( platform );
			AddPlatformSpecificProjectResources( platform );
		}
	}
}