using BCT.BuildScript.BaseProjects;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ExternalTools : BaseCSharpExecutable
	{
		public ExternalTools( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "ExternalTools";
			ApplicationIcon = "Resources\\1389031861_7824.ico";

			AddProjectFiles();
			applicationDefinition = "App.xaml";

			DependsOn<EditorCore>();

			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Windows.Forms" );
			ReferenceAssembly( "Microsoft.CSharp" );
			ReferenceAssembly( "Newtonsoft.Json", @"%(VendorsDir)Json.Net\Bin\Net40\Newtonsoft.Json.dll" );
			ReferenceAssembly( "DevkitsInterop", @"%(VendorsDir)DevkitsInterop\bin\Release\DevkitsInterop.dll" );

			// Все сторонние либы должны быть подключены как референсы, что бы можно было поставлять один exe файл
			string vendorDir = workSpace.ResolveMacroVariables( "%(VendorsDir)" );
			string clientDir = workSpace.ResolveMacroVariables( "%(ClientDir)" );
			string vendorRelative = "..\\" + Utilites.RelativePath( clientDir, vendorDir );

			AddFileLink( vendorRelative + @"Json.Net\Bin\Net40\Newtonsoft.Json.dll" );
			AddFileLink( vendorRelative + @"DevkitsInterop\bin\Release\DevkitsInterop.dll" );
			AddFileLink( vendorRelative + @"DevkitsInterop\bin\Release\Interop.ORTMAPILib.dll" );
			AddFileLink( vendorRelative + @"DevkitsInterop\bin\Release\Microsoft.Xbox.Xtf.ConsoleManager.dll" );
			AddFileLink( vendorRelative + @"DevkitsInterop\bin\Release\Microsoft.Xbox.XTF.Interop.dll" );
			AddFileLink( vendorRelative + @"DevkitsInterop\bin\Release\XtfConsoleManager.dll" );
		}
	}
}