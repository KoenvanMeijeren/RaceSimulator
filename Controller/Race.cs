using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Model;

namespace Controller
{

    public class Race
    {

        private const int
            StartDistanceOfParticipant = 0,
            TimerInterval = 500,
            RoundsStartValue = 0,
            MaxRounds = 2;
        
        public const int SectionLength = IEquipment.MaximumPerformance * IEquipment.MaximumSpeed;

        public Track Track { get; private set; }

        public List<IParticipant> Participants { get; private set; }

        public DateTime StartTime { get; private set; }

        private Random _random;

        // Only 2 participants per section are allowed.
        private Dictionary<Section, SectionData> _positions;

        public Dictionary<IParticipant, int> Rounds { get; private set; }

        private readonly Timer _timer;

        public static event EventHandler<DriversChangedEventArgs> DriversChanged;

        private static Race _raceReference;

        private static bool _changedDrivers;

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            this._random = new Random(DateTime.Now.Millisecond);
            this._positions = new Dictionary<Section, SectionData>();
            this.Rounds = new Dictionary<IParticipant, int>(participants.Capacity);
            this._timer = new Timer(Race.TimerInterval);
            this._timer.Elapsed += Race.OnTimedEvent;
            
            this.PlaceParticipantsOnStartPositions();
        }

        public void Start()
        {
            this.StartTime = DateTime.Now;
            this._timer.Enabled = true;
            Race._raceReference = this;
        }

        public static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Race._raceReference.MoveParticipants();
            if (!Race._changedDrivers)
            {
                return;
            }

