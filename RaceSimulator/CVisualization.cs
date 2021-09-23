using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;

namespace RaceSimulator
{

    public enum Directions
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public static class CVisualization
    {

        private static readonly Directions _start_direction = Directions.East;
        private static Directions _direction = CVisualization._start_direction;

        private static readonly int
            _symbolSpaces = 4,
            _cursorStartEastPosition = 55,
            _cursorStartNorthPosition = 15;

        private static int
            _cursorEastPosition = _cursorStartEastPosition,
            _cursorNorthPosition = _cursorStartNorthPosition;

        #region graphics

        private static readonly string[]
            FinishHorizontal = {"----", "  # ", "  # ", "----"},
            FinishVertical = {"|  |", "|##|", "|  |", "|  |"},

            StartGridHorizontal = {"----", "  | ", "  | ", "----"},
            StartGridVertical = {"|  |", "|--|", "|  |", "|  |"},

            StraightHorizontal = {"----", "    ", "    ", "----"},
            StraightVertical = {"|  |", "|  |", "|  |", "|  |"},

            NorthCornerToRight = {"|  /", "|   ", "/   ", " /--"},
            NorthCornerToLeft = {"\\  |", "   |", "   \\", "--\\ "},

            EastCornerToRight = {"--\\ ", "   \\", "   |", "\\  |"},
            EastCornerToLeft = {"/  |", "   |", "   /", "--/ "},

            SouthCornerToRight = CVisualization.EastCornerToLeft,
            SouthCornerToLeft = {"|  \\", "|   ", "\\   ", " \\--"},

            WestCornerToRight = CVisualization.SouthCornerToLeft,
            WestCornerToLeft = {" /--", "/   ", "|   ", "|  /"};
        #endregion

        public static void Initialize()
        {
            
        }

        public static void DrawTrack(Track track)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.SetCursorPosition(CenteredTextCursorStartPosition(track.Name), 1);
            Console.WriteLine(track.Name);

            foreach (Section trackSection in track.Sections)
            {
                CVisualization.DrawTrackSection(trackSection);
            }
        }

