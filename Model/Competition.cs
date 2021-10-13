using System.Collections.Generic;

namespace Model
{
    public class Competition
    {

        public List<IParticipant> Participants { get; private set; }

        public Queue<Track> Tracks { get; private set; }

        public Competition(List<IParticipant> participants, Queue<Track> tracks)
        {
            this.Participants = participants;
            this.Tracks = tracks;
        }

        public Track NextTrack()
        {
            if (this.Tracks == null || this.Tracks.Count < 1)
            {
                return null;
            }

            return this.Tracks.Dequeue();
        }

    }
}
