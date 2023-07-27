using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class DBCore : BaseCSharpLibrary
	{
		public DBCore( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ResourceDB";

			AddProjectFiles();

			ReferenceAssembly( "EPPlus", "%(VendorsDir)WheresMyMemory/Vendors/EPPlus/EPPlus.dll" );
			ReferenceAssembly( "FileHelpers", "%(VendorsDir)FileHelpers/DotNet 2.0/FileHelpers.dll" );

			DependsOn<ClientTools>();
			DependsOn<ClientToolsCSharp>();
			DependsOn<LoggerCli>();
			DependsOn<ResourceDB>();
			DependsOn<ResourceDBNative>();
		}
	}
}