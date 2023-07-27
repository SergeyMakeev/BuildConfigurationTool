namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    internal sealed class PropertyElement : ProjectElement 
    {
        public PropertyElement( string name, string value=null ) : base( name )
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

    class PropertyGroupElement : ElementContainer
    {
        public PropertyGroupElement() : base( "PropertyGroup" ) {}

        public PropertyElement AddProperty( PropertyElement propertyElement )
        {
            AppendElement( propertyElement );
            return propertyElement;
        }

        public PropertyElement AddProperty(string name, string value)
        {
            var propertyElement = new PropertyElement(name, value);
            AddProperty( propertyElement );
            return propertyElement;
        }
    }
}
