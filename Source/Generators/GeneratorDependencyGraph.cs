using System;
using System.Collections.Generic;
using System.IO;
using BCT.Source.Model;

namespace BCT.Source.Generators
{
	class ProjectFileLayerComparer : IComparer<ProjectFile>
	{
		public int Compare( ProjectFile a, ProjectFile b )
		{
			return a.layer - b.layer;
		}
	}

	public class GeneratorDepencyGraph : IGenerator
	{
		public bool BuildProject( Workspace workSpace, List<ProjectFile> projectConfigurations )
		{
			return true;
		}

		public bool BeforeBuild( Workspace workSpace )
		{
			return true;
		}

		public bool AfterBuild( Workspace workSpace, Dictionary<Type, List<ProjectFile>> projectConfigurations )
		{
			return true;
		}

		public bool BuildSolution( Workspace workSpace, SolutionFile solution, Dictionary<Type, List<ProjectFile>> projectConfigurations )
		{
			var solutionName = solution.GetType().Name;
			var fileName = Utilites.GetTargetDirectory() + solutionName + ".dot";

			using ( var fileStream = new FileStream( fileName, FileMode.Create, FileAccess.Write ) )
			{
				using ( var output = new StreamWriter( fileStream ) )
				{
					output.WriteLine( "digraph {0} {{", solution.GetType().Name );

					output.WriteLine( "pack = true;" );
					output.WriteLine( "packmode = \"array_u\";" );
					output.WriteLine( "compound = \"true\";" );

					var sortedProjects = new List<ProjectFile>();

					var uniqueThirdParties = new Dictionary<Type, int>();

					foreach ( var projects in projectConfigurations )
					{
						var proj = projects.Value[0];

						foreach ( var thirdPartyRef in proj.ReferencesThirdParty )
						{
							int tmp;
							if ( !uniqueThirdParties.TryGetValue( thirdPartyRef, out tmp ) )
								uniqueThirdParties.Add( thirdPartyRef, 0 );
						}

						var uselessDepends = proj.GetUselessDepends();
						if ( uselessDepends.Count > 0 )
						{
							Console.WriteLine( "Useless depends from " + proj.GetType().Name );

							foreach ( var uselessDepend in uselessDepends )
								Console.WriteLine( "     -> " + uselessDepend.GetType().Name );
						}
						sortedProjects.Add( proj );
					}

					sortedProjects.Sort( new ProjectFileLayerComparer() );

					var needSubgraphs = true;

					var clusterIndex = 0;

					if ( needSubgraphs )
					{
						output.WriteLine( "subgraph cluster{0} {{", clusterIndex );
						output.WriteLine( "sortv = {0};", clusterIndex );
					}

					var curLayer = sortedProjects[0].layer;

					foreach ( var proj in sortedProjects )
					{
						if ( needSubgraphs && curLayer != proj.layer )
						{
							curLayer = proj.layer;

							clusterIndex++;

							output.WriteLine( "}" );
							output.WriteLine( "subgraph cluster{0} {{", clusterIndex );
							output.WriteLine( "sortv = {0};", clusterIndex );
						}

						var clr = GetColorFromLayer( proj.layer );
						output.WriteLine( "      {0} [shape=box, style=filled, fillcolor=\"{1}\"];", proj.GetType().Name, clr );
					}

					if ( needSubgraphs )
						output.WriteLine( "}" );

					foreach ( var tp in uniqueThirdParties )
						output.WriteLine( "      {0} [shape=ellipse, style=filled, fillcolor=\"#903090\"];", tp.Key.Name );

					foreach ( var projects in projectConfigurations )
					{
						var proj = projects.Value[0];

						var references = proj.References;

						foreach ( var reference in references )
						{
							if ( IfLinkIsNeed( proj, reference.project ) )
							{
								output.WriteLine( "{0} -> {1} [color=\"{2}\"];", proj.GetType().Name, reference.project.GetType().Name,
																	GetLinkColor( proj, reference.project ) );
							}
						}

						foreach ( var tp in proj.ReferencesThirdParty )
						{
							if ( IfLinkIsNeed( proj, tp ) )
								output.WriteLine( "{0} -> {1} [color=\"#903090\"];", proj.GetType().Name, tp.Name );
						}
					}
					output.WriteLine( "}" );
				}
			}

			Console.WriteLine( "dot {0}.dot -Tbmp -o {0}.bmp", solutionName );
			return true;
		}

		public string GetColorFromLayer( Layer layer )
		{
			switch ( layer )
			{
				case Layer.APPLICATION:
					return "#00BDE6";
				case Layer.FOUNDATION:
					return "#FF0000";
				case Layer.PLATFORM_IMPLEMENTATION:
					return "#00FF00";
				case Layer.PLATFORM_ABSTRACTION:
					return "#FFDE00";
				case Layer.PROJECT_SPECIFIC:
					return "#A0A0A0";
				case Layer.RESOURCE_TYPES:
					return "#FF00FF";
				case Layer.ENGINE:
					return "#FFFF00";
				case Layer.TOOLS:
					break;
				default:
					throw new ArgumentOutOfRangeException( "layer", layer, null );
			}

			return "#FFFFFF";
		}

		public bool IfLinkIsNeed( ProjectFile from, Type to )
		{
			return to.Name != "Profiler";
		}

		public bool IfLinkIsNeed( ProjectFile from, ProjectFile to )
		{
			if ( from.layer != Layer.FOUNDATION && to.layer == Layer.FOUNDATION )
				return false;

			if ( to.layer == Layer.RESOURCE_TYPES )
				return false;

			return true;
		}

		public string GetLinkColor( ProjectFile from, ProjectFile to )
		{
			if ( from.layer == to.layer )
				return "#808080";

			//на имплементацию кода для платформы может ссылаться только App
			if ( to.layer == Layer.PLATFORM_IMPLEMENTATION && from.layer != Layer.APPLICATION )
				return "#FF0000";

			if ( from.layer != Layer.PLATFORM_IMPLEMENTATION )
			{
				// от базовых проектов нельзя ссылаться на верхний уровень
				if ( from.layer < to.layer )
					return "#FF0000";
			}
			else
			{
				// platform implementation может ссылаться только на platform abstraction и ниже
				if ( to.layer > Layer.PLATFORM_ABSTRACTION )
					return "#FF0000";
			}

			return "#000000";
		}
	}
}