        private static void DrawTrackSection(Section section)
        {
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    DrawStartGrid();
                    break;
                case SectionTypes.Straight:
                    DrawStraight();
                    break;
                case SectionTypes.LeftCorner:
                    DrawLeftCorner();
                    break;
                case SectionTypes.RightCorner:
                    DrawRightCorner();
                    break;
                case SectionTypes.Finish:
                    DrawFinish();
                    break;
            }
        }

        private static bool DirectionIsVertical()
        {
            return CVisualization._direction is Directions.South or Directions.North;
        }

        private static bool DirectionIsHorizontal()
        {
            return CVisualization._direction is Directions.West or Directions.East;
        }

        private static void DrawLeftCorner()
        {
            if (CVisualization._direction == Directions.East)
            {
                DrawDownwards(CVisualization.EastCornerToLeft);
                CVisualization._direction = Directions.North;
            }
            else if (CVisualization._direction == Directions.South)
            {
                DrawDownwards(CVisualization.SouthCornerToLeft);
                CVisualization._direction = Directions.East;
            }
            else if (CVisualization._direction == Directions.West)
            {
                DrawDownwards(CVisualization.WestCornerToLeft);
                CVisualization._direction = Directions.South;
            }
            else if (CVisualization._direction == Directions.North)
            {
                DrawUpwards(CVisualization.NorthCornerToLeft);
                CVisualization._direction = Directions.West;
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorNorthPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorEastPosition += CVisualization._symbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorNorthPosition += 1;
                    CVisualization._cursorEastPosition -= CVisualization._symbolSpaces;
                    break;
                case Directions.North:
                    CVisualization._cursorNorthPosition -= CVisualization._symbolSpaces + 1;
                    break;
                case Directions.South:
                    // do nothing
                    break;
            }
        }

        private static void DrawRightCorner()
        {
            if (CVisualization._direction == Directions.East)
            {
                DrawDownwards(CVisualization.EastCornerToRight);
                CVisualization._direction = Directions.South;
            }
            else if (CVisualization._direction == Directions.South)
            {
                DrawDownwards(CVisualization.SouthCornerToRight);
                CVisualization._direction = Directions.West;
            }
            else if (CVisualization._direction == Directions.West)
            {
                DrawDownwards(CVisualization.WestCornerToRight);
                CVisualization._direction = Directions.North;
            }
            else if (CVisualization._direction == Directions.North)
            {
                DrawUpwards(CVisualization.NorthCornerToRight);
                CVisualization._direction = Directions.East;
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorNorthPosition += 1;
                    CVisualization._cursorEastPosition += CVisualization._symbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorNorthPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorEastPosition -= CVisualization._symbolSpaces;
                    break;
                case Directions.North:
                    CVisualization._cursorNorthPosition -= CVisualization._symbolSpaces + 1;
                    break;
                case Directions.South:
                    // do nothing
                    break;
            }
        }

        private static void DrawStraight()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.Draw(CVisualization.StraightVertical);
                return;
            }

            CVisualization.Draw(CVisualization.StraightHorizontal);
        }

        private static void DrawStartGrid()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.Draw(CVisualization.StartGridVertical);
                return;
            }

            CVisualization.Draw(CVisualization.StartGridHorizontal);
        }

        private static void DrawFinish()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.Draw(CVisualization.FinishVertical);
                return;
            }

            CVisualization.Draw(CVisualization.FinishHorizontal);
        }

        private static void Draw(string[] symbols)
        {
            if (CVisualization._direction is Directions.East or Directions.West or Directions.South)
            {
                DrawDownwards(symbols);
            }
            else
            {
                DrawUpwards(symbols);
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorNorthPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorEastPosition += CVisualization._symbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorNorthPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorEastPosition -= CVisualization._symbolSpaces;
                    break;
            }
        }

        private static void DrawDownwards(string[] symbols)
        {
            foreach (string symbol in symbols)
            {
                CVisualization.WriteLine(symbol);
                CVisualization.MoveCursorDownwards();
            }
        }

        private static void DrawUpwards(string[] symbols)
        {
            foreach (string symbol in symbols)
            {
                CVisualization.WriteLine(symbol);
                CVisualization.MoveCursorUpwards();
            }
        }

        private static void MoveCursorLeftwards()
        {
            CVisualization._cursorEastPosition--;
        }

        private static void MoveCursorRightwards()
        {
            CVisualization._cursorEastPosition++;
        }

        private static void MoveCursorUpwards()
        {
            CVisualization._cursorNorthPosition--;
        }

        private static void MoveCursorDownwards()
        {
            CVisualization._cursorNorthPosition++;
        }

        private static void WriteLine(string symbol)
        {
            Console.SetCursorPosition(CVisualization._cursorEastPosition, CVisualization._cursorNorthPosition);
            Console.WriteLine(symbol);
        }

        private static int CenteredTextCursorStartPosition(string text)
        {
            return (Console.WindowWidth / 2) - (text.Length / 2);
        }

        private static void DrawTestTracks()
        {
            DrawRoundTestTrackClockwise();
            _cursorEastPosition += 50;
            DrawRoundTestTrackCounterClockwise();
            _cursorNorthPosition += 30;
            DrawHalfRoundTestTracks();
        }

        private static void DrawHalfRoundTestTracks()
        {
            DrawStartGrid();
            DrawStraight();
            DrawRightCorner();
            DrawStraight();
            DrawLeftCorner();
            DrawStraight();
            DrawFinish();

            _cursorNorthPosition += 15;

            _direction = Directions.West;

            DrawStartGrid();
            DrawStraight();
            DrawLeftCorner();
            DrawStraight();
            DrawRightCorner();
            DrawStraight();
            DrawFinish();
        }

        private static void DrawRoundTestTrackCounterClockwise()
        {
            CVisualization._direction = Directions.West;

            CVisualization.DrawStraight();
            CVisualization.DrawStartGrid();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStraight();

            CVisualization.DrawLeftCorner();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStartGrid();

            CVisualization.DrawLeftCorner();
            CVisualization.DrawStraight();
            CVisualization.DrawStartGrid();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStraight();
            CVisualization.DrawStraight();

            CVisualization.DrawLeftCorner();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStartGrid();

            CVisualization.DrawLeftCorner();
            CVisualization.DrawStraight();
        }

        private static void DrawRoundTestTrackClockwise()
        {
            CVisualization._direction = Directions.East;

            CVisualization.DrawStraight();
            CVisualization.DrawStartGrid();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStraight();

            CVisualization.DrawRightCorner();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStartGrid();

            CVisualization.DrawRightCorner();
            CVisualization.DrawStraight();
            CVisualization.DrawStartGrid();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStraight();
            CVisualization.DrawStraight();

            CVisualization.DrawRightCorner();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStartGrid();

            CVisualization.DrawRightCorner();
            CVisualization.DrawStraight();
        }

    }
}
