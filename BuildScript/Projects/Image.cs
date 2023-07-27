using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Image : BaseCppLibrary
	{
		public Image( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.PLATFORM_IMPLEMENTATION;
            UseThirdParty<LibJpeg>();
            UseThirdParty<LibPng>();
            UseThirdParty<ZLib>();
			AddProjectFiles();
			DependsOn<Tools>();
		}
	}
}