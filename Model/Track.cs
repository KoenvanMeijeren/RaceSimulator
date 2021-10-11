using System;
using System.Collections.Generic;

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

        private const int CursorUndefined = 0;
        public const int SectionCountUndefined = 0;
        public string Name { get; private set; }

        public LinkedList<Section> Sections { get; private set; }

        private int
            _cursorEastPosition = Track.CursorUndefined,
            _cursorNorthPosition = Track.CursorUndefined;

        public int MinEastPosition { get; private set; }
        public int MaxEastPosition { get; private set; }

        public int MinNorthPosition { get; private set; }
        public int MaxNorthPosition { get; private set; }

        public Track(string name, IEnumerable<SectionTypes> sections)
        {
            this.Name = name;
            this.Sections = this.SectionTypeToSections(sections);
            
            this.SimulateTrack();
        }

        private LinkedList<Section> SectionTypeToSections(IEnumerable<SectionTypes> sectionTypes)
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
            return 0;
        }
        
        public int GetSouthwardSectionsCount()
        {
            return 0;
        }

        public int GetWestwardSectionsCount()
        {
            return 0;
        }
        
        public int GetNorthwardSectionsCount()
        {
            return 0;
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
                    case SectionTypes.Straight:
                    case SectionTypes.StartGrid:
                    case SectionTypes.Finish:
                        this.Draw();
                        break;
                }
            }
        }

        private void Draw()
        {
            switch (this._direction)
            {
                case Directions.East:
                    this._cursorEastPosition++;
                    if (this._cursorEastPosition > this.MaxEastPosition)
                    {
                        this.MaxEastPosition = this._cursorEastPosition;
                    }
                    
                    break;
                case Directions.South:
                    this._cursorNorthPosition++;
                    if (this._cursorNorthPosition > this.MaxNorthPosition)
                    {
                        this.MaxNorthPosition = this._cursorNorthPosition;
                    }
                    
                    break;
                case Directions.West:
                    this._cursorEastPosition--;
                    if (this._cursorEastPosition < this.MinEastPosition)
                    {
                        this.MinEastPosition = this._cursorEastPosition;
                    }
                    
                    break;
                case Directions.North:
                    this._cursorNorthPosition--;
                    if (this._cursorNorthPosition < this.MinNorthPosition)
                    {
                        this.MinNorthPosition = this._cursorNorthPosition;
                    }
                    
                    break;
            }
        }
 
        private void DrawLeftCorner()
        {
            switch (this._direction)
            {
                case Directions.East:
                    this._cursorEastPosition++;
                    if (this._cursorEastPosition > this.MaxEastPosition)
                    {
                        this.MaxEastPosition = this._cursorEastPosition;
                    }
                    
                    this._direction = Directions.North;
                    break;
                case Directions.South:
                    this._cursorNorthPosition++;
                    if (this._cursorNorthPosition > this.MaxNorthPosition)
                    {
                        this.MaxNorthPosition = this._cursorNorthPosition;
                    }
                    
                    this._direction = Directions.East;
                    break;
                case Directions.West:
                    this._cursorEastPosition--;
                    if (this._cursorEastPosition < this.MinEastPosition)
                    {
                        this.MinEastPosition = this._cursorEastPosition;
                    }
                    
                    this._direction = Directions.South;
                    break;
                case Directions.North:
                    this._cursorNorthPosition--;
                    if (this._cursorNorthPosition < this.MinNorthPosition)
                    {
                        this.MinNorthPosition = this._cursorNorthPosition;
                    }
                    
                    this._direction = Directions.West;
                    break;
            }
        }
                
        private void DrawRightCorner()
        {
            switch (this._direction)
            {
                case Directions.East:
                    this._cursorEastPosition++;
                    if (this._cursorEastPosition > this.MaxEastPosition)
                    {
                        this.MaxEastPosition = this._cursorEastPosition;
                    }
                    
                    this._direction = Directions.South;
                    break;
                case Directions.South:
                    this._cursorNorthPosition++;
                    if (this._cursorNorthPosition > this.MaxNorthPosition)
                    {
                        this.MaxNorthPosition = this._cursorNorthPosition;
                    }
                    
                    this._direction = Directions.West;
                    break;
                case Directions.West:
                    this._cursorEastPosition--;
                    if (this._cursorEastPosition < this.MinEastPosition)
                    {
                        this.MinEastPosition = this._cursorEastPosition;
                    }
                    
                    this._direction = Directions.North;
                    break;
                case Directions.North:
                    this._cursorNorthPosition--;
                    if (this._cursorNorthPosition < this.MinNorthPosition)
                    {
                        this.MinNorthPosition = this._cursorNorthPosition;
                    }
                    
                    this._direction = Directions.East;
                    break;
            }
        }
        
    }
}
