using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Model;

namespace RaceSimulator
{
    public static class CVisualization
    {

        private const Directions StartDirection = Directions.East;
        private static Directions _direction = CVisualization.StartDirection;

        private const int
            TrackPositionUndefined = -1,
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
            MaxInitialsLength = 1,
            InitialsOnPositionOneInGraphicsSymbolsHorizontalIndex = 1,
            InitialsOnPositionTwoInGraphicsSymbolsHorizontalIndex = 2;

        private static readonly int
            CursorStartEastPosition = Console.WindowWidth / 2, 
            CursorStartNorthPosition = Console.WindowHeight / 2;

        private static int
            _trackEastPosition = TrackPositionUndefined,
            _trackNorthPosition = TrackPositionUndefined,
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

        private static void OnDriversChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            CVisualization.DrawTrack(eventArgs.Race);
        }

        public static void DrawTrack(Race race)
        {
            Track track = race.Track;
            int 
                northPosition = CVisualization.GetNorthPosition(track), 
                eastPosition = CVisualization.GetEastPosition(track);
            
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            Console.SetCursorPosition(CenteredTextCursorStartPosition(track.Name),  northPosition < 5 ? 1 :  northPosition - 4);
            Console.WriteLine(track.Name);
            
            CVisualization._direction = CVisualization.StartDirection;
            CVisualization._cursorEastPosition = eastPosition < 20 ? CVisualization.CursorStartEastPosition : eastPosition;
            CVisualization._cursorNorthPosition = northPosition < 10 ? northPosition + 10 : northPosition;

            foreach (Section trackSection in track.Sections)
            {
                CVisualization.DrawTrackSection(trackSection, race.GetSectionData(trackSection));
            }
        }
        
        private static int GetEastPosition(Track track)
        {
            if (CVisualization._trackEastPosition != CVisualization.TrackPositionUndefined)
            {
                return CVisualization._trackEastPosition;
            }

            int
                westwardSections = track.GetWestwardSectionsCount() * SymbolSpaces,
                eastwardSections = track.GetWestwardSectionsCount() * SymbolSpaces;
            
            if (eastwardSections == Track.SectionCountUndefined || westwardSections == Track.SectionCountUndefined)
            {
                return CVisualization.CursorStartEastPosition;
            }
            
            CVisualization._trackEastPosition = eastwardSections + (westwardSections - eastwardSections);
            
            return CVisualization._trackEastPosition;
        }

