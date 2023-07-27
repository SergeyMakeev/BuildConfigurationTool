using System;

using BCT.Source;
using BCT.Source.Model;
using BCT.BuildScript.Vendors;

namespace BCT.BuildScript.BaseProjects
{
	public abstract class BaseCppProject : BaseSkyforgeProject
	{
		protected BaseCppProject( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			language = Language.CPP;

			if ( platform == PlatformType.Durango )
			{
				Define( "F1_XBOXONE=1" );
				//Define( "_MONOLITHIC_EXECUTABLE" );
			}
			else if ( platform == PlatformType.Orbis )
			{
				Define( "F1_PS4=1" );
				Define( "_MONOLITHIC_EXECUTABLE" );
			}
			else if ( platform.IsWindows() )
			{
				Define( "F1_WINDOWS=1" );
			}

            if (platform == PlatformType.Durango)
            {
                Library("vccorlib");
                Library("combase");
                Library("kernelx");
                Library("toolhelpx");
                Library("uuid");
            }

			var pchName = "\"stdafx.h\"";
			if ( platform == PlatformType.Orbis )
			{
				pchName = string.Format( "<{0}/stdafx.h>", projectName );
				AdditionalCompilerOptions.Add("-Wno-error=#pragma-messages");

/*
				if (configuration == Configuration.DEBUG)
				{
					Library("libSceDbgUBSanitizer_stub_weak");
				}
*/ 

                AdditionalCompilerOptions.AddRange(new[]{
                    "-fdelayed-template-parsing",
                    "-fbuiltin",
                    "-fdiagnostics-format=msvc",
                    "-mno-omit-leaf-frame-pointer",
                    "-Wno-microsoft",
                    "-Wno-invalid-source-encoding",
										"-Wno-invalid-offsetof"
                });
								              
                if (configuration.target == Configuration.Target.FINALRELEASE)
                    // possible for final release Warnings should be "NormalWarnings"
                    AdditionalCompilerOptions.AddRange(new[]{
                        "-Wno-unused-value",
                        "-Wno-unused-variable",
                        "-Wno-unused-function",
                        "-Wno-unused-private-field",
                        "-Wno-unused-local-typedef"
                    });

								if (configuration.target != Configuration.Target.DEBUG)
								{
									// add fastMath options
									AdditionalCompilerOptions.AddRange(new[]{
                 "-fno-honor-infinities",
								 "-fno-honor-nans",
								//-fassociative-math			// we don't need this
								 "-freciprocal-math",
								 "-fno-signed-zeros",
								 "-fno-trapping-math",
                    });
								}
            }
			else
			{
                AdditionalCompilerOptions.AddRange(new[]{
                    "/bigobj",
                    "/Zm200"
                });
			}


			UseThirdParty<Profiler>();
			UseThirdParty<TaskScheduler>();

			Define( "_GAMEPROJECT" );
			Define( "_CRT_SECURE_NO_DEPRECATE" );
			Define( "WIN32_LEAN_AND_MEAN" );
			Define( "PCH_NAME=" + pchName );
            Define("_SILENCE_STDEXT_HASH_DEPRECATION_WARNINGS");

			if ( configuration.target == Configuration.Target.DEBUG )
			{
				Define( "_DEBUG" );

				if (platform == PlatformType.Orbis)
				{
					Define("SCE_DBG_ASSERTS_ENABLED");
				}
			}

			if ( configuration.target == Configuration.Target.FINALRELEASE )
			{
				Define("_FINALRELEASE");
				Define("_MONOLITHIC_EXECUTABLE");
			}
			

			if ( platform == PlatformType.Win64 )
			{
				Define( "WIN64" );
			}

            if (configuration.target == Configuration.Target.DEBUG)
            {
                Define("_ITERATOR_DEBUG_LEVEL=1");
            }

			if ( configuration.enableMutableLibDB )
			{
				Define("ENABLE_MUTABLE_LIBDB");
			}

			if ( configuration.developerClient )
			{
				Define("DEVELOPERS_CLIENT");
			}

			characterSet = CharacterSet.UNICODE;
			wholeProgramOptimization = WholeProgramOptimization.NONE;

			outputDirectory = Utilites.GetOutputDir( configuration, platform );
			intermediateDirectory = Utilites.GetTempBuildDir( configuration, platform ) + GetType().Name + "\\";

			if ( configuration.target == Configuration.Target.DEBUG )
			{
				disableOptimization = true;
				incrementalLinking = true;
				generateDebugInformation = true;
			}
			else
			{
				disableOptimization = false;
				incrementalLinking = false;
				generateDebugInformation = true;
			}

			UnityBuildIgnoreFiles( "MemoryRef.cpp" );

			IncludePath( "%(ClientDir)" );
			IncludePath( "%(ClientDir)/BuildSources/BinaryLayout/" );
		}

        public void AddPlatformSpecificProjectFiles(PlatformType platform)
        {
            Files(location + "stdafx.h");
            Files(location + "stdafx.cpp");
						Files( location + "MemoryRef.cpp" );
            AddProjectFiles(location + "Common\\");

            switch (platform)
            {
                case PlatformType.Win32:
                case PlatformType.Win64:
                    {
                        AddProjectFiles(location + "Windows\\");
                        break;
                    }
                case PlatformType.Durango:
                    {
                        AddProjectFiles(location + "Durango\\");
                        break;
                    }
                case PlatformType.Orbis:
                    {
                        AddProjectFiles(location + "Orbis\\");
                        break;
                    }
                default:
                    throw new NotSupportedException();
            }
        }

		public void AddProjectFiles( string overrideLocation = "" )
		{
			var filesLocation = location;
			if ( !string.IsNullOrEmpty( overrideLocation ) )
				filesLocation = overrideLocation;

            Files( string.Format( "{0}*.cxx", filesLocation ) );
			Files( string.Format( "{0}*.cpp", filesLocation ) );
			Files( string.Format( "{0}*.h", filesLocation ) );
			Files( string.Format( "{0}*.hpp", filesLocation ) );
			Files( string.Format( "{0}*.hh", filesLocation ) );

			var projectName = GetType().Name;
			var projectJavaDescriptors = workSpace.ResolveMacroVariables( "%(ClientJavaDescriptors)" + projectName );
			if ( System.IO.Directory.Exists( projectJavaDescriptors ) )
				Files( projectJavaDescriptors + "/*.java" );
		}

        public void AddPlatformSpecificProjectResources(PlatformType platform)
        {

            switch (platform)
            {
                case PlatformType.Win32:
                case PlatformType.Win64:
                    {
                        AddProjectResources(location + "Windows\\");
                        break;
                    }
                case PlatformType.Durango:
                    {
                        //ресурсные файлы Durango должны лежать в корне :( на них есть ссылки из appxmanifest
                        AddProjectResources(location);
                        Exclude(location + "Windows\\*");
                        Exclude(location + "Orbis\\*");
                        break;
                    }
                case PlatformType.Orbis:
                    {
                        AddProjectResources(location + "Orbis\\");
                        break;
                    }
                default:
                    throw new NotSupportedException();
            }
        }


		public void AddProjectResources( string overrideLocation = "" )
		{
			var filesLocation = location;
			if ( !string.IsNullOrEmpty( overrideLocation ) )
				filesLocation = overrideLocation;

			Resources( string.Format( "{0}*.rc", filesLocation ) );
            Resources(string.Format("{0}*.appxmanifest", filesLocation));
            Resources(string.Format("{0}*.png", filesLocation));
            Files(string.Format("{0}*.ico", filesLocation));
		}
	}
}