using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Section
    {

        public SectionTypes SectionType { get; private set; }

        public Section(SectionTypes sectionType)
        {
            this.SectionType = sectionType;
        }

        // check which number is the lowest one and increase all other numbers with that number.
        // public Section(SectionTypes sectionType, int x, int y)
        // {
        //    // this.SectionType = sectionType;
        // }

    }

    public enum SectionTypes
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }

}
