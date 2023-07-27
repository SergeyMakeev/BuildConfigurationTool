using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Collision : BaseCppLibrary
	{
		public Collision( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			UseThirdParty<Opcode>();

			AddProjectFiles();

			//там злой opcode.h который кучу всего тянет и конфликтует
			UnityBuildIgnoreFiles( "OpcodeMeshResource.cpp" );

			DependsOn<VisualObjectRuntime>();
			DependsOn<SpaceIndex>();
		}
	}
}