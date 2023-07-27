using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

using BCT.Source.Model;
using BCT.Source.Generators.VisualStudio.ProjectStructure;


namespace BCT.Source.Generators.VisualStudio
{
    // ReSharper disable InconsistentNaming
    internal abstract class VSFile : IDisposable
    // ReSharper restore InconsistentNaming
    {
        private readonly MemoryStream memoryStream;
        protected readonly StreamWriter streamWriter;
        
        internal VSFile()
        {
            memoryStream = new MemoryStream();
            streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(true));
        }

        protected virtual void Write(string str, params object[] args)
        {
            streamWriter.WriteLine(str, args);
        }

        public abstract string FileFullPath { get; }

        public virtual void Save()
        {
            streamWriter.Flush();
            Utilites.SaveFileIfChanged(FileFullPath, memoryStream);
        }

        public void Dispose()
        {
            if (streamWriter != null)
                streamWriter.Dispose();
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    // ReSharper disable InconsistentNaming
    internal abstract class VSXmlFile : VSFile
    // ReSharper restore InconsistentNaming
    {
        protected readonly XDocument xmlDocument;
        protected readonly RootElement projectRoot;

        protected VSXmlFile(string defaultTargets=null)
        {
            projectRoot = new RootElement
            {
                DefaultTargets = defaultTargets,
                ToolsVersion = VSVersion.CurrentVersion.BuildToolsVersion
            };
            xmlDocument = new XDocument(projectRoot.xmlElement);
        }

        public override void Save()
        {
            xmlDocument.Save(streamWriter);
            base.Save();
        }
    }
}
