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

        private int _eastwardSections = Track.SectionCountUndefined;
        private int _westwardSections = Track.SectionCountUndefined;
        private int _northwardSections = Track.SectionCountUndefined;
        private int _southwardSections = Track.SectionCountUndefined;

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
            if (this._eastwardSections != -1)
            {
                return this._eastwardSections;
            }
            
            this.SimulateTrack();
            return this._eastwardSections;
        }
        
        public int GetSouthwardSectionsCount()
        {
            if (this._southwardSections != -1)
            {
                return this._southwardSections;
            }
            
            this.SimulateTrack();
            return this._southwardSections;
        }

        public int GetWestwardSectionsCount()
        {
            if (this._westwardSections != -1)
            {
                return this._westwardSections;
            }
            
            this.SimulateTrack();
            return this._westwardSections;
        }
        
        public int GetNorthwardSectionsCount()
        {
            if (this._northwardSections != -1)
            {
                return this._northwardSections;
            }
            
            this.SimulateTrack();
            return this._northwardSections;
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
                        this._eastwardSections++;
                        break;
                    case Directions.South:
                        this._southwardSections++;
                        break;
                    case Directions.West:
                        this._westwardSections++;
                        break;
                    case Directions.North:
                        this._northwardSections++;
                        break;
                }
            }
        }
 
        private void DrawLeftCorner()
        {
            if (this._direction == Directions.East)
            {
                this._eastwardSections++;
                this._direction = Directions.North;
            }
            else if (this._direction == Directions.South)
            {
                this._southwardSections++;
                this._direction = Directions.East;
            }
            else if (this._direction == Directions.West)
            {
                this._westwardSections++;
                this._direction = Directions.South;
            }
            else if (this._direction == Directions.North)
            {
                this._northwardSections++;
                this._direction = Directions.West;
            }
        }
                
        private void DrawRightCorner()
        {
            if (this._direction == Directions.East)
            {
                this._eastwardSections++;
                this._direction = Directions.South;
            }
            else if (this._direction == Directions.South)
            {
                this._southwardSections++;
                this._direction = Directions.West;
            }
            else if (this._direction == Directions.West)
            {
                this._westwardSections++;
                this._direction = Directions.North;
            }
            else if (this._direction == Directions.North)
            {
                this._northwardSections++;
                this._direction = Directions.East;
            }
        }
        
    }
}
