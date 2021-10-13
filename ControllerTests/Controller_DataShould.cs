using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTests
{

    [TestFixture]
    public class Controller_DataShould
    {

        [SetUp]
        public void Setup()
        {
            Data.Initialize();
        }

        [Test]
        public void Data_Initialize_Participants_CanRead()
        {
            Data.Initialize();

            Assert.IsNotEmpty(Data.Participants);
            Assert.AreEqual(5, Data.Participants.Count);
            Assert.AreEqual("Koen", Data.Participants.First().Name);
            Assert.AreEqual("Henk", Data.Participants[1].Name);
            Assert.AreEqual("Jan", Data.Participants[2].Name);
            Assert.AreEqual("Piet", Data.Participants[3].Name);
            Assert.AreEqual("Stan", Data.Participants[4].Name);
        }

        [Test]
        public void Data_Participants_CanAdd()
        {
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);

            Data.AddParticipant(new Driver("Test Driver", 100, defaultCar, TeamColors.Green));

            Assert.AreEqual(6, Data.Participants.Count);
            Assert.AreEqual("Koen", Data.Participants.First().Name);
            Assert.AreEqual("Stan", Data.Participants[4].Name);
            Assert.AreEqual("Test Driver", Data.Participants[5].Name);
        }

        [Test]
        public void Data_Initialize_Tracks_CanRead()
        {
            Data.Initialize();

            Assert.IsNotEmpty(Data.Tracks);
            Assert.AreEqual(3, Data.Tracks.Count);
            Assert.AreNotSame("Circuit Zwolle", Data.Tracks.Peek().Name);
            Assert.AreSame("TT Assen", Data.Tracks.Peek().Name);
            Assert.AreSame("Circuit Harderwijk", Data.Tracks.ToArray()[1].Name);
        }

        [Test]
        public void Data_Tracks_CanAdd()
        {
            SectionTypes[] routeAmsterdam =
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner,
                SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid
            };

            Data.AddTrack(new Track("Test Route", routeAmsterdam));
            
            Assert.AreEqual(4, Data.Tracks.Count);
            Assert.AreSame("Test Route", Data.Tracks.ToArray()[3].Name);
        }

        [Test]
        public void Data_CurrentRace_CanRead()
        {
            Assert.AreEqual(5, Data.CurrentRace.Participants.Count);
            Assert.AreSame("Circuit Zwolle", Data.CurrentRace.Track.Name);
        }

        [Test]
        public void Data_CurrentRace_NextRace()
        {
            Assert.AreSame("Circuit Zwolle", Data.CurrentRace.Track.Name);
            Data.NextRace();
            Assert.AreSame("TT Assen", Data.CurrentRace.Track.Name);
            Data.NextRace();
            Assert.AreSame("Circuit Harderwijk", Data.CurrentRace.Track.Name);
        }

        [Test]
        public void Data_CurrentRace_NextRace_ReturnsNull()
        {
            Data.NextRace();
            Assert.IsNotNull(Data.CurrentRace);
            Data.NextRace();
            Assert.IsNotNull(Data.CurrentRace);
            Data.NextRace();
            Assert.IsNotNull(Data.CurrentRace);
            Data.NextRace();
            Assert.IsNull(Data.CurrentRace);
            Data.NextRace();
            Assert.IsNull(Data.CurrentRace);
        }

    }
}
