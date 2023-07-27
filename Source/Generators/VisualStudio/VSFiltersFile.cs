namespace BCT.Source.Generators.VisualStudio
{
    
    // ReSharper disable InconsistentNaming
    class VSFiltersFile : VSXmlFile
    // ReSharper restore InconsistentNaming
    {

        // TODO: этот метод был взят как есть из GeneratorMSVC2010.cs        
//        public bool BuildFilters(string projectName, string projectFileName, List<FileDesc> files)
//        {
//            var filterFiles = new List<FilterFileDesc>();
//
//            var uniqueFolderAndGuids = new SortedDictionary<string, string>();
//
//            foreach (var fileDesc in files)
//            {
//                var dirName = Path.GetDirectoryName(fileDesc.fileName) ?? string.Empty;
//
//                var fileName = fileDesc.fileName.Substring(dirName.Length + 1, fileDesc.fileName.Length - dirName.Length - 2);
//
//                dirName = dirName.Replace("..\\", "");
//
//                if (dirName.StartsWith(projectName, true, null))
//                {
//                    dirName = dirName.Substring(projectName.Length);
//                    dirName = dirName.TrimStart('\\');
//                }
//
//                var filterFileDesc = new FilterFileDesc
//                {
//                    path = dirName,
//                    fileName = fileName,
//                    fullNameAndPath = fileDesc.fileName
//                };
//
//                filterFiles.Add(filterFileDesc);
//
//                if (string.IsNullOrEmpty(dirName))
//                    continue;
//
//                string guid;
//                if (!uniqueFolderAndGuids.TryGetValue(dirName, out guid))
//                {
//                    guid = Utilites.GetGuidFromString(dirName);
//                    uniqueFolderAndGuids.Add(dirName, guid);
//                }
//
//                var parentDirList = dirName.Split('\\');
//
//                var parentDir = "";
//                for (var i = 0; i < parentDirList.Length; i++)
//                {
//                    if (i > 0)
//                        parentDir += "\\";
//                    parentDir += parentDirList[i];
//
//                    if (uniqueFolderAndGuids.TryGetValue(parentDir, out guid))
//                        continue;
//
//                    var parentGuid = Utilites.GetGuidFromString(parentDir);
//                    uniqueFolderAndGuids.Add(parentDir, parentGuid);
//                }
//            }
//
//            using (var memoryStream = new MemoryStream())
//            {
//                using (var output = new StreamWriter(memoryStream))
//                {
//                    output.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
//                    output.WriteLine("<Project ToolsVersion=\"4.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
//
//                    output.WriteLine("  <ItemGroup>");
//
//                    foreach (var filterFileDesc in filterFiles)
//                    {
//                        var fileType = Utilites.GetFileType(filterFileDesc.fullNameAndPath);
//                        var stringFileType = GetIncludeTypeFromFileType(fileType);
//
//                        output.WriteLine("    <{0} Include=\"{1}\">", stringFileType, filterFileDesc.fullNameAndPath);
//
//                        if (!string.IsNullOrEmpty(filterFileDesc.path))
//                            output.WriteLine("      <Filter>{0}</Filter>", filterFileDesc.path);
//                        output.WriteLine("    </{0}>", stringFileType);
//                    }
//
//                    output.WriteLine("  </ItemGroup>");
//
//                    if (uniqueFolderAndGuids.Count > 0)
//                    {
//                        output.WriteLine("  <ItemGroup>");
//
//                        foreach (var folderDesc in uniqueFolderAndGuids)
//                        {
//                            output.WriteLine("    <Filter Include=\"{0}\">", folderDesc.Key);
//                            output.WriteLine("      <UniqueIdentifier>{{{0}}}</UniqueIdentifier>", folderDesc.Value);
//                            output.WriteLine("    </Filter>");
//                        }
//
//                        output.WriteLine("  </ItemGroup>");
//                    }
//
//                    output.WriteLine("</Project>");
//
//                    output.Flush();
//
//                    //save here
//                    var filtersFileName = projectFileName + ".filters";
//                    Utilites.SaveFileIfChanged(filtersFileName, memoryStream);
//                }
//            }
//
//            return true;
//        }
//        public class FilterFileDesc
//        {
//            public string fileName;
//            public string fullNameAndPath;
//            public string path;
//        }

        public override string FileFullPath { get { throw new System.NotImplementedException(); } }
    }
}
