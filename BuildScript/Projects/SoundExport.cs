using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class SoundExport : BaseCliLibrary
	{
		public SoundExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<Export>();

			if ( ( platform == PlatformType.Win32 ) || ( platform == PlatformType.Win64 ) )
			{
				UseThirdParty<FMod>();
			}
			else if ( platform == PlatformType.Orbis )
			{
				UseThirdParty<FMod>();
			}
			else
			{
				Define( "DEFINE_NULL_SOUNDSYSTEM_ONLY" );
			}

			ReferenceAssembly( "System.Xml" );
		}
	}
}