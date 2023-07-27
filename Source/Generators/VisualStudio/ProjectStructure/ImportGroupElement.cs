namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    sealed class ImportElement : ProjectElement
    {
        private string project;
        
        public ImportElement() : base("Import") { }
        
        public string Project
        {
            get
            {
                return project;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || project == value)
                    return;
                xmlElement.SetAttributeValue("Project", value);
                project = value;
            }
        }
    }
    sealed class ImportGroupElement : ElementContainer
    {
        public ImportGroupElement() : base( "ImportGroup" ) {}

        public ImportElement AddImport(string project)
        {
            return AddImport( new ImportElement() { Project = project } );
        }

        public ImportElement AddImport( ImportElement importElement )
        {
            AppendElement(importElement);
            return importElement;
        }
    }
}
