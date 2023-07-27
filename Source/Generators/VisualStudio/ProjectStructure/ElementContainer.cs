using System.Collections.Generic;

namespace BCT.Source.Generators.VisualStudio.ProjectStructure
{
    abstract class ElementContainer: ProjectElement
    {
        protected ElementContainer( string name ) : base( name ) {}

        public void AppendElement(ProjectElement projectElement)
        {
            xmlElement.Add(projectElement.xmlElement);
        }

        public void AppendElements(IEnumerable<ProjectElement> projectElements)
        {
            foreach ( var projectElement in projectElements )
            {
                AppendElement( projectElement );
            }
        }
    }
}
