using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class Snappy : ThirdParty
	{
		public Snappy( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)Snappy/src" );

			string libConfigurationDir;
			switch ( configuration.target )
			{
				case Configuration.Target.DEBUG:
				{
					libConfigurationDir = "Debug";
					break;
				}
				case Configuration.Target.RELEASE:
				{
					libConfigurationDir = "Release";
					break;
				}
				case Configuration.Target.FINALRELEASE:
				{
					libConfigurationDir = "Final";
					break;
				}
				default:
					throw new NotSupportedException();
			}

			string platformPathPart;
			switch ( platform )
			{
				case PlatformType.Win32:
					platformPathPart = "";
					break;
				case PlatformType.Win64:
					platformPathPart = "/x64";
					break;
				case PlatformType.Durango:
					platformPathPart = "/Durango";
					break;
				case PlatformType.Orbis:
					platformPathPart = "/Orbis";
					break;
				default:
					throw new NotSupportedException();
			}

			project.LibrariesPath(string.Format("%(VendorsDir)Snappy/Build/{0}/lib{1}/{2}", VSVersion.CurrentVersion, platformPathPart, libConfigurationDir));
			project.Library( "Snappy" );
		}
	}
}