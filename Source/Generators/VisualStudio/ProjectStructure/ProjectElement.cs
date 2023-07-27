using System.Xml.Linq;


namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    interface IProjectElement {}

    abstract class ProjectElement : IProjectElement
    {
        private string condition;
        private readonly XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
        public readonly XElement xmlElement;

        protected ProjectElement(string name)
        {
            xmlElement = new XElement(ns + name);
        }

        public void AttachToXmlDocument(XDocument xdoc)
        {
            xdoc.Add(xmlElement);
        }

        public virtual string Condition
        {
            get
            {
                return condition;
            }
            set
            {
                xmlElement.SetAttributeValue("Condition", value);
                condition = value;
            }
        }

        public virtual string Label
        {
            get
            {
                return condition;
            }
            set
            {
                xmlElement.SetAttributeValue("Label", value);
                condition = value;
            }
        }
    }
}
