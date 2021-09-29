using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controller;
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

        private static readonly Directions _startDirection = Directions.East;
        private static Directions _direction = CVisualization._startDirection;

        private const int
            SymbolSpaces = 4,

            /*
             * The first property determines the max length of the initials and trims the initials
             * to the selected length. The second property determines the index of the initials
             * inside a string of the graphics strings.
             *
             * If we have the string: "----", "  | ", "  | ", "----" for example, the initials
             * will be placed on the selected position. The third and fourth property determines
             * which string of the symbols we have to take. 
             *
             * If the value of the first property is 0, and of the second 1 and of the third 2,
             * the output will be this: "----", "AB| ", "CD| ", "----" or "----", "A | ", "BC| ", "----"
             * or "----", "AB| ", "  | ", "----".
             */
            MaxInitialsLength = 2,
            InitialsInGraphicsSymbolHorizontalStartIndex = 0,
            InitialsOnPositionOneInGraphicsSymbolsHorizontalIndex = 1,
            InitialsOnPositionTwoInGraphicsSymbolsHorizontalIndex = 2,
            InitialsInGraphicsSymbolVerticalStartIndex = 1,
            InitialsOnPositionOneInGraphicsSymbolsVerticalIndex = 2,
            InitialsOnPositionTwoInGraphicsSymbolsVerticalIndex = 3;

        private static readonly int
            CursorStartEastPosition = Console.WindowWidth / 2, 
            CursorStartNorthPosition = Console.WindowHeight / 2;

        private static int
            _cursorEastPosition = CursorStartEastPosition,
            _cursorNorthPosition = CursorStartNorthPosition;

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
            Race.DriversChanged += CVisualization.OnDriversChanged;
        }

        public static void OnDriversChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            CVisualization.DrawTrack(eventArgs.Race);
        }

        public static void DrawTrack(Race race)
        {
            Track track = race.Track;

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.SetCursorPosition(CenteredTextCursorStartPosition(track.Name), 1);
            Console.WriteLine(track.Name);

            CVisualization._cursorEastPosition = CVisualization.CursorStartEastPosition;
            if (track.EastStartPosition != Track.StartPositionUndefined)
            {
                CVisualization._cursorEastPosition = track.EastStartPosition;
            }

            CVisualization._cursorNorthPosition = CVisualization.CursorStartNorthPosition;
            if (track.NorthStartPosition != Track.StartPositionUndefined)
            {
                CVisualization._cursorNorthPosition = track.NorthStartPosition;
            }

            foreach (Section trackSection in track.Sections)
            {
                CVisualization.DrawTrackSection(trackSection, race.GetSectionData(trackSection));
            }
        }

        private static void DrawTrackSection(Section section, SectionData sectionData)
        {
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    DrawStartGrid(sectionData);
                    break;
                case SectionTypes.Straight:
                    DrawStraight(sectionData);
                    break;
                case SectionTypes.LeftCorner:
                    DrawLeftCorner(sectionData);
                    break;
                case SectionTypes.RightCorner:
                    DrawRightCorner(sectionData);
                    break;
                case SectionTypes.Finish:
                    DrawFinish(sectionData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown section type given!");
            }
        }

        private static bool DirectionIsVertical()
        {
            return CVisualization._direction is Directions.South or Directions.North;
        }

        private static void DrawLeftCorner(SectionData sectionData = null)
        {
            if (CVisualization._direction == Directions.East)
            {
                DrawDownwards(CVisualization.EastCornerToLeft, sectionData);
                CVisualization._direction = Directions.North;
            }
            else if (CVisualization._direction == Directions.South)
            {
                DrawDownwards(CVisualization.SouthCornerToLeft, sectionData);
                CVisualization._direction = Directions.East;
            }
            else if (CVisualization._direction == Directions.West)
            {
                DrawDownwards(CVisualization.WestCornerToLeft, sectionData);
                CVisualization._direction = Directions.South;
            }
            else if (CVisualization._direction == Directions.North)
            {
                DrawUpwards(CVisualization.NorthCornerToLeft, sectionData);
                CVisualization._direction = Directions.West;
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorNorthPosition -= CVisualization.SymbolSpaces;
                    CVisualization._cursorEastPosition += CVisualization.SymbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorNorthPosition += 1;
                    CVisualization._cursorEastPosition -= CVisualization.SymbolSpaces;
                    break;
                case Directions.North:
                    CVisualization._cursorNorthPosition -= CVisualization.SymbolSpaces + 1;
                    break;
                case Directions.South:
                    // do nothing
                    break;
            }
        }

        private static void DrawRightCorner(SectionData sectionData = null)
        {
            if (CVisualization._direction == Directions.East)
            {
                DrawDownwards(CVisualization.EastCornerToRight, sectionData);
                CVisualization._direction = Directions.South;
            }
            else if (CVisualization._direction == Directions.South)
            {
                DrawDownwards(CVisualization.SouthCornerToRight, sectionData);
                CVisualization._direction = Directions.West;
            }
            else if (CVisualization._direction == Directions.West)
            {
                DrawDownwards(CVisualization.WestCornerToRight, sectionData);
                CVisualization._direction = Directions.North;
            }
            else if (CVisualization._direction == Directions.North)
            {
                DrawUpwards(CVisualization.NorthCornerToRight, sectionData);
                CVisualization._direction = Directions.East;
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorNorthPosition += 1;
                    CVisualization._cursorEastPosition += CVisualization.SymbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorNorthPosition -= CVisualization.SymbolSpaces;
                    CVisualization._cursorEastPosition -= CVisualization.SymbolSpaces;
                    break;
                case Directions.North:
                    CVisualization._cursorNorthPosition -= CVisualization.SymbolSpaces + 1;
                    break;
                case Directions.South:
                    // do nothing
                    break;
            }
        }

        private static void DrawStraight(SectionData sectionData = null)
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.Draw(CVisualization.StraightVertical, sectionData);
                return;
            }

            CVisualization.Draw(CVisualization.StraightHorizontal, sectionData);
        }

        private static void DrawStartGrid(SectionData sectionData = null)
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.Draw(CVisualization.StartGridVertical, sectionData);
                return;
            }

            CVisualization.Draw(CVisualization.StartGridHorizontal, sectionData);
        }

        private static void DrawFinish(SectionData sectionData = null)
        {
            if (CVisualization.DirectionIsVertical())
            {
                CVisualization.Draw(CVisualization.FinishVertical, sectionData);
                return;
            }

            CVisualization.Draw(CVisualization.FinishHorizontal, sectionData);
        }

        private static void Draw(string[] symbols, SectionData sectionData = null)
        {
            if (CVisualization._direction is Directions.East or Directions.West or Directions.South)
            {
                DrawDownwards(symbols, sectionData);
            }
            else
            {
                DrawUpwards(symbols, sectionData);
            }

            switch (CVisualization._direction)
            {
                case Directions.East:
                    CVisualization._cursorNorthPosition -= CVisualization.SymbolSpaces;
                    CVisualization._cursorEastPosition += CVisualization.SymbolSpaces;
                    break;
                case Directions.West:
                    CVisualization._cursorNorthPosition -= CVisualization.SymbolSpaces;
                    CVisualization._cursorEastPosition -= CVisualization.SymbolSpaces;
                    break;
            }
        }

        private static void DrawDownwards(string[] symbols, SectionData sectionData = null)
        {
            string[] writeSymbols = CVisualization.SectionDataToSymbols(symbols, sectionData);

            foreach (string symbol in writeSymbols)
            {
                CVisualization.WriteLine(symbol);
                CVisualization.MoveCursorDownwards();
            }
        }

        private static void DrawUpwards(string[] symbols, SectionData sectionData = null)
        {
            string[] writeSymbols = CVisualization.SectionDataToSymbols(symbols, sectionData);

            foreach (string symbol in writeSymbols)
            {
                CVisualization.WriteLine(symbol);
                CVisualization.MoveCursorUpwards();
            }
        }

        private static string[] SectionDataToSymbols(string[] symbols, SectionData sectionData = null)
        {
            if (sectionData == null || (sectionData.Left == null && sectionData.Right == null))
            {
                return symbols;
            }

            if (sectionData.Left != null && sectionData.Right != null)
            {
                return CVisualization.PlaceBothParticipantsOnSection(symbols.ToArray(), sectionData.Left, sectionData.Right);
            }
            
            if (sectionData.Left != null || sectionData.Right != null)
            {
                return CVisualization.PlaceOneParticipantOnSection(symbols.ToArray(), sectionData.Left ?? sectionData.Right);
            }

            return symbols;
        }

        private static string[] PlaceBothParticipantsOnSection(string[] symbols, IParticipant participantOne, IParticipant participantTwo)
        {
            int indexOne = CVisualization.InitialsOnPositionOneInGraphicsSymbolsHorizontalIndex,
                indexTwo = CVisualization.InitialsOnPositionTwoInGraphicsSymbolsHorizontalIndex,
                initialsStartIndex = CVisualization.InitialsInGraphicsSymbolHorizontalStartIndex;
            string
                initialsStartOne = participantOne.GetInitials(CVisualization.MaxInitialsLength),
                initialsStartTwo = participantTwo.GetInitials(CVisualization.MaxInitialsLength),
                initialsOne = initialsStartOne,
                initialsTwo = initialsStartTwo;


            if (CVisualization.DirectionIsVertical())
            {
                indexOne = CVisualization.InitialsOnPositionOneInGraphicsSymbolsVerticalIndex;
                indexTwo = CVisualization.InitialsOnPositionTwoInGraphicsSymbolsVerticalIndex;
                initialsStartIndex = CVisualization.InitialsInGraphicsSymbolVerticalStartIndex;
                initialsOne = initialsStartOne.First().ToString() + initialsStartTwo.First().ToString();
                initialsTwo = initialsStartOne?.ElementAtOrDefault(1).ToString() +
                              initialsStartTwo?.ElementAtOrDefault(1).ToString();
            }

            symbols[indexOne] = CVisualization.MergeInitialsIntoSymbol(symbols[indexOne], initialsOne, initialsStartIndex);
            symbols[indexTwo] = CVisualization.MergeInitialsIntoSymbol(symbols[indexTwo], initialsTwo, initialsStartIndex);

            return symbols;
        }

        private static string[] PlaceOneParticipantOnSection(string[] symbols, IParticipant participant)
        {
            int indexOne = CVisualization.InitialsOnPositionOneInGraphicsSymbolsHorizontalIndex;

            symbols[indexOne] = CVisualization.MergeInitialsIntoSymbol(symbols[indexOne], participant.GetInitials(CVisualization.MaxInitialsLength));

            return symbols;
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

        private static string MergeInitialsIntoSymbol(string symbol, string initials, int initialsStartIndex = CVisualization.InitialsInGraphicsSymbolHorizontalStartIndex)
        { 
            int maxInitialsLength = initials.Length < CVisualization.MaxInitialsLength ? initials.Length : CVisualization.MaxInitialsLength;

            string trimmedInitials = initials.ToString();
            if (initials.Length > CVisualization.MaxInitialsLength)
            {
                trimmedInitials = trimmedInitials.Remove(CVisualization.MaxInitialsLength);
            }

            return symbol.Remove(initialsStartIndex, maxInitialsLength).Insert(initialsStartIndex, trimmedInitials);
        }

        private static int CenteredTextCursorStartPosition(string text)
        {
            return (Console.WindowWidth / 2) - (text.Length / 2);
        }
    }
}
