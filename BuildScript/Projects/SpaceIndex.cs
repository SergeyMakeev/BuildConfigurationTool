using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class SpaceIndex : BaseCppLibrary
	{
		public SpaceIndex( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.ENGINE;

			AddProjectFiles();
			Files( "%(ClientDir)SpaceIndex/*.inl" );

			DependsOn<Tools>();
		}
	}
}