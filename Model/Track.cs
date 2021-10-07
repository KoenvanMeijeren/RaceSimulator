using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum Directions
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public class Track
    {
        private const Directions StartDirection = Directions.East;
        private Directions _direction = Track.StartDirection;
        
        public const int SectionCountUndefined = -1;
        public string Name { get; private set; }

        public LinkedList<Section> Sections { get; private set; }

        private int EastwardSections = Track.SectionCountUndefined;
        private int WestwardSections = Track.SectionCountUndefined;
        private int NorthwardSections = Track.SectionCountUndefined;
        private int SouthwardSections = Track.SectionCountUndefined;

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

        public int GetEastwardSectionsCount()
        {
            if (this.EastwardSections != -1)
            {
                return this.EastwardSections;
            }
            
            this.SimulateTrack();
            return this.EastwardSections;
        }
        
        public int GetSouthwardSectionsCount()
        {
            if (this.SouthwardSections != -1)
            {
                return this.SouthwardSections;
            }
            
            this.SimulateTrack();
            return this.SouthwardSections;
        }

        public int GetWestwardSectionsCount()
        {
            if (this.WestwardSections != -1)
            {
                return this.WestwardSections;
            }
            
            this.SimulateTrack();
            return this.WestwardSections;
        }
        
        public int GetNorthwardSectionsCount()
        {
            if (this.NorthwardSections != -1)
            {
                return this.NorthwardSections;
            }
            
            this.SimulateTrack();
            return this.NorthwardSections;
        }
        
        private void SimulateTrack()
        {
            foreach (Section section in Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.LeftCorner:
                        DrawLeftCorner();
                        break;
                    case SectionTypes.RightCorner:
                        DrawRightCorner();
                        break;
                }

                switch (this._direction)
                {
                    case Directions.East:
                        this.EastwardSections++;
                        break;
                    case Directions.South:
                        this.SouthwardSections++;
                        break;
                    case Directions.West:
                        this.WestwardSections++;
                        break;
                    case Directions.North:
                        this.NorthwardSections++;
                        break;
                }
            }
        }
 
        private void DrawLeftCorner()
        {
            if (this._direction == Directions.East)
            {
                this.EastwardSections++;
                this._direction = Directions.North;
            }
            else if (this._direction == Directions.South)
            {
                this.SouthwardSections++;
                this._direction = Directions.East;
            }
            else if (this._direction == Directions.West)
            {
                this.WestwardSections++;
                this._direction = Directions.South;
            }
            else if (this._direction == Directions.North)
            {
                this.NorthwardSections++;
                this._direction = Directions.West;
            }
        }
                
        private void DrawRightCorner()
        {
            if (this._direction == Directions.East)
            {
                this.EastwardSections++;
                this._direction = Directions.South;
            }
            else if (this._direction == Directions.South)
            {
                this.SouthwardSections++;
                this._direction = Directions.West;
            }
            else if (this._direction == Directions.West)
            {
                this.WestwardSections++;
                this._direction = Directions.North;
            }
            else if (this._direction == Directions.North)
            {
                this.NorthwardSections++;
                this._direction = Directions.East;
            }

        }
        
    }
}
