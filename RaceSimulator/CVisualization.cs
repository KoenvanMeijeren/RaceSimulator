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

        private static Directions _direction = Directions.East;

        private static readonly int
            _symbolSpaces = 4,
            _cursorStartLeftPosition = 45,
            _cursorStartTopPosition = 5;

        private static int
            _cursorLeftPosition = _cursorStartLeftPosition,
            _cursorTopPosition = _cursorStartTopPosition;

        #region graphics
        private static readonly string[] 
            FinishHorizontal = { "----", "  # ", "  # ", "----" },
            FinishVertical = { "|  |", "|##|", "|  |", "|  |" },

            StartGridHorizontal = { "----", "  | ", "  | ", "----" },
            StartGridVertical = { "|  |", "|--|", "|  |", "|  |" },

            StraightHorizontal = {"----", "    ", "    ", "----" },
            StraightVertical = { "|  |", "|  |", "|  |", "|  |" },

            RightCornerHorizontalRightwardsDownwards = { "|  \\", "|   ", "\\   ", " \\--" },
            RightCornerHorizontalRightwardsUpwards = { "|  \\", "|   ", "\\   ", " \\--" },
            RightCornerVerticalRightwardsDownwards = { " /--", "/   ", "|   ", "|  /" },
            RightCornerVerticalRightwardsUpwards = { "|  /", "|   ", "/   ", " /--" },

            LeftCornerHorizontalLeftwardsUpwards = { " /--", "/   ", "|   ", "|  /" },
            LeftCornerVerticalLeftwardsUpwards = { "|  \\", "|   ", "\\   ", " \\--" },
            LeftCornerHorizontalRightwardsDownwards = { "--\\ ", "   \\", "   |", "\\  |" },
            LeftCornerVerticalRightwardsDownwards = { "/  |", "   |", "   /", "--/ " };
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

            DrawRoundTestTrack();
        }

        private static bool DirectionIsVertical()
        {
            return CVisualization._direction is Directions.South or Directions.North;
        }

        private static bool DirectionIsHorizontal()
        {
            return CVisualization._direction is Directions.West or Directions.East;
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

        private static void DrawLeftCornerDownwardsClockwise()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.DrawLeftCorner(CVisualization.LeftCornerVerticalRightwardsDownwards, true);
                return;
            }

            CVisualization.DrawLeftCorner(CVisualization.LeftCornerHorizontalRightwardsDownwards, true);
        }

        private static void DrawLeftCornerDownwardsCounterClockwise()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.DrawLeftCorner(CVisualization.LeftCornerVerticalRightwardsDownwards, false);
                return;
            }

            CVisualization.DrawLeftCorner(CVisualization.LeftCornerHorizontalRightwardsDownwards, false);
        }

        private static void DrawRightCornerDownwardsClockwise()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.DrawRightCorner(CVisualization.RightCornerVerticalRightwardsDownwards, true);
                return;
            }

            CVisualization.DrawRightCorner(CVisualization.RightCornerHorizontalRightwardsDownwards, true);
        }

        private static void DrawRightCornerDownwardsCounterClockwise()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.DrawRightCorner(CVisualization.RightCornerVerticalRightwardsDownwards, false);
                return;
            }

            CVisualization.DrawRightCorner(CVisualization.RightCornerHorizontalRightwardsDownwards, false);
        }

        private static void DrawRightCornerUpwardsClockwise()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.DrawRightCorner(CVisualization.RightCornerVerticalRightwardsUpwards, true);
                return;
            }

            CVisualization.DrawRightCorner(CVisualization.RightCornerHorizontalRightwardsUpwards, true);
        }

        private static void DrawRightCornerUpwardsCounterClockwise()
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.DrawRightCorner(CVisualization.RightCornerVerticalRightwardsUpwards, false);
                return;
            }

            CVisualization.DrawRightCorner(CVisualization.RightCornerHorizontalRightwardsUpwards, false);
        }

        private static void Draw(string[] symbols)
        {
            if (CVisualization._direction == Directions.East || CVisualization._direction == Directions.West || CVisualization._direction == Directions.South)
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
                    CVisualization._cursorTopPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorLeftPosition += CVisualization._symbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorTopPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorLeftPosition -= CVisualization._symbolSpaces;
                    break;
                case Directions.South:

                    break;
            }

            CVisualization.WriteLine("test");
        }

        private static void DrawLeftCorner(string[] symbols, bool clockwise)
        {
            if (CVisualization._direction == Directions.East || CVisualization._direction == Directions.West)
            {
                DrawDownwards(symbols);
                CVisualization._direction = Directions.South;
            }
            else if (CVisualization._direction == Directions.South)
            {
                DrawDownwards(symbols);
                CVisualization._direction = clockwise ? Directions.West : Directions.East;
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorTopPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorLeftPosition += CVisualization._symbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorTopPosition -= CVisualization._symbolSpaces;
                    CVisualization._cursorLeftPosition -= CVisualization._symbolSpaces;
                    break;
                case Directions.South:
                    // do nothing
                    break;
            }

            CVisualization.WriteLine("test");
        }

        private static void DrawRightCorner(string[] symbols, bool clockwise)
        {
            if (CVisualization._direction == Directions.East || CVisualization._direction == Directions.West)
            {
                DrawDownwards(symbols);
                CVisualization._direction = Directions.North;
            }
            else if (CVisualization._direction == Directions.North)
            {
                DrawUpwards(symbols);
                CVisualization._direction = clockwise ? Directions.East : Directions.West;
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorTopPosition += 1;
                    CVisualization._cursorLeftPosition += CVisualization._symbolSpaces;
                    break;
                case Directions.West:

                    break;
                case Directions.North:
                    CVisualization._cursorTopPosition -= CVisualization._symbolSpaces + 1;
                    break;
                case Directions.South:
                    // do nothing
                    break;
            }

            CVisualization.WriteLine("test");
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
            CVisualization._cursorLeftPosition--;
        }

        private static void MoveCursorRightwards()
        {
            CVisualization._cursorLeftPosition++;
        }

        private static void MoveCursorUpwards()
        {
            CVisualization._cursorTopPosition--;
        }

        private static void MoveCursorDownwards()
        {
            CVisualization._cursorTopPosition++;
        }

        private static void WriteLine(string symbol)
        {
            Console.SetCursorPosition(CVisualization._cursorLeftPosition, CVisualization._cursorTopPosition);
            Console.WriteLine(symbol);
        }

        private static int CenteredTextCursorStartPosition(string text)
        {
            return (Console.WindowWidth / 2) - (text.Length / 2);
        }

        private static void DrawRoundTestTrack()
        {
            CVisualization.DrawStraight();
            CVisualization.DrawStartGrid();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStraight();

            CVisualization.DrawLeftCornerDownwardsClockwise();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStartGrid();

            CVisualization.DrawLeftCornerDownwardsClockwise();
            CVisualization.DrawStraight();
            CVisualization.DrawStartGrid();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStraight();
            CVisualization.DrawStraight();

            CVisualization.DrawRightCornerDownwardsClockwise();
            CVisualization.DrawStraight();
            CVisualization.DrawFinish();
            CVisualization.DrawStartGrid();

            CVisualization.DrawRightCornerUpwardsClockwise();
            CVisualization.DrawStraight();
        }

    }
}
