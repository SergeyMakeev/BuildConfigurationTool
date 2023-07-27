namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    sealed class TargetElement : ElementContainer
    {
        private string name;

        public TargetElement() : base("Target") { }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || name == value)
                    return;
                xmlElement.SetAttributeValue("Name", value);
                name = value;
            }
        }
    }
}

