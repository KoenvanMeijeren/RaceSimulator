using System;
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
            CursorEastStartPosition = 0,
            CursorNorthStartPosition = 0,
            CarWidth = 62,
            CarHeight = 40,
            SectionWidth = 150,
            SectionHeight = 150;

        private static int
            _trackWidth = TrackPositionUndefined,
            _trackHeight = TrackPositionUndefined,
            _cursorEastPosition = CursorEastStartPosition,
            _cursorNorthPosition = CursorNorthStartPosition;

        private static Bitmap _bitmap;

        #region Graphics

        private const string
            // @todo find out how to load files the correct way and set the build actions for files.
            BaseGraphicsPath = "..\\..\\..\\Graphics\\",

            CarBlue = "CarBlue.PNG",
            CarBlueBroken = "CarBlueBroken.PNG",
            CarGreen = "CarGreen.PNG",
            CarGreenBroken = "CarGreenBroken.PNG",
            CarYellow = "CarYellow.PNG",
            CarYellowBroken = "CarYellowBroken.PNG",
            CarGrey = "CarGrey.PNG",
            CarGreyBroken = "CarGreyBroken.PNG",
            CarRed = "CarRed.PNG",
            CarRedBroken = "CarRedBroken.PNG",

            Finish = "Finish.PNG",
            StartGrid = "Startgrid.PNG",
            Straight = "Straight.PNG",
            Corner = "Corner.PNG";

        #endregion

        public static void Initialize()
        {
            WPFVisualization._bitmap = null;
            WPFImageBuilder.ClearCache();
        }

        public static BitmapSource DrawTrack(Race race)
        {
            Track track = race.Track;
            int
                trackWidth = WPFVisualization.GetTrackWidth(track),
                trackHeight = WPFVisualization.GetTrackHeight(track);

            // @todo: Optimize calculating the width and height of the bitmap and start positions of the cursor.
            WPFVisualization._cursorEastPosition = trackWidth;
            WPFVisualization._cursorNorthPosition = trackHeight;

            WPFVisualization._bitmap = WPFImageBuilder.CreateBitmap(Convert.ToInt16(trackWidth * 1.6), Convert.ToInt16(trackHeight * 2.5));

            foreach (Section trackSection in track.Sections)
            {
                WPFVisualization.DrawTrackSection(trackSection, race.GetSectionData(trackSection));
            }

            return WPFImageBuilder.CreateBitmapSourceFromGdiBitmap(WPFVisualization._bitmap);
        }

        private static void DrawTrackSection(Section section, SectionData sectionData)
        {
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    DrawImage(StartGrid, sectionData);
                    break;
                case SectionTypes.Straight:
                    DrawImage(Straight, sectionData);
                    break;
                case SectionTypes.LeftCorner:
                    DrawLeftCorner(sectionData);
                    break;
                case SectionTypes.RightCorner:
                    DrawRightCorner(sectionData);
                    break;
                case SectionTypes.Finish:
                    DrawImage(Finish, sectionData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown section type given!");
            }
        }

        private static void DrawLeftCorner(SectionData sectionData = null)
        {
            Bitmap bitmap = WPFImageBuilder.LoadImage(BaseGraphicsPath + Corner, SectionWidth, SectionHeight);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    WPFVisualization._direction = Directions.North;
                    break;
                case Directions.South:
                    WPFVisualization._direction = Directions.East;
                    break;
                case Directions.West:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    WPFVisualization._direction = Directions.South;
                    break;
                case Directions.North:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    WPFVisualization._direction = Directions.West;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_direction");
            }

            WPFVisualization.Draw(bitmap, sectionData);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    WPFVisualization._cursorEastPosition += WPFVisualization.SectionWidth;
                    break;
                case Directions.West:
                    WPFVisualization._cursorEastPosition -= WPFVisualization.SectionWidth;
                    break;
                case Directions.North:
                    WPFVisualization._cursorNorthPosition -= WPFVisualization.SectionHeight;
                    break;
                case Directions.South:
                    WPFVisualization._cursorNorthPosition += WPFVisualization.SectionHeight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_direction");
            }
        }

        private static void DrawRightCorner(SectionData sectionData = null)
        {
            Bitmap bitmap = WPFImageBuilder.LoadImage(BaseGraphicsPath + Corner, SectionWidth, SectionHeight);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    WPFVisualization._direction = Directions.South;
                    break;
                case Directions.South:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    WPFVisualization._direction = Directions.West;
                    break;
                case Directions.West:
                    WPFVisualization._direction = Directions.North;
                    break;
                case Directions.North:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    WPFVisualization._direction = Directions.East;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_direction");
            }

            WPFVisualization.Draw(bitmap, sectionData);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    WPFVisualization._cursorEastPosition += WPFVisualization.SectionWidth;
                    break;
                case Directions.West:
                    WPFVisualization._cursorEastPosition -= WPFVisualization.SectionWidth;
                    break;
                case Directions.North:
                    WPFVisualization._cursorNorthPosition -= WPFVisualization.SectionHeight;
                    break;
                case Directions.South:
                    WPFVisualization._cursorNorthPosition += WPFVisualization.SectionHeight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_direction");
            }
        }

        private static void DrawImage(string graphic, SectionData sectionData)
        {
            Bitmap bitmap = WPFImageBuilder.LoadImage(BaseGraphicsPath + graphic, SectionWidth, SectionHeight);

            if (WPFVisualization.DirectionIsVertical())
            {
                bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            WPFVisualization.Draw(bitmap, sectionData);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    WPFVisualization._cursorEastPosition += WPFVisualization.SectionWidth;
                    break;
                case Directions.South:
                    WPFVisualization._cursorNorthPosition += WPFVisualization.SectionHeight;
                    break;
                case Directions.West:
                    WPFVisualization._cursorEastPosition -= WPFVisualization.SectionWidth;
                    break;
                case Directions.North:
                    WPFVisualization._cursorNorthPosition -= WPFVisualization.SectionHeight;
                    break;
            }
        }

        private static void Draw(Bitmap bitmap, SectionData sectionData)
        {
            Graphics graphics = Graphics.FromImage(WPFVisualization._bitmap);
            graphics.DrawImage(bitmap, new PointF(WPFVisualization._cursorEastPosition, WPFVisualization._cursorNorthPosition));
        }

        private static int GetTrackWidth(Track track)
        {
            if (WPFVisualization._trackWidth != WPFVisualization.TrackPositionUndefined)
            {
                return WPFVisualization._trackWidth;
            }

            int
                eastwardSections = track.GetEastwardSectionsCount() * SectionWidth,
                westwardSections = track.GetWestwardSectionsCount() * SectionWidth;
            
            WPFVisualization._trackWidth = eastwardSections + (westwardSections - eastwardSections);
            
            return WPFVisualization._trackWidth;
        }

        private static int GetTrackHeight(Track track)
        {
            if (WPFVisualization._trackHeight != WPFVisualization.TrackPositionUndefined)
            {
                return WPFVisualization._trackHeight;
            }

            int 
                northwardSections = track.GetNorthwardSectionsCount() * SectionHeight,
                southwardSections = track.GetSouthwardSectionsCount() * SectionHeight;
            
            WPFVisualization._trackHeight = southwardSections + (northwardSections - southwardSections);

            return WPFVisualization._trackHeight;
        }

        private static bool DirectionIsVertical()
        {
            return WPFVisualization._direction is Directions.South or Directions.North;
        }

    }
}