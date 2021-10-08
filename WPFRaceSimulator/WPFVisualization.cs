using System.Drawing;
using System.Windows.Media.Imaging;
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

        public static void Initialize()
        {
            
        }

        public static BitmapSource DrawTrack(Race race)
        {
            Track track = race.Track;
            int
                trackWidth = WPFVisualization.GetTrackWidth(track),
                trackHeight = WPFVisualization.GetTrackHeight(track);

            Bitmap bitmap = WPFImageBuilder.CreateBitmap(trackWidth, trackHeight);

            return WPFImageBuilder.CreateBitmapSourceFromGdiBitmap(bitmap);
        }

        public static int GetTrackWidth(Track track)
        {
            if (WPFVisualization._trackWidth != TrackPositionUndefined)
            {
                return WPFVisualization._trackWidth;
            }

            int
                eastwardSections = track.GetEastwardSectionsCount() * SectionWidth,
                westwardSections = track.GetWestwardSectionsCount() * SectionWidth;
            
            WPFVisualization._trackWidth = eastwardSections + (westwardSections - eastwardSections);
            
            return WPFVisualization._trackWidth;
        }

        public static int GetTrackHeight(Track track)
        {
            if (WPFVisualization._trackHeight != TrackPositionUndefined)
            {
                return WPFVisualization._trackHeight;
            }

            int 
                northwardSections = track.GetNorthwardSectionsCount() * SectionHeight,
                southwardSections = track.GetSouthwardSectionsCount() * SectionHeight;
            
            WPFVisualization._trackHeight = southwardSections + (northwardSections - southwardSections);

            return WPFVisualization._trackHeight;
        }

    }
}