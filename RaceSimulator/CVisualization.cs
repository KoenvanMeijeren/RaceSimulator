using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;

namespace RaceSimulator
{
    public static class CVisualization
    {

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

            RightCornerHorizontalRightwards = { "|  \\", "|   ", "\\   ", " \\--" },
            RightCornerVerticalRightwards = { " /--", "/   ", "|   ", "|  /" },

            LeftCornerHorizontalRightwards = { "--\\ ", "   \\", "   |", "\\  |" },
            LeftCornerVerticalRightwards = { "/  |", "   /", "  / ", "--  " };
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

            CVisualization.DrawHorizontalRightwards(CVisualization.StraightHorizontal);
            CVisualization.DrawHorizontalRightwards(CVisualization.StartGridHorizontal);
            CVisualization.DrawHorizontalRightwards(CVisualization.StraightHorizontal);
            CVisualization.DrawHorizontalRightwards(CVisualization.LeftCornerHorizontalRightwards);

            CVisualization.MoveCursorToLeftBottomStart();
            CVisualization.DrawVerticalDownwards(CVisualization.StraightVertical);
            CVisualization.DrawVerticalDownwards(CVisualization.LeftCornerVerticalRightwards);

            CVisualization.MoveCursorToLeftTopStart();
            CVisualization.DrawHorizontalLeftwards(CVisualization.StraightHorizontal);
            CVisualization.DrawHorizontalLeftwards(CVisualization.FinishHorizontal);
            CVisualization.DrawHorizontalLeftwards(CVisualization.StraightHorizontal);
            CVisualization.DrawHorizontalLeftwards(CVisualization.RightCornerHorizontalRightwards);

            CVisualization.MoveCursorToRightTopStart(1);
            CVisualization.DrawVerticalUpwards(CVisualization.StraightVertical);

            CVisualization._cursorTopPosition -= 3;
            CVisualization.DrawHorizontalRightwards(CVisualization.RightCornerVerticalRightwards);
        }
        private static void DrawHorizontalLeftwards(string[] symbols)
        {
            CVisualization.DrawHorizontalLeftwards(symbols, 1);
        }

        private static void DrawHorizontalLeftwards(string[] symbols, int repeat)
        {
            for (int delta = 0; delta < repeat; delta++)
            {
                foreach (string symbol in symbols)
                {
                    CVisualization.WriteLine(symbol);
                    CVisualization.MoveCursorDownwards();
                }

                CVisualization.MoveCursorToLeftTopStart();
            }
        }

        private static void DrawHorizontalRightwards(string[] symbols)
        {
            CVisualization.DrawHorizontalRightwards(symbols, 1);
        }

        private static void DrawHorizontalRightwards(string[] symbols, int repeat)
        {
            for (int delta = 0; delta < repeat; delta++)
            {
                foreach (string symbol in symbols)
                {
                    CVisualization.WriteLine(symbol);
                    CVisualization.MoveCursorDownwards();
                }

                CVisualization.MoveCursorToRightTopStart();
            }
        }

        private static void DrawVerticalDownwards(string[] symbols)
        {
            CVisualization.DrawVerticalDownwards(symbols, 1);
        }

        private static void DrawVerticalDownwards(string[] symbols, int repeat)
        {
            for (int delta = 0; delta < repeat; delta++)
            {
                foreach (string symbol in symbols)
                {
                    CVisualization.WriteLine(symbol);
                    CVisualization.MoveCursorDownwards();
                }
            }
        }

        private static void DrawVerticalUpwards(string[] symbols)
        {
            CVisualization.DrawVerticalUpwards(symbols, 1);
        }

        private static void DrawVerticalUpwards(string[] symbols, int repeat)
        {
            for (int delta = 0; delta < repeat; delta++)
            {
                foreach (string symbol in symbols)
                {
                    CVisualization.WriteLine(symbol);
                    CVisualization.MoveCursorUpwards();
                }
            }
        }

        private static void MoveCursorToLeftTopStart()
        {
            CVisualization._cursorTopPosition -= CVisualization._symbolSpaces;
            CVisualization._cursorLeftPosition -= CVisualization._symbolSpaces;
        }

        private static void MoveCursorLeftwards()
        {
            CVisualization._cursorLeftPosition--;
        }

        private static void MoveCursorToRightTopStart()
        {
            CVisualization.MoveCursorToRightTopStart(CVisualization._symbolSpaces);
        }

        private static void MoveCursorToRightTopStart(int newLines)
        {
            CVisualization._cursorTopPosition -= newLines;
            CVisualization._cursorLeftPosition += CVisualization._symbolSpaces;
        }

        private static void MoveCursorRightwards()
        {
            CVisualization._cursorLeftPosition++;
        }

        private static void MoveCursorUpwards()
        {
            CVisualization._cursorTopPosition--;
        }
        private static void MoveCursorToLeftBottomStart()
        {
            CVisualization.MoveCursorToLeftBottomStart(CVisualization._symbolSpaces);
        }

        private static void MoveCursorToLeftBottomStart(int newLines)
        {
            CVisualization._cursorTopPosition += newLines;
            CVisualization._cursorLeftPosition -= CVisualization._symbolSpaces;
        }

        private static void MoveCursorDownwards()
        {
            CVisualization._cursorTopPosition++;
        }

        private static int CenteredTextCursorStartPosition(string text)
        {
            return (Console.WindowWidth / 2) - (text.Length / 2);
        }

        private static void WriteLine(string symbol)
        {
            Console.SetCursorPosition(CVisualization._cursorLeftPosition, CVisualization._cursorTopPosition);
            Console.WriteLine(symbol);
        }
    }
}
