using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class JavaDescriptors : BaseCppLibrary
	{
		public JavaDescriptors( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = ApplicationKind.UTILITY;
		}
	}
}