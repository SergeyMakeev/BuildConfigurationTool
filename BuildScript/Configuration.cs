namespace BCT.BuildScript
{
	public class Configuration : BCT.Source.Model.Configuration
	{
		public bool enableProfiling = true;
		public bool enableMutableLibDB = true;
		public bool developerClient = true;

		public bool UseDebugVendors()
		{
			return target == Target.DEBUG;
		}

		public Configuration(string name, Target target, bool linkTimeOptimization)
			: base(name, target, linkTimeOptimization)
		{

		}
	}

	// Специально в отдельном классе, чтобы сломалась компиляция для вот такой строчки configuration == Configuration.DEBUG;
	public class ConfigurationFactory
	{
		static public Configuration DEBUG = new Configuration("Debug", Configuration.Target.DEBUG, false);
		static public Configuration RELEASE = new Configuration("Release", Configuration.Target.RELEASE, false);

		static public Configuration FINALRELEASE = new Configuration("FinalRelease", Configuration.Target.FINALRELEASE, true) { enableMutableLibDB = false, developerClient = false, enableProfiling = false };
		static public Configuration FINALRELEASE_PROFILE = new Configuration("FinalRelease_Profile", Configuration.Target.FINALRELEASE, false) { enableMutableLibDB = false };

		static public Configuration DEBUG_PACKED = new Configuration("Debug_Packed", Configuration.Target.DEBUG, false) { enableMutableLibDB = false };
		static public Configuration RELEASE_PACKED = new Configuration("Release_Packed", Configuration.Target.RELEASE, false) { enableMutableLibDB = false };

	}

}