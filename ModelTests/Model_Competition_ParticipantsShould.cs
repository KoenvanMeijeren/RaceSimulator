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
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);

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
            Assert.AreEqual(IEquipment.MaximumQuality, participantOne.Equipment.Quality);
            Assert.AreEqual(IEquipment.MaximumPerformance, participantOne.Equipment.Performance);
            Assert.AreEqual(IEquipment.MaximumSpeed, participantOne.Equipment.Speed);

            Assert.AreEqual("Klaas van Meijeren", participantTwo.Name);
            Assert.AreEqual(190, participantTwo.Points);
            Assert.AreEqual(TeamColors.Blue, participantTwo.TeamColor);
            Assert.AreEqual(IEquipment.MaximumQuality, participantTwo.Equipment.Quality);
            Assert.AreEqual(IEquipment.MaximumPerformance, participantTwo.Equipment.Performance);
            Assert.AreEqual(IEquipment.MaximumSpeed, participantTwo.Equipment.Speed);
        }

        [Test]
        public void CanReadParticipantsInitials()
        {
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);

            IParticipant testParticipant = new Driver(name: "K", points: 200, equipment: defaultCar,
                teamColor: TeamColors.Red);

            Assert.AreEqual("", testParticipant.GetInitials(0));
            Assert.AreEqual("K", testParticipant.GetInitials());
            Assert.AreEqual("K", testParticipant.GetInitials(4));

            Assert.AreEqual("Ko", this._participants.ElementAt(0).GetInitials());
            Assert.AreEqual("Koen", this._participants.ElementAt(0).GetInitials(4));
            Assert.AreEqual("Kl", this._participants.ElementAt(1).GetInitials());
        }

        [Test]
        public void CanReadBrokenParticipantsInitials()
        {
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            defaultCar.IsBroken = true;
            
            IParticipant testParticipant = new Driver(name: "K", points: 200, equipment: defaultCar,
                teamColor: TeamColors.Red);
            
            Assert.IsTrue(defaultCar.IsBroken);
            Assert.AreNotSame("K", testParticipant.GetInitials());
            Assert.AreSame(IParticipant.BrokenInitials, testParticipant.GetInitials());
        }
        
    }
}
