using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Scaleform : BaseCppLibrary
	{
		public Scaleform( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			DependsOn<RenderD3D12>();
			DependsOn<RenderGnm>();
			DependsOn<RenderD3D9>();

			if (platform == PlatformType.Orbis)
			{
				Library("libSceRtc_stub_weak");
			}
			else
			{
				UseThirdParty<D3D9>();
			}

			UseThirdParty<ScaleformLib>();

			AddProjectFiles();

			DependsOn<Scene3D>();
		}
	}
}