        private static int GetNorthPosition(Track track)
        {
            if (CVisualization._trackNorthPosition != CVisualization.TrackPositionUndefined)
            {
                return CVisualization._trackNorthPosition;
            }

            int
                southwardSections = track.GetSouthwardSectionsCount() * SymbolSpaces,
                northwardSections = track.GetNorthwardSectionsCount() * SymbolSpaces;
            
            if (northwardSections == Track.SectionCountUndefined || southwardSections == Track.SectionCountUndefined)
            {
                return CVisualization.CursorStartNorthPosition;
            }
            
            CVisualization._trackNorthPosition = southwardSections + (northwardSections - southwardSections);

            return CVisualization._trackNorthPosition;
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
            switch (CVisualization._direction)
            {
                case Directions.East:
                    DrawDownwards(CVisualization.EastCornerToLeft, sectionData);
                    CVisualization._direction = Directions.North;
                    break;
                case Directions.South:
                    DrawDownwards(CVisualization.SouthCornerToLeft, sectionData);
                    CVisualization._direction = Directions.East;
                    break;
                case Directions.West:
                    DrawDownwards(CVisualization.WestCornerToLeft, sectionData);
                    CVisualization._direction = Directions.South;
                    break;
                case Directions.North:
                    DrawUpwards(CVisualization.NorthCornerToLeft, sectionData);
                    CVisualization._direction = Directions.West;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_direction");
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
            switch (CVisualization._direction)
            {
                case Directions.East:
                    DrawDownwards(CVisualization.EastCornerToRight, sectionData);
                    CVisualization._direction = Directions.South;
                    break;
                case Directions.South:
                    DrawDownwards(CVisualization.SouthCornerToRight, sectionData);
                    CVisualization._direction = Directions.West;
                    break;
                case Directions.West:
                    DrawDownwards(CVisualization.WestCornerToRight, sectionData);
                    CVisualization._direction = Directions.North;
                    break;
                case Directions.North:
                    DrawUpwards(CVisualization.NorthCornerToRight, sectionData);
                    CVisualization._direction = Directions.East;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("_direction");
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

            if (CVisualization.SectionIsCorner(sectionData.Section.SectionType))
            {
                return CVisualization.PlaceParticipantsOnCorner(symbols, sectionData);
            }
            
            if (sectionData.Left != null && sectionData.Right != null)
            {
                return CVisualization.PlaceParticipantsOnSection(
                    symbols.ToArray(), 
                    sectionData.Left, sectionData.DistanceLeft, 
                    sectionData.Right, sectionData.DistanceRight
                );
            }

            return CVisualization.PlaceParticipantOnSection(
                symbols.ToArray(), sectionData.Left ?? sectionData.Right, 
                sectionData.Left != null ? sectionData.DistanceLeft : sectionData.DistanceRight,
                sectionData.Left != null
            );
        }

        private static string[] PlaceParticipantsOnCorner(string[] symbols, SectionData sectionData)
        {
            if (sectionData.Left != null && sectionData.Right != null)
            {
                if (sectionData.Section.SectionType == SectionTypes.RightCorner)
                {
                    return CVisualization.PlaceParticipantsOnRightCorner(
                        symbols.ToArray(), 
                        sectionData.Left, sectionData.DistanceLeft, 
                        sectionData.Right, sectionData.DistanceRight
                    );
                }
                
                return CVisualization.PlaceParticipantsOnLeftCorner(
                    symbols.ToArray(), 
                    sectionData.Left, sectionData.DistanceLeft, 
                    sectionData.Right, sectionData.DistanceRight
                );
            }

            if (sectionData.Section.SectionType == SectionTypes.RightCorner)
            {
                return CVisualization.PlaceParticipantOnRightCorner(
                    symbols.ToArray(), sectionData.Left ?? sectionData.Right, 
                    sectionData.Left != null ? sectionData.DistanceLeft : sectionData.DistanceRight
                );
            }
            
            return CVisualization.PlaceParticipantOnLeftCorner(
                symbols.ToArray(), sectionData.Left ?? sectionData.Right, 
                sectionData.Left != null ? sectionData.DistanceLeft : sectionData.DistanceRight
            );
        }

        private static string[] PlaceParticipantsOnLeftCorner(string[] symbols,IParticipant participantLeft, int distanceLeft, IParticipant participantRight, int distanceRight)
        {
            int
                indexOne = CVisualization.ConvertIndexForLeftCorner(distanceLeft, CVisualization._direction == Directions.East),
                indexTwo = CVisualization.ConvertIndexForLeftCorner(distanceRight, CVisualization._direction != Directions.East),
                distanceOne = CVisualization.ConvertDistanceForLeftCorner(distanceLeft, CVisualization._direction == Directions.East),
                distanceTwo = CVisualization.ConvertDistanceForLeftCorner(distanceRight, CVisualization._direction != Directions.East);

            string
                initialsOne = participantLeft.GetInitials(CVisualization.MaxInitialsLength),
                initialsTwo = participantRight.GetInitials(CVisualization.MaxInitialsLength);

            symbols[indexOne] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[indexOne], initialsOne, distanceOne);
            symbols[indexTwo] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[indexTwo], initialsTwo, distanceTwo);
            
            return symbols;
        }

