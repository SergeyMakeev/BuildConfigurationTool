using System;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Profiler : ThirdParty
	{
		public Profiler(ProjectFile project, PlatformType platform, Configuration configuration)
			: base(project, platform, configuration)
		{
				project.IncludePath("%(VendorsDir)Brofiler/Publish/Include");

				string configName = Utilites.GetVendorsConfigurationName(configuration);

				switch (platform)
				{
					case PlatformType.Orbis:
						project.LibrariesPath("%(VendorsDir)Brofiler/Publish/Lib/vs2012/Orbis/" + configName);
						project.Library("libBrofilerCore");
						break;
					case PlatformType.Win32:
						project.LibrariesPath("%(VendorsDir)Brofiler/Publish/Lib/vs2010/x86/" + configName);
						project.Library("BrofilerCore");
						break;
					case PlatformType.Win64:
						project.LibrariesPath("%(VendorsDir)Brofiler/Publish/Lib/vs2010/x64/" + configName);
						project.Library("BrofilerCore");
						break;
					case PlatformType.Durango:
						project.LibrariesPath("%(VendorsDir)Brofiler/Publish/Lib/vs2015/durango/" + configName);
						project.Library("BrofilerCore");
						project.Library("ws2_32.lib");
						break;
					default:
						throw new NotSupportedException();
				}
			

		}
	}
}