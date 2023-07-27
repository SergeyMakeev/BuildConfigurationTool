using System.Collections.Generic;
using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class EditorIndex : BaseCSharpExecutable
	{
		public EditorIndex( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration, assemblies: GetAssemblies() )
		{
			rootNamespace = "EditorIndex";

			applicationDefinition = "App.xaml";
			ApplicationIcon = "Resources\\icon.ico";

			AddProjectFiles();
			AddFileLink( @"..\ClientToolsCSharp\EditorIndex\Contracts.cs" );

			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Runtime.Serialization" );
			ReferenceAssembly( "nunit.framework", @"%(VendorsDir)NUnit\bin\nunit.framework.dll" );
		}

		private static IEnumerable<CSharpLibrary> GetAssemblies()
		{
			yield return new CSharpLibrary( "System" );
			yield return new CSharpLibrary( "System.Core" );
			yield return new CSharpLibrary( "PresentationCore" );
			yield return new CSharpLibrary( "PresentationFramework" );
			yield return new CSharpLibrary( "WindowsBase" );
		}
	}
}