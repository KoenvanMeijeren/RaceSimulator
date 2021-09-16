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

        public Track(string name, Section[] sections)
        {
            this.Name = name;
            this.Sections = new LinkedList<Section>(sections);
        }

    }
}
