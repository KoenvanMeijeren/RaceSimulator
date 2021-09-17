using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public class Race
    {

        public Track Track { get; private set; }

        public List<IParticipant> Participants { get; private set; }

        public DateTime StartTime { get; private set; }

        private Random _random;

        // Only 2 participants per section are allowed.
        private Dictionary<Section, SectionData> Positions;

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            this._random = new Random(DateTime.Now.Millisecond);
            this.Positions = new Dictionary<Section, SectionData>();
        }


        public SectionData GetSectionData(Section section)
        {
            SectionData foundSectionData;
            if (this.Positions == null || this.Positions.Count() < 1 || !this.Positions.TryGetValue(section, out foundSectionData))
            {
                // @todo find out how we can create a new section data object.
                foundSectionData = new SectionData();
                this.Positions.Add(section, foundSectionData);
            }

            return foundSectionData;
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
