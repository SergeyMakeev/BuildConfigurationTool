using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ResourceDBUtils : BaseCSharpLibrary
	{
		public ResourceDBUtils( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ResourceDBUtils";

			AddProjectFiles();

			DependsOn<ClientToolsCSharp>();
			DependsOn<ResourceDB>();
			DependsOn<DBCore>();
			DependsOn<DBGeneratedClientClasses>();
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Xml.Linq" );

			ReferenceAssembly( "EPPlus", "%(VendorsDir)WheresMyMemory/Vendors/EPPlus/EPPlus.dll" );
		}
	}
}