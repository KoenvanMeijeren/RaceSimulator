using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public const int StartPositionUndefined = -1;
        public string Name { get; private set; }

        public LinkedList<Section> Sections { get; private set; }

        public int EastStartPosition { get; private set; }
        public int NorthStartPosition { get; private set; }

        public Track(string name, SectionTypes[] sections) : this(name, sections, StartPositionUndefined, StartPositionUndefined)
        {
            
        }

        public Track(string name, SectionTypes[] sections, int eastStartPosition, int northStartPosition)
        {
            this.Name = name;
            this.Sections = this.SectionTypeToSections(sections);
            this.EastStartPosition = eastStartPosition;
            this.NorthStartPosition = northStartPosition;
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
