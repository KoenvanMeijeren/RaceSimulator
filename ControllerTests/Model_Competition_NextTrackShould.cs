using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ControllerTests
{

    [TestFixture]
    public class Model_Competition_NextTrackShould
    {

        private Competition Competition;

        private List<IParticipant> Participants = new List<IParticipant>();
        private Queue<Track> Tracks = new Queue<Track>();

        [SetUp]
        public void Setup()
        {
            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

            this.Participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            this.Participants.Add(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            this.Participants.Add(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));
            this.Participants.Add(new Driver(name: "Piet van Meijeren", points: 192, equipment: toyota, teamColor: TeamColors.Grey));
            this.Participants.Add(new Driver(name: "Stan van Meijeren", points: 197, equipment: defaultCar, teamColor: TeamColors.Yellow));

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

            this.Tracks.Enqueue(new Track(name: "Circuit Zwolle", sections: routeZwolle));
            this.Tracks.Enqueue(new Track(name: "TT Assen", sections: routeElburg));
            this.Tracks.Enqueue(new Track(name: "Monaco", sections: routeAmsterdam));

            this.Competition = new Competition(this.Participants, new Queue<Track>());
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = this.Competition.NextTrack();
            Assert.IsNull(result);
        }

    }
}
