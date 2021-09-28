using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTests
{
    [TestFixture]
    public class Controller_RaceShould
    {

        private Race _race;
        private Race _emptyRace;

        [SetUp]
        public void Setup()
        {
            Data.Initialize();

            SectionTypes[] route =
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner,
                SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid
            };
            List<IParticipant> participants = new List<IParticipant>();
            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

            participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            participants.Add(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            participants.Add(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));

            this._race = new Race(new Track(name: "Monaco", sections: route), participants);
            this._emptyRace = new Race(new Track("Test", new SectionTypes[0]), new List<IParticipant>());
        }

        [Test]
        public void Race_CanRead_Track()
        {
            Assert.AreEqual(5, Data.CurrentRace.Participants.Count);
            Assert.AreEqual("Circuit Zwolle", Data.CurrentRace.Track.Name);
            Assert.AreEqual("Monaco", this._race.Track.Name);
            Assert.AreEqual("Test", this._emptyRace.Track.Name);
            Assert.AreEqual("1-1-0001 00:00:00", this._emptyRace.StartTime.ToString());
            Assert.AreEqual("1-1-0001 00:00:00", this._race.StartTime.ToString());
        }

        [Test]
        public void Race_CanRead_SectionData()
        {
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.First.Value).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.First.Value).Right);
            Assert.IsNotNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(1)).Left);
            Assert.IsNotNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(1)).Right);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(2)).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(2)).Right);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(3)).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(3)).Right);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(4)).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(4)).Right);
            Assert.IsNotNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(5)).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(5)).Right);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(6)).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(6)).Right);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(7)).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.ElementAt(7)).Right);
        }

        [Test]
        public void Race_CanUpdate_SectionData()
        {
            Assert.IsFalse(this._emptyRace.UpdateSectionData(new Section(SectionTypes.StartGrid), new SectionData()));
            Assert.IsTrue(this._race.UpdateSectionData(this._race.Track.Sections.ElementAt(1), new SectionData()));
        }

        [Test]
        public void Race_CanStart()
        {
            this._race.Start();
            this._emptyRace.Start();

            Assert.AreNotEqual("1-1-0001 00:00:00", this._race.StartTime.ToString());
            Assert.AreNotEqual("1-1-0001 00:00:00", this._emptyRace.StartTime.ToString());
        }

        [Test]
        public void Race_CanPerformActions_OnTimedEvent()
        {
            this._race.Start();
            Race.OnTimedEvent(null, null);
            Race.OnTimedEvent(null, null);
        }

    }
}
