using Controller;
using Model;

namespace WPFRaceSimulator
{
    public static class WPFVisualization
    {
        
        private static readonly Directions _startDirection = Directions.East;
        private static Directions _direction = WPFVisualization._startDirection;

        private const int
            TrackPositionUndefined = -1,
            CarWidth = 62,
            CarHeight = 40,
            SectionWidth = 150,
            SectionHeight = 150;

        private static int
            _trackWidth = TrackPositionUndefined,
            _trackHeight = TrackPositionUndefined;

        public static int GetTrackWidth(Race race)
        {
            if (WPFVisualization._trackWidth != TrackPositionUndefined)
            {
                return WPFVisualization._trackWidth;
            }

            int
                eastwardSections = race.Track.GetEastwardSectionsCount() * SectionWidth,
                westwardSections = race.Track.GetWestwardSectionsCount() * SectionWidth;
            
            WPFVisualization._trackWidth = eastwardSections + (westwardSections - eastwardSections);
            
            return WPFVisualization._trackWidth;
        }

        public static int GetTrackHeight(Race race)
        {
            if (WPFVisualization._trackHeight != TrackPositionUndefined)
            {
                return WPFVisualization._trackHeight;
            }

            int 
                northwardSections = race.Track.GetNorthwardSectionsCount() * SectionHeight,
                southwardSections = race.Track.GetSouthwardSectionsCount() * SectionHeight;
            
            WPFVisualization._trackHeight = southwardSections + (northwardSections - southwardSections);

            return WPFVisualization._trackHeight;
        }

    }
}