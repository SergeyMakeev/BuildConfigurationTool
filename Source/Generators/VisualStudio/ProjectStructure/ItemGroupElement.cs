namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    internal sealed class MetadataElement: ProjectElement
    {
        public MetadataElement( string name, string value=null) : base( name )
        {
            Value = value;
        }

        public string Name
        {
            get { return xmlElement.Name.LocalName; }
        }

        public string Value
        {
            get { return xmlElement.Value; }
            set
            {
                if ( string.IsNullOrEmpty( value ) || Value == value )
                    return;
                xmlElement.SetValue(value);
            }
        }
    }
    internal sealed class ItemElement : ElementContainer
    {
        private string include;
        public bool hasMetadata = false;
        public ItemElement(string itemType) : base(itemType) {}

        public string ItemType
        {
            get { return xmlElement.Name.LocalName; }
        }
        public string Include
        {
            get
            {
                return include;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || include == value)
                    return;
                xmlElement.SetAttributeValue("Include", value);
                include = value;
            }
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

    class ItemGroupElement : ElementContainer
    {
        public bool hasItems = false;
        public ItemGroupElement() : base( "ItemGroup" ){}

        public ItemElement AddItem(string itemType)
        {
            return AddItem( itemType, null );
        }

        public ItemElement AddItem(string itemType, string include, params MetadataElement[] metadataElements)
        {
            var item = new ItemElement(itemType) { Include = include };
            if ( metadataElements != null )
            {
                foreach ( var metadataElement in metadataElements )
                {
                    item.AddMetadata( metadataElement );
                }
            }
            AppendElement(item);
            hasItems = true;
            return item;
        }
    }
}
