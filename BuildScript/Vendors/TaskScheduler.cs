using System;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class TaskScheduler : ThirdParty
	{
		public TaskScheduler( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			string vsName;
			string platformName;

			switch (platform)
			{
				case PlatformType.Win64:
				case PlatformType.Win32:
			    vsName = VSVersion.CurrentVersion;
					platformName = Utilites.GetPlatformNameSharp(platform);
					break;
				case PlatformType.Orbis:
					vsName = "vs2012";
					platformName = "ps4";
					break;
				case PlatformType.Durango:
					vsName = "vs2015";
					platformName = "x1";
					break;
				default:
					throw new NotSupportedException();
			}

			if (configuration.target != Configuration.Target.FINALRELEASE || configuration.enableProfiling)
			{
				project.Define("MT_INSTRUMENTED_BUILD=1");
			}

			project.IncludePath("%(VendorsDir)TaskScheduler/Scheduler/Include");
			
			Configuration.Target target = configuration.target;

			// костыль, чтобы на fr_profile у нас использовался инструментированый вендор
			if (target == Configuration.Target.FINALRELEASE && configuration.enableProfiling)
			{
				target = Configuration.Target.RELEASE;
			}

			project.Library(string.Format("%(VendorsDir)TaskScheduler/VisualStudio/lib/scheduler_{0}_{1}_{2}", platformName, target.ToString(), vsName));


			if (platform == PlatformType.Orbis)
			{
				project.Library("-lSceSysmodule_stub_weak");
				project.Library("-lScePosix_stub_weak");
				project.Library("-lSceFiber_stub_weak");
			}

		}
	}
}