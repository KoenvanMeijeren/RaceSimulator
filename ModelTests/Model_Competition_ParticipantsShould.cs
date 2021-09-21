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

        private List<IParticipant> _participants = new List<IParticipant>();

        [SetUp]
        public void Setup()
        {
            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

            this._participants = new List<IParticipant>();
            this._participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            this._participants.Add(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            this._participants.Add(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));
            this._participants.Add(new Driver(name: "Piet van Meijeren", points: 192, equipment: toyota, teamColor: TeamColors.Grey));
            this._participants.Add(new Driver(name: "Stan van Meijeren", points: 197, equipment: defaultCar, teamColor: TeamColors.Yellow));

            this._emptyCompetition = new Competition(new List<IParticipant>(), new Queue<Track>());
            this._competition = new Competition(this._participants, new Queue<Track>());
        }

        [Test]
        public void EmptyParticipants()
        {
            Assert.AreEqual(0, this._emptyCompetition.Participants.Count);
        }

        [Test]
        public void NotEmptyParticipants()
        {
            Assert.AreEqual(5, this._competition.Participants.Count);

            IParticipant participantOne = this._participants[0];
            IParticipant participantTwo = this._participants[1];


            Assert.AreEqual("Koen van Meijeren", participantOne.Name);
            Assert.AreEqual(200, participantOne.Points);
            Assert.AreEqual(TeamColors.Red, participantOne.TeamColor);
            Assert.AreEqual(100, participantOne.Equipment.Quality);
            Assert.AreEqual(150, participantOne.Equipment.Performance);
            Assert.AreEqual(25, participantOne.Equipment.Speed);

            Assert.AreEqual("Klaas van Meijeren", participantTwo.Name);
            Assert.AreEqual(190, participantTwo.Points);
            Assert.AreEqual(TeamColors.Blue, participantTwo.TeamColor);
            Assert.AreEqual(65, participantTwo.Equipment.Quality);
            Assert.AreEqual(34, participantTwo.Equipment.Performance);
            Assert.AreEqual(10, participantTwo.Equipment.Speed);
        }

    }
}
