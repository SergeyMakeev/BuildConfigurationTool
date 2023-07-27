using System;
using BCT.Source.Model;

namespace BCT.BuildScript.Vendors
{
	public class CrnDecomp : ThirdParty
	{
		public CrnDecomp(ProjectFile project, PlatformType platform, Configuration configuration)
			: base(project, platform, configuration)
		{
				project.IncludePath("%(VendorsDir)CrnLib/inc/");
		}
	}
}