            Race.DriversChanged?.Invoke(source, new DriversChangedEventArgs(Race._raceReference));
            Race._changedDrivers = false;
        }

        public SectionData GetSectionData(Section section)
        {
            if (!this._positions.Any() || !this._positions.TryGetValue(section, out var foundSectionData))
            {
                foundSectionData = new SectionData();
                this._positions.Add(section, foundSectionData);
            }

            return foundSectionData;
        }

        public bool UpdateSectionData(Section section, SectionData sectionData)
        {
            if (!this._positions.ContainsKey(section))
            {
                return false;
            }

            this._positions[section] = sectionData;
            Race._changedDrivers = true;
            return true;
        }
        
        public int GetRounds(IParticipant participant)
        {
            if (!this.Rounds.Any() || !this.Rounds.TryGetValue(participant, out var rounds))
            {
                rounds = Race.RoundsStartValue;
                this.Rounds.Add(participant, rounds);
            }

            return rounds;
        }

        public bool UpdateRounds(IParticipant participant, int rounds)
        {
            if (!this.Rounds.ContainsKey(participant))
            {
                return false;
            }

            this.Rounds[participant] = rounds;
            return true;
        }

        private void MoveParticipants()
        {
            Section[] sections = this.Track.Sections.ToArray();
            for (int delta = 0; delta < sections.Length; delta++)
            {
                int nextSectionDelta = (delta + 1) >= sections.Length ? 0 : delta + 1;

                Section
                    section = sections[delta],
                    nextSection = sections[nextSectionDelta];
                SectionData
                    sectionData = this.GetSectionData(section),
                    nextSectionData = this.GetSectionData(nextSection);
                
                if (sectionData.Left != null && this.CanMoveParticipant(sectionData.DistanceLeft))
                {
                    sectionData.MoveLeft();
                    this.UpdateSectionData(section, sectionData);
                }

                if (sectionData.Right != null && this.CanMoveParticipant(sectionData.DistanceRight))
                {
                    sectionData.MoveRight();
                    this.UpdateSectionData(section, sectionData);
                }
                
                this.MoveParticipantsToNextSectionIfNecessary(section, sectionData, nextSection, nextSectionData);
            }
        }

        private void MoveParticipantsToNextSectionIfNecessary(Section section, SectionData sectionData, Section nextSection, SectionData nextSectionData)
        {
            if (this.ShouldMoveParticipantsToNextSection(sectionData))
            {
                nextSectionData = this.MoveParticipantsToNextSection(
                    sectionData, nextSection, nextSectionData, sectionData.Left, sectionData.Right
                );

                if (section.SectionType == SectionTypes.Finish)
                {
                    int roundsLeft = this.GetRounds(nextSectionData.Left);
                    this.UpdateRounds(nextSectionData.Left, roundsLeft + 1);
                
                    int roundsRight = this.GetRounds(nextSectionData.Right);
                    this.UpdateRounds(nextSectionData.Right, roundsRight + 1);

                    nextSectionData = this.RemoveParticipantsOnTrackCompletion(nextSectionData, nextSectionData.Left, roundsLeft);
                    nextSectionData = this.RemoveParticipantsOnTrackCompletion(nextSectionData, nextSectionData.Right, roundsRight);
                }

                this.UpdateSectionData(nextSection, nextSectionData);
                this.UpdateSectionData(section, sectionData);
            }
            else if (this.ShouldMoveParticipantToNextSection(sectionData))
            {
                IParticipant participant = this.GetParticipantWhoShouldMoveToNextSection(sectionData);
                
                nextSectionData = this.MoveParticipantToNextSection(
                    sectionData, nextSection, nextSectionData, participant
                );

                if (section.SectionType == SectionTypes.Finish)
                {
                    int rounds = this.GetRounds(participant);
                    this.UpdateRounds(participant, rounds + 1);
                    
                    nextSectionData = this.RemoveParticipantsOnTrackCompletion(nextSectionData, participant, rounds);
                }

                this.UpdateSectionData(nextSection, nextSectionData);
                this.UpdateSectionData(section, sectionData);
            }
        }
        
        private SectionData RemoveParticipantsOnTrackCompletion(SectionData sectionData, IParticipant participant, int rounds)
        {
            if (rounds >= Race.MaxRounds)
            {
                sectionData.Clear(participant);
            }
            
            return sectionData;
        }

        private bool CanMoveParticipant(int distance)
        {
            return distance < Race.SectionLength;
        }
        
        private bool ShouldMoveParticipantToNextSection(SectionData sectionData)
        {
            return (sectionData.DistanceLeft >= Race.SectionLength && sectionData.Left != null) 
                   || (sectionData.DistanceRight >= Race.SectionLength && sectionData.Right != null);
        }

        private bool ShouldMoveParticipantsToNextSection(SectionData sectionData)
        {
            return sectionData.DistanceLeft >= Race.SectionLength && sectionData.Left != null
                   && sectionData.DistanceRight >= Race.SectionLength && sectionData.Right != null;
        }

        private IParticipant GetParticipantWhoShouldMoveToNextSection(SectionData sectionData)
        {
            if (!this.ShouldMoveParticipantToNextSection(sectionData))
            {
                return null;
            }

            return sectionData.DistanceLeft >= Race.SectionLength ? sectionData.Left : sectionData.Right;
        }

        private SectionData MoveParticipantToNextSection(SectionData sectionData, Section nextSection, SectionData nextSectionData, IParticipant participant)
        {
            if (!this.CanPlaceParticipant(nextSectionData, participant))
            {
                return nextSectionData;
            }
            
            nextSectionData = this.ParticipantToSectionData(nextSection, nextSectionData, participant);
            sectionData.Clear(participant);

            return nextSectionData;
        }

        private SectionData MoveParticipantsToNextSection(SectionData sectionData, Section nextSection, SectionData nextSectionData, IParticipant participantLeft, IParticipant participantRight)
        {
            if (!this.CanPlaceParticipants(nextSectionData, participantLeft, participantRight))
            {
                return nextSectionData;
            }
            
            nextSectionData = this.ParticipantsToSectionData(nextSection, nextSectionData, participantLeft, participantRight);
            sectionData.Clear(participantLeft, participantRight);

            return nextSectionData;
        }

        private void PlaceParticipantsOnStartPositions()
        {
            List<IParticipant> participants = this.Participants.ToList();
            Section[] sections = this.Track.Sections.ToArray();

            for (int delta = 0; delta < sections.Length; delta++)
            {
                Section section = sections[delta];
                SectionData sectionData = this.GetSectionData(section);

                if (!this.CanPlaceParticipantsOnStartPosition(section.SectionType))
                {
                    continue;
                }

                IParticipant participantOne = participants.ElementAtOrDefault(0);
                IParticipant participantTwo = participants.ElementAtOrDefault(1);
                bool 
                    canPlaceBoth = this.CanPlaceParticipants(sectionData, participantOne, participantTwo),
                    canPlaceOne = this.CanPlaceParticipant(sectionData, participantOne);

                if (!canPlaceBoth && canPlaceOne)
                {
                    sectionData = this.ParticipantToSectionData(section, sectionData, participantOne);
                    participants.RemoveAt(0);
                }
                else if (canPlaceBoth)
                {
                    sectionData = this.ParticipantsToSectionData(section, sectionData, participantOne, participantTwo);
                    participants.RemoveAt(0);
                    participants.RemoveAt(0);
                }

                this.UpdateSectionData(section, sectionData);
            }
        }

        private bool CanPlaceParticipantsOnStartPosition(SectionTypes sectionType)
        {
            return sectionType == SectionTypes.StartGrid;
        }

        private bool CanPlaceParticipants(SectionData sectionData, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            if (leftParticipant == null || rightParticipant == null)
            {
                return false;
            }

            return sectionData.Left == null && sectionData.Right == null;
        }

        private bool CanPlaceParticipant(SectionData sectionData, IParticipant participant)
        {
            if (participant == null)
            {
                return false;
            }

            return this.CanPlaceLeftParticipant(sectionData) || this.CanPlaceRightParticipant(sectionData);
        }

        private bool CanPlaceLeftParticipant(SectionData sectionData)
        {
            return sectionData.Left == null && (sectionData.Right != null || sectionData.Right == null);
        }

        private bool CanPlaceRightParticipant(SectionData sectionData)
        {
            return sectionData.Right == null && (sectionData.Left != null || sectionData.Left == null);
        }

        private SectionData ParticipantsToSectionData(Section section, SectionData sectionData, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            if (!this.CanPlaceParticipants(sectionData, leftParticipant, rightParticipant))
            {
                return null;
            }

            int defaultDistance = Race.StartDistanceOfParticipant;

            return new SectionData(section, leftParticipant, defaultDistance, rightParticipant, defaultDistance);
        }

        private SectionData ParticipantToSectionData(Section section, SectionData sectionData, IParticipant participant)
        {
            int defaultDistance = Race.StartDistanceOfParticipant;

            if (this.CanPlaceLeftParticipant(sectionData))
            {
                return new SectionData(section, participant, defaultDistance, sectionData.Right, defaultDistance);
            }

            if (this.CanPlaceRightParticipant(sectionData))
            {
                return new SectionData(section, sectionData.Left, defaultDistance, participant, defaultDistance);
            }

            return null;
        }

        private void RandomizeEquipment()
        {
            foreach (IParticipant participant in this.Participants)
            {
                IEquipment equipment = participant.Equipment;
                equipment.SetRandomPerformance();
                equipment.SetRandomQuality();
            }
        }

        public static int ConvertRange(int originalStart, int originalEnd, int newStart, int newEnd, int value)
        {
            double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
            
            return (int)(newStart + ((value - originalStart) * scale));
        }
        
    }
}
