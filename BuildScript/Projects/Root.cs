using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class Root : BaseCppLibrary
	{
		public Root( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			applicationKind = ApplicationKind.UTILITY;

            filesRecuriveSearch = false;

			var rootFiles = new[]
											{
												"narrowing_cast.h",
												"types.h",
												"client_config.h",
												"RevisionId.h",
												"modules.h",
												"features.h",
												"common.h",
												"common_editor.h",
												"common_game.h",
												"Guid.h",
												"app_version.h",
												"common_nstl.h",
												"solution_config.h",
												"platform.h",
												"nstl_config.h",
												"offset_calculator.h",
											};

			foreach ( var q in rootFiles )
				Files( "%(ClientDir)" + q );
		}
	}
}