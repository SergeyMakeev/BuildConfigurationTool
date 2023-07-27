using System;
using System.Collections.Generic;
using BCT.Source.Model;

namespace BCT.Source.Generators
{
	public interface IGenerator
	{
		bool BeforeBuild( Workspace workSpace );

		bool BuildProject( Workspace workSpace, List<ProjectFile> projectConfigurations );

		bool BuildSolution( Workspace workSpace, SolutionFile solution, Dictionary<Type, List<ProjectFile>> projectConfigurations );

		bool AfterBuild( Workspace workSpace, Dictionary<Type, List<ProjectFile>> projectConfigurations );
	}
}