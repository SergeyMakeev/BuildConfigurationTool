using BCT.Source;
using BCT.Source.Model;
using BCT.Source.Generators;
using BCT.BuildScript.Solutions;

namespace BCT.BuildScript
{
    class Make : IMake
    {    
        public bool Build(CommandLineOptions options)
        {
            var workspace = new Workspace(options);
						workspace.Solution( typeof(GameUnpacked) );
						workspace.Solution( typeof(GamePacked) );
						workspace.Solution( typeof(Bots) );
						workspace.Solution( typeof(_F1) );
						workspace.Solution( typeof(Smoke) );
						workspace.Solution(typeof(RemoteConsole));
						workspace.Solution(typeof(PackingProcess));
            
            //--------------
            var targetDir = Utilites.GetCurrentDirectory();
            Utilites.SetTargetDirectory(targetDir);

            workspace.SetVariable("VendorsDir", Utilites.GetCurrentDirectory() + "../../vendors/Client/");

						//FixupSlashesUnixStyle нужен, что бы пихать значения в макрос
            workspace.SetVariable("ClientDir", Utilites.FixupSlashesUnixStyle(Utilites.GetCurrentDirectory()));
            workspace.SetVariable("ClientJavaDescriptors", Utilites.GetCurrentDirectory() + "JavaDescriptors\\client\\");
            workspace.SetVariable("BinDir", Utilites.GetCurrentDirectory() + "BuildConfigurationTool\\bin\\");
            workspace.SetVariable("CodeToolsDir", Utilites.GetCurrentDirectory() + "../../codetools/");
						//workspace.SetVariable("GameWorkingDirectory", Utilites.GetCurrentDirectory() + "../../codetools/");

            var generator = new GeneratorVisualStudio();
            //var generator = new GeneratorFastBuild();
            workspace.Build(generator);

            workspace.CleanTargetDirectories();

            //Execute copy-extra-resources.bat 
            workspace.CopyExtraBinResources();
            return true;
        }
    }
}
