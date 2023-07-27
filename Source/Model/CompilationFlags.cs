// ReSharper disable InconsistentNaming
using System;

namespace BCT.Source.Model
{
	public class Configuration : IComparable
	{
		public enum Target
		{
			DEBUG,
			RELEASE,
			FINALRELEASE /* shipping configuration */
		}

		public readonly string name;
		public readonly Target target;
		public readonly bool linkTimeOptimization = false;

		public Configuration(string name, Target target, bool linkTimeOptimization)
		{
			this.name = name;
			this.target = target;
			this.linkTimeOptimization = linkTimeOptimization;
		}


		public int CompareTo(object obj)
		{
			var other = obj as Configuration;
			return other == null ? 1 : String.Compare(name, other.name, StringComparison.Ordinal);
		}

		public override bool Equals( object obj )
		{
			var other = obj as Configuration;
			if (other == null)
				return false;
			return name.Equals(other.name);
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		public override string ToString()
		{
			return name;
		}

		public string GetTargetConfigurationName()
		{
			switch (target)
			{
				case Target.DEBUG:
					return "Debug";
				case Target.RELEASE:
					return "Release";
				case Target.FINALRELEASE:
					return "FinalRelease";
				default:
					return null;
			}
		}
	}

	public enum PlatformType
	{
		Win32,
		Win64,
		Durango,
        Orbis
	}

	public enum CharacterSet
	{
		UNICODE,
		ANSI
	}

	public enum WholeProgramOptimization
	{
		NONE,
		LINK_TIME_CODE_GENERATION
	}

	public enum ApplicationKind
	{
		CONSOLE_APPLICATION,
		WINDOWED_APPLICATION,
		SHARED_LIBRARY,
		STATIC_LIBRARY,
		OBJECT_LIST,

		UTILITY /*use it for empty projects*/
	}

	public enum Language
	{
		CPP,
		C_SHARP,
		CPP_CLI
	}

    public enum UnityBuildType
    {
        NEVER,
        ALWAYS,
        DEFINED_BY_USER
    }

    public static class PlatformTypeExtensions
    {
        public static bool IsWindows(this PlatformType platformType)
        {
            return platformType == PlatformType.Win32 || platformType == PlatformType.Win64;
        }
    }
}