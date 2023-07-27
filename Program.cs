using System;
using BCT.Source;

namespace BCT
{
	class Program
	{
		static int Main( string[] args )
		{
			var options = new CommandLineOptions(args);
			var buildScriptRunner = new BuildScriptRunner(options);
			try
			{
			    using ( new TimeScope( "Total time: {0:F0} sec" ) )
			    {
                    return buildScriptRunner.Run();
			    }
            }
            catch ( BCTInvalidOperation ex )
			{
                if (ex.HasMessage) 
                    Log.Error(ex.Message);
			    return 0xffff;
			}
			catch ( Exception ex )
			{
				Log.Error( ex.ToString() );
				Log.Error( "\n[ GENERAL FAILURE ]\n" );
				return 0xdead;
			}
		}
	}
}