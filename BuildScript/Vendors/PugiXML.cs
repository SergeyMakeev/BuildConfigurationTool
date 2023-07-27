using System;
using BCT.Source;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class PugiXML : ThirdParty
	{
		public PugiXML( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			string vsName;
			string platformName;

			//TODO: vsName заменить на имя платформы 
			switch ( platform )
			{
				case PlatformType.Win64:
				case PlatformType.Win32:
					vsName = VSVersion.CurrentVersion;
					platformName = Utilites.GetPlatformNameSharp( platform );
					break;
				case PlatformType.Orbis:
					vsName = "vs2012";
					platformName = "PS4_orbis";
					break;
				case PlatformType.Durango:
					vsName = "vs2015";
					platformName = "X1_durango";
					break;
				default:
					throw new NotSupportedException();
			}

			project.IncludePath( "%(VendorsDir)PugiXML/Src" );
			project.Library( string.Format( "%(VendorsDir)PugiXML/Build/{0}/lib/PugiXML_{1}_{2}", vsName, platformName, configuration.target.ToString() ) );
		}
	}
}