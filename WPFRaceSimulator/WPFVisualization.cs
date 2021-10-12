using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace WPFRaceSimulator
{
    public static class WPFVisualization
    {
        
        private static Directions _direction = Track.StartDirection;

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
            _trackCursorEastPosition = TrackPositionUndefined,
            _trackCursorNorthPosition = TrackPositionUndefined,
            _cursorEastPosition = CursorEastStartPosition,
            _cursorNorthPosition = CursorNorthStartPosition;

        private static Bitmap _bitmap;

        #region Graphics

        private const string
            BaseGraphicsPath = ".\\Graphics\\",

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
            WPFVisualization._trackWidth = WPFVisualization.TrackPositionUndefined;
            WPFVisualization._trackHeight = WPFVisualization.TrackPositionUndefined;
            
            WPFImageBuilder.ClearCache();
        }

        public static BitmapSource DrawTrack(Race race)
        {
            Track track = race.Track;
            int
                trackWidth = WPFVisualization.GetTrackWidth(track),
                trackHeight = WPFVisualization.GetTrackHeight(track);

            WPFVisualization._direction = Track.StartDirection;
            WPFVisualization._cursorEastPosition = WPFVisualization._trackCursorEastPosition;
            WPFVisualization._cursorNorthPosition = WPFVisualization._trackCursorNorthPosition;

            WPFVisualization._bitmap = WPFImageBuilder.CreateBitmap(trackWidth, trackHeight);

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
            Bitmap bitmap = WPFImageBuilder.LoadImageBitmap(BaseGraphicsPath + Corner, SectionWidth, SectionHeight);

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
            Bitmap bitmap = WPFImageBuilder.LoadImageBitmap(BaseGraphicsPath + Corner, SectionWidth, SectionHeight);

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
            Bitmap bitmap = WPFImageBuilder.LoadImageBitmap(BaseGraphicsPath + graphic, SectionWidth, SectionHeight);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    break;
                case Directions.South:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Directions.West:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                default:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
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
            
            WPFVisualization.DrawCarForSection(graphics, sectionData);
        }

        private static void DrawCarForSection(Graphics graphics, SectionData sectionData)
        {
            if (sectionData.Left != null)
            {
                IParticipant driverLeft = sectionData.Left;

                graphics.DrawImage(LoadCar(driverLeft.TeamColor, driverLeft.Equipment.IsBroken), WPFVisualization.LoadCoordinatesForSection(sectionData, true));
            }
            
            if (sectionData.Right != null)
            {
                IParticipant driverRight = sectionData.Right;

                graphics.DrawImage(LoadCar(driverRight.TeamColor, driverRight.Equipment.IsBroken), WPFVisualization.LoadCoordinatesForSection(sectionData, false));
            }
        }

        private static PointF LoadCoordinatesForSection(SectionData sectionData, bool left)
        {
            int
                distance = left ? sectionData.DistanceLeft : sectionData.DistanceRight,
                cursorEastPosition = WPFVisualization._cursorEastPosition,
                cursorNorthPosition = WPFVisualization._cursorNorthPosition;

            Section section = sectionData.Section;
            int convertedDistance = Race.ConvertRange(0, Race.SectionLength, 0, 4, distance);

            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    cursorNorthPosition += left ? 30 : 45 + CarHeight;
                    cursorEastPosition +=  convertedDistance switch
                    {
                        0 => 15,
                        1 => 50,
                        2 => 70,
                        _ => 90
                    };
                    break;
                case Directions.South:
                    cursorEastPosition += left ? 20 + CarWidth : 25;
                    
                    cursorNorthPosition +=  convertedDistance switch
                    {
                        0 => 15,
                        1 => 50,
                        2 => 70,
                        _ => 90
                    };
                    break;
                case Directions.West:
                    cursorNorthPosition += left ? 45 + CarHeight : 30;
                    if (section?.SectionType is SectionTypes.LeftCorner or SectionTypes.RightCorner)
                    {
                        break;
                    }
                    
                    cursorEastPosition += SectionWidth - CarWidth;
                    
                    cursorEastPosition -=  convertedDistance switch
                    {
                        0 => 15,
                        1 => 50,
                        2 => 70,
                        _ => 90
                    };
                    
                    break;
                case Directions.North:
                    cursorEastPosition += left ? 25 : 20 + CarWidth;
                    if (section?.SectionType is SectionTypes.LeftCorner or SectionTypes.RightCorner)
                    {
                        break;
                    }

                    cursorNorthPosition += SectionHeight - CarHeight - 15;
                    
                    cursorNorthPosition -=  convertedDistance switch
                    {
                        0 => 15,
                        1 => 50,
                        2 => 70,
                        _ => 90
                    };
                    break;
            }
            
            if (section?.SectionType is SectionTypes.LeftCorner or SectionTypes.RightCorner)
            {
                switch (WPFVisualization._direction)
                {
                    case Directions.East:
                    case Directions.West:
                        cursorEastPosition += 50;
                        break;
                    default:
                        cursorNorthPosition += 50;
                        break;
                }
            }

            return new PointF(cursorEastPosition, cursorNorthPosition);
        }
        
        private static Bitmap LoadCar(TeamColors teamColors, bool broken = false)
        {
            string carPath = teamColors switch
            {
                TeamColors.Blue => broken ? CarBlueBroken : CarBlue,
                TeamColors.Grey => broken ? CarGreyBroken : CarGrey,
                TeamColors.Green => broken ? CarGreenBroken : CarGreen,
                TeamColors.Yellow => broken ? CarYellowBroken : CarYellow,
                TeamColors.Red => broken ? CarRedBroken : CarRed,
                _ => throw new ArgumentOutOfRangeException(nameof(teamColors), teamColors, null)
            };

            Bitmap bitmap = WPFImageBuilder.LoadImageBitmap(BaseGraphicsPath + carPath, CarWidth, CarHeight);
            switch (WPFVisualization._direction)
            {
                case Directions.East:
                    break;
                case Directions.South:
                    bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Directions.West:
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Directions.North:
                    bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            
            return bitmap;
        }

        private static int GetTrackWidth(Track track)
        {
            if (WPFVisualization._trackWidth != WPFVisualization.TrackPositionUndefined)
            {
                return WPFVisualization._trackWidth;
            }

            int minEastPosition = track.MinEastPosition, cursorEastPosition = track.MinEastPosition;
            if (minEastPosition < 0)
            {
                minEastPosition *= -1;
                cursorEastPosition = minEastPosition;
            }

            if (cursorEastPosition > 0)
            {
                cursorEastPosition--;
            }

            WPFVisualization._trackWidth = minEastPosition + 1 + track.MaxEastPosition;
            WPFVisualization._trackWidth *= SectionWidth;
            
            WPFVisualization._trackCursorEastPosition = minEastPosition * SectionWidth;

            return WPFVisualization._trackWidth;
        }

        private static int GetTrackHeight(Track track)
        {
            if (WPFVisualization._trackHeight != WPFVisualization.TrackPositionUndefined)
            {
                return WPFVisualization._trackHeight;
            }

            int minNorthPosition = track.MinNorthPosition;
            if (minNorthPosition < 0)
            {
                minNorthPosition *= -1;
            }
            
            WPFVisualization._trackHeight = minNorthPosition + 1 + track.MaxNorthPosition;
            WPFVisualization._trackHeight *= SectionHeight;
            
            WPFVisualization._trackCursorNorthPosition = minNorthPosition * SectionHeight;

            return WPFVisualization._trackHeight;
        }

    }
}