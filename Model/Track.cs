using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {

        public string Name { get; private set; }

        public LinkedList<Section> Sections { get; private set; }

        public Track(string name, SectionTypes[] sections)
        {
            this.Name = name;
            this.Sections = this.SectionTypeToSections(sections);
        }

        private LinkedList<Section> SectionTypeToSections(SectionTypes[] sectionTypes)
        {
            LinkedList<Section> sections = new LinkedList<Section>();
            foreach (SectionTypes sectionType in sectionTypes)
            {
                sections.AddLast(new Section(sectionType));
            }

            return sections;
        }

    }
}
