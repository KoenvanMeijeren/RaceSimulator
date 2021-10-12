using System.Collections.Generic;
using Model;

namespace Controller
{
    public static class Data
    {

        private static Competition _competition;
        public static Race CurrentRace { get; private set; }

        public static List<IParticipant> Participants { get; private set; }
        public static Queue<Track> Tracks { get; private set; }

        public static void Initialize()
        {
            // resets the data in order to prevent data errors.
            Data._competition = null;
            Data.CurrentRace = null;
            Data.Participants = new List<IParticipant>();
            Data.Tracks = new Queue<Track>();

            Data.AddTestParticipants();
            Data.AddTestTracks();

            Data._competition = new Competition(Data.Participants, Data.Tracks);

            if (Data.CurrentRace == null)
            {
                Data.NextRace();
            }
        }

        public static void NextRace()
        {
            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.End();
            }

            Track currentTrack = Data._competition.NextTrack();
            if (currentTrack == null)
            {
                Data.CurrentRace = null;
                return;
            }

            Data.CurrentRace = new Race(currentTrack, Data.Participants);
        }

        private static void AddTestParticipants()
        {
            Data.AddParticipant(new Driver(name: "Koen", points: 200, equipment: new Car(quality: 100, performance: 1, speed: 50), teamColor: TeamColors.Red));
            Data.AddParticipant(new Driver(name: "Henk", points: 190, equipment: new Car(quality: 65, performance: 1, speed: 25), teamColor: TeamColors.Blue));
            Data.AddParticipant(new Driver(name: "Jan", points: 195, equipment: new Car(quality: 65, performance: 2, speed: 30), teamColor: TeamColors.Green));
            Data.AddParticipant(new Driver(name: "Piet", points: 192, equipment: new Car(quality: 65, performance: 2, speed: 50), teamColor: TeamColors.Grey));
            Data.AddParticipant(new Driver(name: "Stan", points: 197, equipment: new Car(quality: 65, performance: 2, speed: 25), teamColor: TeamColors.Yellow));
        }

        public static void AddParticipant(Driver driver)
        {
            Data.Participants ??= new List<IParticipant>();

            Data.Participants.Add(driver);
        }

        private static void AddTestTracks()
        {
            SectionTypes[] routeZwolle = {
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish,
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid
            };

            SectionTypes[] routeElburg = {
                SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner,
                SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.StartGrid, SectionTypes.StartGrid
            };

            SectionTypes[] routeAmsterdam =
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner,
                SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid
            };

            SectionTypes[] routeHarderwijk =
            {
                SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, 
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, 
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner, 
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, 
                SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, 
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.StartGrid,
            };
            
            Data.AddTrack(new Track(name: "Circuit Zwolle", sections: routeElburg));
            Data.AddTrack(new Track(name: "TT Assen", sections: routeZwolle));
            Data.AddTrack(new Track(name: "Circuit Harderwijk", sections: routeHarderwijk));
            Data.AddTrack(new Track(name: "Monaco", sections: routeAmsterdam));
        }

        public static void AddTrack(Track track)
        {
            Data.Tracks ??= new Queue<Track>();

            Data.Tracks.Enqueue(track);
        }

    }
}
