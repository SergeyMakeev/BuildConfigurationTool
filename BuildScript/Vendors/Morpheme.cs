using System;
using BCT.Source.Model;
using Target = BCT.Source.Model.Configuration.Target;

namespace BCT.BuildScript.Vendors
{
	public class Morpheme : ThirdParty
	{
		public Morpheme( ProjectFile project, PlatformType platform, Configuration configuration )
			: base( project, platform, configuration )
		{
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMPlatform/include" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMNumerics/include" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMRuntimeUtils/include" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMTL/include" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/XMD/include" );

			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/SDK/core/include" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/SDK" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/morpheme/utils/simpleBundle/include" );
			project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMExpression/include" );

			string suffix;
			string platformSuffix = string.Empty;

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMPlatform/include/NMPlatform/win32" );
					project.LibrariesPath( "%(VendorsDir)Morpheme/NaturalMotion/lib/win32/vs10.0" );

					suffix = configuration.UseDebugVendors() ? "_target_LE32_debug" : "_target_LE32";
					platformSuffix = configuration.UseDebugVendors() || configuration.target == Target.FINALRELEASE
						                 ? string.Empty
						                 : "_release"; // Версия с перехватом ассертов
					break;
				}
				case PlatformType.Win64:
				{
					project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMPlatform/include/NMPlatform/win32" );
					project.LibrariesPath( "%(VendorsDir)Morpheme/NaturalMotion/lib/x64/vs10.0" );

					suffix = configuration.UseDebugVendors() ? "_target_LE64_debug" : "_target_LE64";
					platformSuffix = configuration.UseDebugVendors() || configuration.target == Target.FINALRELEASE
						                 ? string.Empty
						                 : "_release"; // Версия с перехватом ассертов
					break;
				}
				case PlatformType.Orbis:
				{
					project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMPlatform/include/NMPlatform/PS4" );
					project.LibrariesPath( "%(VendorsDir)Morpheme/NaturalMotion/lib/ps4/vs11.0" );

					suffix = GetSuffix( configuration.target, "_debug.a", "_release.a", "_shipping.a" );
					break;
				}
				case PlatformType.Durango:
				{
					project.IncludePath( "%(VendorsDir)Morpheme/NaturalMotion/src/common/NMPlatform/include/NMPlatform/XboxOne" );
					project.LibrariesPath( "%(VendorsDir)Morpheme/NaturalMotion/lib/xboxone/vs14.0" );

					suffix = GetSuffix( configuration.target, "_debug.lib", "_release.lib", "_shipping.lib" );
					break;
				}
				default:
					throw new NotSupportedException();
			}

			project.Library( "morphemeCore" + suffix );
			project.Library( "morphemeSimpleBundle" + suffix );
			project.Library( "NMRuntimeUtils" + suffix );
			project.Library( "NMExpression" + suffix );
			project.Library( "NMPlatform" + suffix + platformSuffix );
		}

		static private string GetSuffix( Target target, string debug, string release, string finalRelease )
		{
			switch ( target )
			{
				case Target.DEBUG:
					return debug;
				case Target.RELEASE:
					return release;
				case Target.FINALRELEASE:
					return finalRelease;
				default:
					throw new ArgumentOutOfRangeException( "target" );
			}
		}
	}
}