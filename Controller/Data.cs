using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
    public static class Data
    {

        public static Competition Competition { get; private set; }


        public static List<IParticipant> Participants { get; private set; }
        public static Queue<Track> Tracks { get; private set; }

        public static void Initialize()
        {
            Data.AddTestParticipants();
            Data.AddTestTracks();

            Data.Competition = new Competition(Data.Participants, Data.Tracks);
        }

        private static void AddTestParticipants()
        {
            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

            Data.AddParticipant(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            Data.AddParticipant(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            Data.AddParticipant(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));
            Data.AddParticipant(new Driver(name: "Piet van Meijeren", points: 192, equipment: toyota, teamColor: TeamColors.Grey));
            Data.AddParticipant(new Driver(name: "Stan van Meijeren", points: 197, equipment: defaultCar, teamColor: TeamColors.Yellow));
        }

        public static void AddParticipant(Driver driver)
        {
            Data.Participants.Add(driver);
        }

        private static void AddTestTracks()
        {
            Section[] route = {
                new Section(SectionTypes.StartGrid),
                new Section(SectionTypes.Straight),
                new Section(SectionTypes.LeftCorner),
                new Section(SectionTypes.Straight),
                new Section(SectionTypes.RightCorner),
                new Section(SectionTypes.LeftCorner),
                new Section(SectionTypes.Straight),
                new Section(SectionTypes.RightCorner),
                new Section(SectionTypes.Straight),
                new Section(SectionTypes.LeftCorner),
                new Section(SectionTypes.Straight),
                new Section(SectionTypes.Straight),
                new Section(SectionTypes.Finish),
            };

            Data.AddTrack(new Track(name: "Zandvoort", sections: route));
            Data.AddTrack(new Track(name: "TT Assen", sections: route));
            Data.AddTrack(new Track(name: "Monaco", sections: route));
        }

        public static void AddTrack(Track track)
        {
            Data.Tracks.Enqueue(track);
        }

    }
}
