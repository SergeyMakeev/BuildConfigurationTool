using System.Collections.Generic;

namespace BCT.Source
{
	public class CommandLineOptions
	{
		readonly Dictionary<string, string> options = new Dictionary<string, string>();

		public CommandLineOptions( string[] args )
		{
			foreach ( var argument in args )
			{
				//not argument
				if ( argument.Length <= 2 )
					continue;

				//not argument
				if ( argument[0] != '-' || argument[1] != '-' )
					continue;

				var arg = argument.TrimStart( '-' );
				var keyAndValue = arg.Split( '=' );

				var key = keyAndValue[0].ToLower();

				var value = "";
				if ( keyAndValue.Length > 1 )
					value = keyAndValue[1].ToLower();

				string v;
				if ( options.TryGetValue( key, out v ) )
					Log.Error( string.Format( "Duplicated command line option found {0}. Second defintion ignored", key ) );
				else
					options.Add( key, value );
			}
		}

		public string GetOptionValue( string optionName, string defaultValue )
		{
			string v;
			return options.TryGetValue( optionName.ToLower(), out v ) ? v : defaultValue;
		}

		public bool IsOptionExist( string optionName )
		{
			string v;
			return options.TryGetValue( optionName.ToLower(), out v );
		}
	}
}