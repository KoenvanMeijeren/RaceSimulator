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
    public class Model_Competition_ParticipantsShould
    {

        private Competition _emptyCompetition;
        private Competition _competition;

        private List<IParticipant> Participants = new List<IParticipant>();

        [SetUp]
        public void Setup()
        {
            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

            this.Participants = new List<IParticipant>();
            this.Participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            this.Participants.Add(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            this.Participants.Add(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));
            this.Participants.Add(new Driver(name: "Piet van Meijeren", points: 192, equipment: toyota, teamColor: TeamColors.Grey));
            this.Participants.Add(new Driver(name: "Stan van Meijeren", points: 197, equipment: defaultCar, teamColor: TeamColors.Yellow));

            this._emptyCompetition = new Competition(new List<IParticipant>(), new Queue<Track>());
            this._competition = new Competition(this.Participants, new Queue<Track>());
        }

        [Test]
        public void NextTrack_EmptyParticipants()
        {
            Assert.AreEqual(0, this._emptyCompetition.Participants.Count);
        }

        [Test]
        public void NextTrack_NotEmptyParticipants()
        {
            Assert.AreEqual(5, this._competition.Participants.Count);

            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

            Driver driverOne = new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red);
            Driver driverTwo = new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue);
            IParticipant participantOne = this.Participants[0];
            IParticipant participantTwo = this.Participants[1];


            Assert.AreEqual(driverOne.Name, participantOne.Name);
            Assert.AreEqual(driverOne.Points, participantOne.Points);
            Assert.AreEqual(driverOne.TeamColor, participantOne.TeamColor);

            Assert.AreEqual(driverTwo.Name, participantTwo.Name);
            Assert.AreEqual(driverTwo.Points, participantTwo.Points);
            Assert.AreEqual(driverTwo.TeamColor, participantTwo.TeamColor);
        }

    }
}
