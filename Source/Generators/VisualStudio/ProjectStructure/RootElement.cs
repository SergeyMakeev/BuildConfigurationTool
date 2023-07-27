namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    class RootElement: ElementContainer
    {
        private string defaultTargets;
        private string toolsVersion;

        public RootElement() : base( "Project" ) {}

        public string DefaultTargets
        {
            get
            {
                return defaultTargets;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || defaultTargets == value)
                    return;
                xmlElement.SetAttributeValue("DefaultTargets", value);
                defaultTargets = value;
            }
        }

        public string ToolsVersion
        {
            get
            {
                return toolsVersion;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || toolsVersion == value)
                    return;
                xmlElement.SetAttributeValue("ToolsVersion", value);
                toolsVersion = value;
            }
        }

        public PropertyGroupElement CreatePropertyGroupElement(params PropertyElement[] propertyGroupElements)
        {
            var propertyGroup = new PropertyGroupElement();
            foreach (var propertyGroupElement in propertyGroupElements)
            {
                propertyGroup.AddProperty(propertyGroupElement);
            }
            AppendElement(propertyGroup);
            return propertyGroup;
        }
  
        public ItemGroupElement CreateItemGroupElement()
        {
            var itemGroup = new ItemGroupElement();
            AppendElement(itemGroup);
            return itemGroup;
        }

        public ItemDefinitionGroupElement CreateProjectItemDefinitionGroupElement()
        {
            var importDefinitionGroup = new ItemDefinitionGroupElement();
            AppendElement(importDefinitionGroup);
            return importDefinitionGroup;
        }

        public ImportElement CreateImportElement(string project)
        {
            var import = new ImportElement { Project = project };
            AppendElement(import);
            return import;
        }

        public ImportGroupElement CreateImportGroupElement()
        {
            var importGroup = new ImportGroupElement();
            AppendElement(importGroup);
            return importGroup;
        }

        public TargetElement CreateTargetElement(string name)
        {
            var target = new TargetElement { Name = name };
            AppendElement(target);
            return target;
        }
        
    }
}
