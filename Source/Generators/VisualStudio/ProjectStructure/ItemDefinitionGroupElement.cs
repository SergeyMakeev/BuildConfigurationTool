namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    internal sealed class ItemDefinitionElement : ElementContainer
    {
        public bool hasMetadata = false;
        public ItemDefinitionElement(string itemType) : base(itemType) { }

        public string ItemType
        {
            get { return xmlElement.Name.LocalName; }
        }

        public MetadataElement AddMetadata(MetadataElement metadata)
        {
            AppendElement(metadata);
            hasMetadata = true;
            return metadata;
        }

        public MetadataElement AddMetadata(string name, string value)
        {
            var metadata = new MetadataElement(name, value);
            return AddMetadata(metadata);
        }
    }

    class ItemDefinitionGroupElement: ElementContainer
    {
        public ItemDefinitionGroupElement() : base("ItemDefinitionGroup") { }
  
        public ItemDefinitionElement AddItemDefinition(string itemType, params MetadataElement[] metadataElements)
        {
            var item = new ItemDefinitionElement(itemType);
            if (metadataElements != null)
            {
                foreach (var metadataElement in metadataElements)
                {
                    item.AddMetadata(metadataElement);
                }
            }
            AppendElement(item);
            return item;
        }
    }
}
