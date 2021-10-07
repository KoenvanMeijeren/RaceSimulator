using Controller;
using Model;

namespace WPFRaceSimulator
{
    public static class WPFVisualization
    {
        
        private static readonly Directions _startDirection = Directions.East;
        private static Directions _direction = WPFVisualization._startDirection;

        private const int
            CarWidth = 62,
            CarHeight = 40,
            SectionWidth = 150,
            SectionHeight = 150;

        public static int GetTrackWidth(Race race)
        {
            return race.Track.GetEastwardSectionsCount() * WPFVisualization.SectionWidth;
        }

        public static int GetTrackHeight(Race race)
        {
            return race.Track.GetSouthwardSectionsCount() * WPFVisualization.SectionHeight;
        }

    }
}