        private static string[] PlaceParticipantsOnRightCorner(string[] symbols,IParticipant participantLeft, int distanceLeft, IParticipant participantRight, int distanceRight)
        {
            int
                indexOne = CVisualization.ConvertIndexForRightCorner(distanceLeft, CVisualization._direction == Directions.West),
                indexTwo = CVisualization.ConvertIndexForRightCorner(distanceRight, CVisualization._direction != Directions.West),
                distanceOne = CVisualization.ConvertDistanceForRightCorner(distanceLeft, CVisualization._direction == Directions.West),
                distanceTwo = CVisualization.ConvertDistanceForRightCorner(distanceRight, CVisualization._direction != Directions.West);

            string
                initialsOne = participantLeft.GetInitials(CVisualization.MaxInitialsLength),
                initialsTwo = participantRight.GetInitials(CVisualization.MaxInitialsLength);

            symbols[indexOne] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[indexOne], initialsOne, distanceOne);
            symbols[indexTwo] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[indexTwo], initialsTwo, distanceTwo);
            
            return symbols;
        }

        private static string[] PlaceParticipantOnLeftCorner(string[] symbols, IParticipant participant, int distance)
        {
            int index = CVisualization.ConvertIndexForLeftCorner(distance, CVisualization._direction == Directions.East),
                distanceOne = CVisualization.ConvertDistanceForLeftCorner(distance, CVisualization._direction == Directions.East);

            symbols[index] = CVisualization.MergeInitialsIntoSymbolByIndex(
                symbols[index], participant.GetInitials(CVisualization.MaxInitialsLength), distanceOne
            );

            return symbols;
        }

        private static string[] PlaceParticipantOnRightCorner(string[] symbols, IParticipant participant, int distance)
        {
            int index = CVisualization.ConvertIndexForRightCorner(distance, CVisualization._direction == Directions.West),
                distanceOne = CVisualization.ConvertDistanceForRightCorner(distance, CVisualization._direction == Directions.West);

            symbols[index] = CVisualization.MergeInitialsIntoSymbolByIndex(
                symbols[index], participant.GetInitials(CVisualization.MaxInitialsLength), distanceOne
            );

            return symbols;
        }
        
        private static string[] PlaceParticipantsOnSection(string[] symbols, IParticipant participantOne, int distanceOne, IParticipant participantTwo, int distanceTwo)
        {
            if (CVisualization.DirectionIsVertical())
            {
                return CVisualization.PlaceParticipantsOnVerticalSection(symbols, participantOne, distanceOne, participantTwo, distanceTwo);
            }
            
            return CVisualization.PlaceParticipantsOnHorizontalSection(symbols, participantOne, distanceOne, participantTwo, distanceTwo);
        }
        
        private static string[] PlaceParticipantOnSection(string[] symbols, IParticipant participant, int distance, bool left)
        {
            if (CVisualization.DirectionIsVertical())
            {
                return CVisualization.PlaceParticipantOnVerticalSection(symbols, participant, distance, left);
            }
            
            return CVisualization.PlaceParticipantOnHorizontalSection(symbols, participant, distance);
        }

        private static string[] PlaceParticipantsOnVerticalSection(string[] symbols, IParticipant participantOne, int distanceOne, IParticipant participantTwo, int distanceTwo)
        {
            int indexOne = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distanceOne);
            int indexTwo = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distanceTwo);
            if (indexOne > 3)
            {
                indexOne = 3;
            }

            if (indexTwo > 3)
            {
                indexTwo = 3;
            }
            
            string
                initialsStartOne = participantOne.GetInitials(CVisualization.MaxInitialsLength),
                initialsStartTwo = participantTwo.GetInitials(CVisualization.MaxInitialsLength),
                initialsOne = initialsStartOne,
                initialsTwo = initialsStartTwo;
            
            symbols[indexOne] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[indexOne], initialsOne, 2);
            symbols[indexTwo] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[indexTwo], initialsTwo, 1);
            
            return symbols;
        }

        private static string[] PlaceParticipantsOnHorizontalSection(string[] symbols, IParticipant participantOne, int distanceOne, IParticipant participantTwo, int distanceTwo)
        {
            int indexOne = CVisualization.InitialsOnPositionOneInGraphicsSymbolsHorizontalIndex,
                indexTwo = CVisualization.InitialsOnPositionTwoInGraphicsSymbolsHorizontalIndex;
            string
                initialsStartOne = participantOne.GetInitials(CVisualization.MaxInitialsLength),
                initialsStartTwo = participantTwo.GetInitials(CVisualization.MaxInitialsLength),
                initialsOne = initialsStartOne,
                initialsTwo = initialsStartTwo;

            symbols[indexOne] = CVisualization.MergeInitialsIntoSymbol(symbols[indexOne], initialsOne, distanceOne);
            symbols[indexTwo] = CVisualization.MergeInitialsIntoSymbol(symbols[indexTwo], initialsTwo, distanceTwo);

            return symbols;
        }
        
        private static string[] PlaceParticipantOnVerticalSection(string[] symbols, IParticipant participant, int distance, bool left)
        {
            int index = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distance);
            if (index > 3)
            {
                index = 3;
            }

            string initials = participant.GetInitials(CVisualization.MaxInitialsLength);
            
            symbols[index] = CVisualization.MergeInitialsIntoSymbolByIndex(symbols[index], initials, left ? 2 : 1);
            
            return symbols;
        }

        private static string[] PlaceParticipantOnHorizontalSection(string[] symbols, IParticipant participant, int distance)
        {
            int index = CVisualization.InitialsOnPositionOneInGraphicsSymbolsHorizontalIndex;

            symbols[index] = CVisualization.MergeInitialsIntoSymbol(
                symbols[index], participant.GetInitials(CVisualization.MaxInitialsLength), distance
            );

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

        private static string MergeInitialsIntoSymbol(string symbol, string initials, int distance)
        { 
            int initialsStartIndex = CVisualization.ConvertDistanceToSymbolIndex(distance);

            return CVisualization.MergeInitialsIntoSymbolByIndex(symbol, initials, initialsStartIndex);
        }

        private static string MergeInitialsIntoSymbolByIndex(string symbol, string initials, int index)
        {
            int maxInitialsLength = initials.Length < CVisualization.MaxInitialsLength ? initials.Length : CVisualization.MaxInitialsLength;
            int startIndex = CVisualization.FlipSymbolIndexIfNecessary(index);

            string trimmedInitials = initials.ToString();
            if (initials.Length > CVisualization.MaxInitialsLength)
            {
                trimmedInitials = trimmedInitials.Remove(CVisualization.MaxInitialsLength);
            }

            if ((trimmedInitials.Length + startIndex) > symbol.Length)
            {
                return symbol;
            }

            return symbol.Remove(startIndex, maxInitialsLength).Insert(startIndex, trimmedInitials);
        }

        private static int ConvertDistanceToSymbolIndex(int distance)
        {
            return Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distance);
        }

        private static int FlipSymbolIndexIfNecessary(int distance)
        {
            if (CVisualization._direction == Directions.West)
            {
                distance = Race.ConvertRange(0, CVisualization.SymbolSpaces, CVisualization.SymbolSpaces, 0, distance);
            }

            return distance;
        }
        
        private static int CenteredTextCursorStartPosition(string text)
        {
            return (Console.WindowWidth / 2) - (text.Length / 2);
        }

        private static bool SectionIsCorner(SectionTypes sectionType)
        {
            return sectionType is SectionTypes.LeftCorner or SectionTypes.RightCorner;
        }
        
        private static int ConvertIndexForRightCorner(int distance, bool right)
        {
            int newDistance = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distance);
            return newDistance switch
            {
                0 => CVisualization._direction switch
                {
                    Directions.East => right ? 2 : 1,
                    Directions.South => 0,
                    Directions.North => 0,
                    Directions.West => right ? 1 : 2,
                    _ => 2
                },
                1 => CVisualization._direction switch
                {
                    Directions.East => right ? 2 : 1,
                    Directions.South => 1,
                    Directions.North => 1,
                    Directions.West => right ? 1 : 2,
                    _ => 2
                },
                2 => CVisualization._direction switch
                {
                    Directions.South => right ? 1 : 2,
                    Directions.North => right ? 1 : 2,
                    Directions.West => 1,
                    _ => 2
                },
                _ => CVisualization._direction switch
                {
                    Directions.South => right ? 1 : 2,
                    Directions.North => right ? 1 : 2,
                    Directions.West => 0,
                    _ => 3
                }
            };
        }
        
        private static int ConvertDistanceForRightCorner(int distance, bool right)
        {
            int newDistance = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distance);
            return newDistance switch
            {
                0 => CVisualization._direction switch
                {
                    Directions.East => 0,
                    Directions.South => right ? 2 : 1,
                    Directions.North => right ? 2 : 1,
                    _ => 1
                },
                1 => CVisualization._direction switch
                {
                    Directions.East => 1,
                    Directions.South => right ? 2 : 1,
                    Directions.North => right ? 2 : 1,
                    _ => 2
                },
                2 => CVisualization._direction switch
                {
                    Directions.East => right ? 1 : 2,
                    Directions.South => 1,
                    Directions.West => right ? 2 : 3,
                    _ => 2
                },
                _ => CVisualization._direction switch
                {
                    Directions.East => right ? 1 : 2,
                    Directions.South => 0,
                    Directions.West => right ? 2 : 3,
                    _ => 3
                }
            };
        }

        private static int ConvertIndexForLeftCorner(int distance, bool left)
        {
            int newDistance = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distance);
            return newDistance switch
            {
                0 => CVisualization._direction switch
                {
                    Directions.East => left ? 1 : 2,
                    Directions.West => left ? 2 : 1,
                    _ => 0
                },
                1 => CVisualization._direction switch
                {
                    Directions.East => left ? 1 : 2,
                    Directions.West => left ? 2 : 1,
                    _ => 1
                },
                2 => CVisualization._direction switch
                {
                    Directions.East => 1,
                    Directions.South => left ? 1 : 2,
                    Directions.North => left ? 2 : 1,
                    _ => 2
                },
                _ => CVisualization._direction switch
                {
                    Directions.East => 0,
                    Directions.South => left ? 1 : 2,
                    Directions.North => left ? 2 : 1,
                    _ => 3
                }
            };
        }
        
        private static int ConvertDistanceForLeftCorner(int distance, bool left)
        {
            int newDistance = Race.ConvertRange(0, Race.SectionLength, 0, CVisualization.SymbolSpaces, distance);
            return newDistance switch
            {
                0 => CVisualization._direction switch
                {
                    Directions.East => 0,
                    Directions.South => left ? 2 : 1,
                    Directions.North => left ? 2 : 1,
                    _ => 1
                },
                1 => CVisualization._direction switch
                {
                    Directions.East => 1,
                    Directions.South => left ? 2 : 1,
                    Directions.North => left ? 2 : 1,
                    _ => 2
                },
                2 => CVisualization._direction switch
                {
                    Directions.East => left ? 1 : 2,
                    Directions.West => left ? 2 : 3,
                    Directions.North => 1,
                    _ => 2
                },
                _ => CVisualization._direction switch
                {
                    Directions.East => left ? 1 : 2,
                    Directions.West => left ? 2 : 3,
                    Directions.North => 0,
                    _ => 3
                }
            };
        }
        
    }
}
