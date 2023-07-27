using System;
using BCT.BuildScript.BaseProjects;
using BCT.BuildScript.Vendors;
using BCT.Source.Model;

namespace BCT.BuildScript.Projects
{
	public class ShaderBuilder : BaseCliProject
	{
		public ShaderBuilder( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			if ( ReferencesThirdParty.Count > 0 )
				throw new Exception( "No third party here!" );

			if ( ReferencedAssemblies.Count > 0 )
				throw new Exception( "No managed libraries here!" );

			if ( GetLibrariesCount() > 0 )
				throw new Exception( "No native libraries here!" );

			if ( GetDependsOnCount() > 0 )
				throw new Exception( "No references to other projects here!" );

			applicationKind = ApplicationKind.SHARED_LIBRARY;

			UseThirdParty<D3D9>();

			AddProjectFiles();

			ReferenceAssembly( "System" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Xml" );

			ReferenceAssembly( "CompilerIPC", "%(VendorsDir)ShaderCompiler/Bin/CompilerIPC.dll" );

			Library( "CompilerBackend" );
			DelayLoaded( "CompilerBackend.dll" );

			switch ( platform )
			{
				case PlatformType.Win32:
				{
					LibrariesPath( "%(VendorsDir)ShaderCompiler/Bin/x86" );
					break;
				}
				case PlatformType.Win64:
				{
					LibrariesPath( "%(VendorsDir)ShaderCompiler/Bin/x64" );
					break;
				}
				default:
					throw new NotSupportedException();
			}
		}
	}
}