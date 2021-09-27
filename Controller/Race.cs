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

        private const int StartDistanceBetweenParticipants = 0, Interval = 500;

        public Track Track { get; private set; }

        public List<IParticipant> Participants { get; private set; }

        public DateTime StartTime { get; private set; }

        private Random _random;

        // Only 2 participants per section are allowed.
        private Dictionary<Section, SectionData> _positions;

        private Timer _timer;

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            this._random = new Random(DateTime.Now.Millisecond);
            this._positions = new Dictionary<Section, SectionData>();
            this._timer = new Timer(Race.Interval);

            // Renders the participants to a new list in order to prevent changing the participants permanently.
            this.PlaceParticipantsOnTrack(track, participants.ToList());

            this._timer.Elapsed += Race.OnTimedEvent;
        }

        public void Start()
        {
            this.StartTime = DateTime.Now;
            this._timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                e.SignalTime);
        }

        public SectionData GetSectionData(Section section)
        {
            if (this._positions == null || !this._positions.Any() || !this._positions.TryGetValue(section, out var foundSectionData))
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
            return true;
        }

        // @todo find out what we should do when there are more participants than start grids.
        private void PlaceParticipantsOnTrack(Track track, List<IParticipant> participants)
        {
            for (int delta = 0; delta < this.Track.Sections.Count; delta++)
            {
                Section section = this.Track.Sections.ToArray()[delta];
                SectionData sectionData = this.GetSectionData(section);

                if (!this.CanPlaceParticipantsOnSectionType(section.SectionType))
                {
                    this.GetSectionData(section);
                    continue;
                }

                IParticipant participantOne = participants.ElementAtOrDefault(0);
                IParticipant participantTwo = participants.ElementAtOrDefault(1);
                if (participantOne == null && participantTwo == null)
                {
                    continue;
                }

                bool canPlaceBoth =
                        this.CanPlaceBothParticipants(sectionData, participantOne, participantTwo),
                    canPlaceOne = this.CanPlaceOneParticipant(sectionData, participantOne);

                if (!canPlaceBoth && canPlaceOne)
                {
                    sectionData = this.OneParticipantToSectionData(sectionData, participantOne);
                    participants.RemoveAt(0);
                }
                else if (canPlaceBoth)
                {
                    sectionData = this.BothParticipantsToSectionData(sectionData, participantOne, participantTwo);
                    participants.RemoveAt(0);
                    participants.RemoveAt(0);
                }

                this.UpdateSectionData(section, sectionData);
            }
        }

        private bool CanPlaceParticipantsOnSectionType(SectionTypes sectionType)
        {
            return sectionType == SectionTypes.StartGrid;
        }

        private bool CanPlaceBothParticipants(SectionData sectionData, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            if (leftParticipant == null || rightParticipant == null)
            {
                return false;
            }

            return sectionData.Left == null && sectionData.Right == null;
        }

        private bool CanPlaceOneParticipant(SectionData sectionData, IParticipant participant)
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
            return sectionData.Right != null && (sectionData.Left != null || sectionData.Left == null);
        }

        // @todo implement distance between participants calculation.
        private SectionData BothParticipantsToSectionData(SectionData sectionData, IParticipant leftParticipant, IParticipant rightParticipant)
        {
            if (!this.CanPlaceBothParticipants(sectionData, leftParticipant, rightParticipant))
            {
                return null;
            }

            int defaultDistance = Race.StartDistanceBetweenParticipants;

            return new SectionData(leftParticipant, defaultDistance, rightParticipant, defaultDistance);
        }

        private SectionData OneParticipantToSectionData(SectionData sectionData, IParticipant participant)
        {
            int defaultDistance = Race.StartDistanceBetweenParticipants;

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

    }
}
