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

        public const int 
            StartDistanceOfParticipant = 0, 
            TimerInterval = 500, 
            SectionLength = 100;

        public Track Track { get; private set; }

        public List<IParticipant> Participants { get; private set; }

        public DateTime StartTime { get; private set; }

        private Random _random;

        // Only 2 participants per section are allowed.
        private Dictionary<Section, SectionData> _positions;

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

        private void MoveParticipants()
        {
            Section[] sections = this.Track.Sections.ToArray();
            foreach (Section section in sections)
            {
                SectionData sectionData = this.GetSectionData(section);
                if (sectionData.Left != null && this.CanMoveParticipant(sectionData.DistanceLeft))
                {
                    sectionData.MoveLeft();
                }

                if (sectionData.Right != null && this.CanMoveParticipant(sectionData.DistanceRight))
                {
                    sectionData.MoveRight();
                }
                
                this.UpdateSectionData(section, sectionData);
            }
            
            this.MoveParticipantsToNextSectionIfNecessary();
        }

        // @todo find out why this method causes a stack overflow when participants are going to overtake their positions.
        private void MoveParticipantsToNextSectionIfNecessary()
        {
            Section[] sections = this.Track.Sections.ToArray();
            
            // Determine how many participants should move to the next section. Than move them, if they can move, and
            // re-trigger this method. We do this because it is not always possible to move the participants to the next
            // section. For example if there are participants on the last 2 sections, and they both should move, then we 
            // cannot move them both, because the second last one cannot move to the next section because those
            // participants weren't move to the next section yet. After the first loop all non-blocking participants
            // were moved to the next section. Now we can move all blocking participants to next section. We repeat this
            // until there are no more participants who should move.
            int sectionsWhichShouldMove = 0;
            foreach (Section section in sections)
            {
                SectionData sectionData = this.GetSectionData(section);
                if (this.ShouldMoveParticipantToNextSection(sectionData))
                {
                    sectionsWhichShouldMove++;
                }
            }

            if (sectionsWhichShouldMove <= 0)
            {
                return;
            }
            
            for (int delta = 0; delta < sections.Length; delta++)
            {
                int nextSectionDelta = (delta + 1) >= sections.Length ? 0 : delta + 1;

                Section
                    section = sections[delta],
                    nextSection = sections[nextSectionDelta];
                SectionData
                    sectionData = this.GetSectionData(section),
                    nextSectionData = this.GetSectionData(nextSection);
                
                if (this.ShouldMoveParticipantsToNextSection(sectionData))
                {
                    nextSectionData = this.MoveParticipantsToNextSection(
                        sectionData, nextSectionData, sectionData.Left, sectionData.Right
                    );
                    
                    this.UpdateSectionData(nextSection, nextSectionData);
                    this.UpdateSectionData(section, sectionData);
                }
                else if (this.ShouldMoveParticipantToNextSection(sectionData))
                {
                    IParticipant participant = this.GetParticipantWhoShouldMoveToNextSection(sectionData);
                
                    nextSectionData = this.MoveParticipantToNextSection(
                        sectionData, nextSectionData, participant
                    );
                    
                    this.UpdateSectionData(nextSection, nextSectionData);
                    this.UpdateSectionData(section, sectionData);
                }
            }

            this.MoveParticipantsToNextSectionIfNecessary();
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

        private SectionData MoveParticipantToNextSection(SectionData sectionData, SectionData nextSectionData, IParticipant participant)
        {
            if (!this.CanPlaceParticipant(nextSectionData, participant))
            {
                return nextSectionData;
            }
            
            nextSectionData = this.ParticipantToSectionData(nextSectionData, participant);
            sectionData.Clear(participant);

            return nextSectionData;
        }

        private SectionData MoveParticipantsToNextSection(SectionData sectionData, SectionData nextSectionData, IParticipant participantLeft, IParticipant participantRight)
        {
            if (!this.CanPlaceParticipants(nextSectionData, participantLeft, participantRight))
            {
                return nextSectionData;
            }
            
            nextSectionData = this.ParticipantsToSectionData(nextSectionData, participantLeft, participantRight);
            sectionData.Clear(participantLeft, participantRight);

            return nextSectionData;
        }

        // @todo find out what we should do when there are more participants than start grids.
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
                    sectionData = this.ParticipantToSectionData(sectionData, participantOne);
                    participants.RemoveAt(0);
                }
                else if (canPlaceBoth)
                {
                    sectionData = this.ParticipantsToSectionData(sectionData, participantOne, participantTwo);
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

        private SectionData ParticipantsToSectionData(SectionData sectionData, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            if (!this.CanPlaceParticipants(sectionData, leftParticipant, rightParticipant))
            {
                return null;
            }

            int defaultDistance = Race.StartDistanceOfParticipant;

            return new SectionData(leftParticipant, defaultDistance, rightParticipant, defaultDistance);
        }

        private SectionData ParticipantToSectionData(SectionData sectionData, IParticipant participant)
        {
            int defaultDistance = Race.StartDistanceOfParticipant;

            if (this.CanPlaceLeftParticipant(sectionData))
            {
                return new SectionData(participant, defaultDistance, sectionData.Right, defaultDistance);
            }

            if (this.CanPlaceRightParticipant(sectionData))
            {
                return new SectionData(sectionData.Left, defaultDistance, participant, defaultDistance);
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
