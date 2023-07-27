using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace BCT.Source.Model
{
    #region VSVersion
    // ReSharper disable InconsistentNaming
    public sealed class VSVersion : IComparable<VSVersion>
    // ReSharper restore InconsistentNaming
    /*
    +---------------------+---------------+-----------+----------------+--------+---------+
    |    Product name     |   Codename    | Version # | .NET Framework | cl.exe | Toolset |
    +---------------------+---------------+-----------+----------------+--------+---------+    
    | Visual Studio 2010  | Dev10/Rosario | 10.0.*    | 2.0 – 4.0      |  16.*  |  v100   |
    | Visual Studio 2012  | Dev11         | 11.0.*    | 2.0 – 4.5.2    |  17.*  |  v110   |
    | Visual Studio 2013  | Dev12         | 12.0.*    | 2.0 – 4.5.2    |  18.*  |  v120   |
    | Visual Studio 2015  | Dev14         | 14.0.*    | 2.0 – 4.6      |  19.*  |  v140   |
    +---------------------+---------------+-----------+----------------+--------+---------+
    */
    {
        private static readonly SortedDictionary<int, VSVersion> supportedVersions = 
            new SortedDictionary<int, VSVersion>();

        public static readonly VSVersion Vs2010 = new VSVersion(2010)
        {
            SolutionFileVersion = "11.00",
            BuildToolsVersion = "4.0",
            PlatformToolset = "v100",
            TargetFramework = "v4.0",
            ProductVersion = "8.0.30703"
        };

        public static readonly VSVersion Vs2012 = new VSVersion(2012)
        {
            SolutionFileVersion = "12.00",
            BuildToolsVersion = "4.0",
            PlatformToolset = "v110",
            TargetFramework = "v4.5"
        };

        public static readonly VSVersion Vs2013 = new VSVersion(2013)
        {
            SolutionFileVersion = "12.00",
            BuildToolsVersion = "12.0",
            PlatformToolset = "v120",
            TargetFramework = "v4.5"
        };

        public static readonly VSVersion Vs2015 = new VSVersion(2015)
        {
            SolutionFileVersion = "12.00",
            BuildToolsVersion = "14.0",
            PlatformToolset = "v140",
            TargetFramework = "v4.5"
        };

        public static VSVersion Default = Vs2010;
        
        private static VSVersion currentVersion = Default;

        private readonly int version;

        VSVersion(int version)
        {
            this.version = version;
            supportedVersions[version] = this;
        }

        public string SolutionFileVersion { get; private set; }
        
        public string BuildToolsVersion { get; private set; }
        
        public string PlatformToolset { get; internal set; }

        public string TargetFramework { get; private set; }

        public string ProductVersion { get; private set; }

        public string ProductName { get { return "Visual Studio " + version; } }

        public string ShortName { get { return this == Vs2015 ? "14" : version.ToString(); } }

        public string FullName { get { return "Visual Studio " + ShortName; } }
   
        public bool IsSuitableForPaltform(PlatformType platform)
        {
            switch ( platform )
            {
                case PlatformType.Durango:
                case PlatformType.Orbis:
                    return this >= Vs2012;
                case PlatformType.Win32:
                case PlatformType.Win64:
                    return this >= Vs2010;
                default:
                    return false;
            }
        }

        public static ReadOnlyCollection<VSVersion> SupportedVersions
        { 
            get { return supportedVersions.Values.ToList().AsReadOnly();} 
        }

        public static VSVersion CurrentVersion
        {
            get { return currentVersion; }
        }
        
        public static VSVersion SetVersion(string version)
        {
            return (currentVersion = Parse(version));
        }

        #region
        public override string ToString()
        {
            return "Vs" + version;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is VSVersion && Equals((VSVersion)obj);
        }

        public override int GetHashCode()
        {
            return version;
        }

        public int CompareTo(VSVersion other)
        {
            return version.CompareTo(other.version);
        }

        public static int Compare(VSVersion v1, VSVersion v2)
        {
            return Comparer<VSVersion>.Default.Compare(v1, v2);
        }

        public static bool operator !=(VSVersion left, VSVersion right)
        {
            return Compare(left, right) != 0;
        }

        public static bool operator ==(VSVersion left, VSVersion right)
        {
            return Compare(left, right) == 0;
        }

        public static bool operator >(VSVersion left, VSVersion right)
        {
            return Compare(left, right) == 1;
        }

        public static bool operator <(VSVersion left, VSVersion right)
        {
            return Compare(left, right) == -1;
        }

        public static bool operator >=(VSVersion left, VSVersion right)
        {
            return Compare(left, right) >= 0;
        }

        public static bool operator <=( VSVersion left, VSVersion right )
        {
            return Compare(left, right) <= 0;
        }

        public static implicit operator string(VSVersion version)
        {
            return version.ToString();
        }

        private bool Equals(VSVersion other)
        {
            return version == other.version;
        }

        private static VSVersion Parse(string version)
        {
            if (!string.IsNullOrEmpty(version))
            {
                var match = Regex.Match(version, @"^(?:vs)*(\d\d\d\d)$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    VSVersion result;
                    if (supportedVersions.TryGetValue(int.Parse(match.Groups[1].Value), out result))
                        return result;
                }
            }
            throw new NotSupportedException();
        }
        #endregion
    }
    #endregion

    #region Toolset options

    public struct Option
    {
        public string name;
        public string value;
    }

    public class CompilerOptions : IEnumerable<Option>
    {
        protected readonly List<Option> options;

        public CompilerOptions()
        {
            options = new List<Option>();
        }

        public void Add( string name, string value )
        {
            options.Add(new Option { name = name, value = value });
        }

        public static CompilerOptions GetOptionsForProject(ProjectFile project)
        {
            var options = new CompilerOptions { { "MultiProcessorCompilation", "true" } };

            switch (project.platform)
            {
                case PlatformType.Orbis:
                    if (project.generateDebugInformation)
                    {
                        //Specifies if the compiler will generate debugging information. You must also change linker settings appropriately to match.   (-g)
                        options.Add("GenerateDebugInformation", "true");
                        if (project.configuration.target != Configuration.Target.FINALRELEASE)
                        {
                            //Enable the generation of debug information for inlined functions. Use this option to improve debuggability of optimized code. Increases executable size.   (-ginlined-scopes)
                            options.Add("InlinedScopes", "true");
                        }
                    }
                    {
                        //Selects the extent of warnings issued during compilation; errors are always displayed.   (-Wall, -w)
                        options.Add("Warnings", "MoreWarnings");
                    }
                    {
                        //Generate extra warnings for certain events not included when using the -Wall switch.   (-Wextra)
                        options.Add("ExtraWarnings", "false"); //""
                    }
                    if (project.treatWarningsAsErrors)
                    {
                        //Enables the compiler to treat warnings as errors.   (-Werror)
                        options.Add("WarningsAsErrors", "true");
                    }
                    if (project.disableOptimization == false)
                    {
                        options.Add("OptimizationLevel", "Level3");//" -O3"
                        //options.Add("FastMath", "true"); //" -ffast-math"
                        options.Add("UnrollLoops", "true"); //" -funroll-loops"

												options.Add("LinkTimeOptimization", project.configuration.linkTimeOptimization ? "true" : "false");
                    }
                    else
                    {
                        options.Add("OptimizationLevel", "Level0"); //" -O0"
                    }
                    {
                        //Enable Exception handling in C++ code.   (-fexceptions)
                        options.Add("CppExceptions", "false"); //""
                    }
                    {
                        //"Enables some non-standard MS VC++ extensions. Some cases of unnamed fields in structures and unions are only accepted with this option.   (-fms-extensions)"
                        options.Add("MsExtensions", "true");
                    }
                    options.Add("DisableSpecificWarnings", string.Join(";", project.DisabledClangWarning) + ";%(DisableSpecificWarnings)");
                    break;
                default:
                    options.Add("Optimization", project.disableOptimization ? "Disabled" : "Full");
                    if (project.treatWarningsAsErrors)
                        options.Add("TreatWarningAsError", "true");

                    if (project.disableOptimization == false)
                    {
                        options.Add("InlineFunctionExpansion", "AnySuitable");
                        options.Add("IntrinsicFunctions", "true");
                        options.Add("FavorSizeOrSpeed", "Speed");
                    }

                    options.Add("RuntimeLibrary", project.disableOptimization == false ? "MultiThreadedDLL" : "MultiThreadedDebugDLL");
                    options.Add("UseFullPaths", "true");

                    if (project.configuration.target != Configuration.Target.DEBUG)
                        options.Add("FunctionLevelLinking", "true");

                    if (project.usePrecompiledHeaders)
                        options.Add("PrecompiledHeader", "Use");

                    if (project.language == Language.CPP_CLI)
                    {
                        options.Add("DisableSpecificWarnings", "4945");
                        options.Add("WarningLevel", "Level4");
                    }
                    else
                    {
					    if (VSVersion.CurrentVersion == VSVersion.Vs2010)
					    {
						    options.Add("EnableEnhancedInstructionSet", "StreamingSIMDExtensions2");
					    }
					    else
					    {
                            if (project.platform == PlatformType.Durango)
                            {
                                options.Add("EnableEnhancedInstructionSet", "AdvancedVectorExtensions");
                                //4458 - declaration hides class member
                                //4459 - declaration hides global declaration
                                //4456 - declaration hides previous local declaration
                                //4457 - declaration hides function parameter
                                options.Add("DisableSpecificWarnings", "4456;4457;4458;4459;");
                            }
					    }

                        options.Add("FloatingPointModel", "Fast");
                        options.Add("MinimalRebuild", "false");
                        options.Add("ExceptionHandling", project.enableExceptionHandling ? "Sync" : "False");
                        options.Add("RuntimeTypeInfo", "true");
                        options.Add("WarningLevel", "Level4");
                        options.Add("CallingConvention", "Cdecl");
                        options.Add("StringPooling", project.stringPooling ? "true" : "false");
                    }

                    if (project.disableOptimization == false)
                        options.Add("BufferSecurityCheck", project.bufferSecurityCheck ? "true" : "false");

                    if (project.generateDebugInformation)
                        options.Add("DebugInformationFormat", "ProgramDatabase");

                    if (project.platform == PlatformType.Durango)
                        options.Add("CompileAsWinRT", "false");
                    break;
            }
            return options;
        }

        public IEnumerator<Option> GetEnumerator()
        {
            return options.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    #endregion

    #region SDK reference
    // ReSharper disable InconsistentNaming
    public sealed class SDKReference : IComparable<SDKReference>
    // ReSharper restore InconsistentNaming
    {
        public const string Version = "8.0";

        public static readonly SDKReference XboxGameChat = new SDKReference("Xbox GameChat API");
        public static readonly SDKReference XboxServices = new SDKReference("Xbox Services API");
        public static readonly SDKReference XboxServicesCpp = new SDKReference("Xbox.Services.API.Cpp", true);
        public static readonly SDKReference XboxDevKitExtensions = new SDKReference("XboxDevKitExtensions", true);

        private readonly string name;

        SDKReference(string name, bool hasExternalProperty = false)
        {
            this.name = name;
            HasExternalProperty = hasExternalProperty;
        }

        public bool HasExternalProperty { get; private set; }

        public string GetIdentity()
        {
            return string.Format("{0}, Version={1}", name, Version);
        }

        public string GetPropertySheetsFilePath()
        {
            return HasExternalProperty ? string.Format("$(XboxOneExtensionSDKLatest)\\ExtensionSDKs\\{0}\\{1}\\CommonConfiguration\\Neutral\\{0}.props", name, Version) : null;
        }

        public int CompareTo( SDKReference other )
        {
            return string.CompareOrdinal(GetIdentity(), other.GetIdentity());
        }
    }
    #endregion
}
