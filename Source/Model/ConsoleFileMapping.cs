using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BCT.Source.Model
{
    public interface IDebugFileMapping
    {
        string FilePath { get; }

        void Write();
    }

    public sealed class Ps4FileMapping : IDebugFileMapping
    {
        public struct Overlay
        {
            public enum Type 
            {
                Opaque,
                Writable
            }

            public Overlay(Type type, int order, string src, string dst)
            {
                if (order < 0 || order > 127)
                    throw new ArgumentOutOfRangeException();
                if (!string.IsNullOrEmpty(src) && Path.IsPathRooted(src) == false)
                    throw new Exception( "Source path must be absolute!" );
                if (string.IsNullOrEmpty(dst))
                    throw new Exception( "Destination path must be child directory of app0!" );
                this.type = type;
                this.order = order;
                this.src = "/host/" + src;
                this.dst = "/app0/" + dst;
            }

            public readonly Type type;
            public readonly int order;
            public readonly string src;
            public readonly string dst;

            public string GetOverlayType()
            {
                return type.ToString().ToUpper();
            }
        }

        public const int FileVersion = 1;
        public const int OverlayLimit = 8;

        private string workingDirectory = string.Empty;
        private string saveDataDirectory = string.Empty;

        private readonly List<Overlay> overlays;

        public Ps4FileMapping(string fileMappingFolder, string fileName)
        {
            overlays = new List<Overlay>(OverlayLimit);
            FilePath = Path.Combine(fileMappingFolder, fileName + ".ps4path");
        }

        public string FilePath { get; internal set; }

        public string WorkingDirectory
        {
            get { return string.IsNullOrEmpty(workingDirectory) ? string.Empty : "/host/" + workingDirectory; }
            set { workingDirectory = CheckDirValue(value); }
        }

        public string SaveDataDirectory
        {
            get { return string.IsNullOrEmpty(saveDataDirectory) ? string.Empty : "/host/" + saveDataDirectory; }
            set { saveDataDirectory = CheckDirValue(value); }
        }

        public void AddOverlay(Overlay.Type type, int order, string src, string dst)
        {
            var overlay = new Overlay( type, order, src, dst );
            if ( string.IsNullOrEmpty( workingDirectory ) == false )
            {
                var overlayChildFolder = Path.Combine(workingDirectory, dst);
                if ( Directory.Exists( overlayChildFolder ) == false )
                {
                    Directory.CreateDirectory( overlayChildFolder );
                }
            }
            overlays.Add(overlay);
        }

        public void Write()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new StreamWriter(ms, new UTF8Encoding(true)))
                {
                    WriteLine( sw, "version", FileVersion );
                    WriteLine( sw, "app0", WorkingDirectory );
                    WriteLine( sw, "savedataRoot", SaveDataDirectory );
                    
                    // * Unsupported
                    WriteLine( sw, "addcontRoot", string.Empty );
                    WriteLine( sw, "otherApps", string.Empty );
                    WriteLine( sw, "otherAppsPatches", string.Empty );
                    WriteLine( sw, "otherAppsSavedata", string.Empty );
                    WriteLine( sw, "otherAppsContents", string.Empty );
                    // * Unsupported

                    for (var i = 1; i <= OverlayLimit; i++)
                    {
                        var type = string.Empty;
                        var order = string.Empty;
                        var src = string.Empty;
                        var dst = string.Empty;

                        if ( i <= overlays.Count )
                        {
                            var overlay = overlays[i - 1];
                            type = overlay.GetOverlayType();
                            order = overlay.order.ToString();
                            src = overlay.src;
                            dst = overlay.dst;
                        }

                        WriteLine( sw, string.Format("overlay{0}type",  i), type );
                        WriteLine( sw, string.Format("overlay{0}order", i), order );
                        WriteLine( sw, string.Format("overlay{0}src",   i), src );
                        WriteLine( sw, string.Format("overlay{0}dst",   i), dst );
                    }

                    sw.Flush();
                    Utilites.SaveFileIfChanged(FilePath, ms);
                }
            }
        }

        private static void WriteLine(TextWriter sw, object key, object value)
        {
            sw.Write("{0}={1}\r\n", key, value);
        }

        private static string CheckDirValue(string value)
        {
            if (Path.IsPathRooted(value) == false)
                throw new Exception("Path must be absolute!");

            if (Directory.Exists(value) == false)
                Directory.CreateDirectory(value);

            return value;
        }
    }

    public sealed class XboxOneFileMapping : IDebugFileMapping
    {
        public XboxOneFileMapping()
        {
            throw new NotImplementedException();
        }

        public string FilePath { get; set; }
        
        public void Write()
        {
            throw new NotImplementedException();
        }
    }
}
