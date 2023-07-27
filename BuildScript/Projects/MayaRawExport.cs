﻿using BCT.BuildScript.BaseProjects;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class MayaRawExport : BaseCliLibrary
	{
		public MayaRawExport( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			layer = Layer.TOOLS;

			AddProjectFiles();

			DependsOn<Tools>();
			DependsOn<Export>();
			DependsOn<DBGeneratedClientClasses>();
			DependsOn<Animation>();
		}
	}
}