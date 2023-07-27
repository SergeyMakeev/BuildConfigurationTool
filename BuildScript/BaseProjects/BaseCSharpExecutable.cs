using System.Collections.Generic;
using BCT.Source.Model;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCSharpExecutable : BaseCSharpLibrary
	{
		protected BaseCSharpExecutable( Workspace workSpace, PlatformType platform, Configuration configuration,
																		ApplicationKind applicationKind = ApplicationKind.WINDOWED_APPLICATION,
																		IEnumerable<CSharpLibrary> assemblies = null )
			: base( workSpace, platform, configuration, assemblies )
		{
			this.applicationKind = applicationKind;

			largeAddressAware = true;

			postBuildEvent = @"IF ""$(PlatformName)"" == ""x86"" (
IF NOT DEFINED IS_MSBUILD call ""$(VS100COMNTOOLS)..\..\vc\vcvarsall.bat"" x86
""$(VS100COMNTOOLS)..\..\vc\bin\EditBin.exe"" ""$(TargetPath)""  /LARGEADDRESSAWARE )";
		}
	}
}