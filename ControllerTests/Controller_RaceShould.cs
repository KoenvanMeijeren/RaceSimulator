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

        private List<IParticipant> _participants;

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
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);

            participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            participants.Add(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            participants.Add(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));

            this._participants = participants;
            this._race = new Race(new Track(name: "Monaco", sections: route), participants);
            this._emptyRace = new Race(new Track("Test", new SectionTypes[0]), new List<IParticipant>());
        }

        [Test]
        public void Race_CanRead_Track()
        {
            Assert.AreEqual(5, Data.CurrentRace.Participants.Count);
            Assert.AreSame("Circuit Zwolle", Data.CurrentRace.Track.Name);
            Assert.AreSame("Monaco", this._race.Track.Name);
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
            this._race.OnTimedEvent(null, null);
            this._race.OnTimedEvent(null, null);
        }

        [Test]
        public void Race_CanCovertRange()
        {
            Assert.AreEqual(0, Race.ConvertRange(0, 100, 0, 4, 10));
            Assert.AreEqual(1, Race.ConvertRange(0, 100, 0, 4, 30));
            Assert.AreEqual(2, Race.ConvertRange(0, 100, 0, 4, 60));
            Assert.AreEqual(3, Race.ConvertRange(0, 100, 0, 4, 80));
            Assert.AreEqual(4, Race.ConvertRange(0, 100, 0, 4, 110));
            
            Assert.AreEqual(0, Race.ConvertRange(0, 100, 0, 4, 12));
            Assert.AreEqual(0, Race.ConvertRange(0, 100, 0, 4, 24));
            Assert.AreEqual(1, Race.ConvertRange(0, 100, 0, 4, 36));
            Assert.AreEqual(1, Race.ConvertRange(0, 100, 0, 4, 48));
            Assert.AreEqual(2, Race.ConvertRange(0, 100, 0, 4, 60));
            Assert.AreEqual(2, Race.ConvertRange(0, 100, 0, 4, 72));
            Assert.AreEqual(3, Race.ConvertRange(0, 100, 0, 4, 84));
            Assert.AreEqual(3, Race.ConvertRange(0, 100, 0, 4, 96));
            Assert.AreEqual(4, Race.ConvertRange(0, 100, 0, 4, 108));
            Assert.AreEqual(4, Race.ConvertRange(0, 100, 0, 4, 120));
        }

        [Test]
        public void Race_CanFlipRace()
        {
            int value = Race.ConvertRange(0, 100, 0, 4, 10);
            Assert.AreEqual(4, Race.ConvertRange(0, 4,  4, 0, value));
            value = Race.ConvertRange(0, 100, 0, 4, 35);
            Assert.AreEqual(3, Race.ConvertRange(0, 4,  4, 0, value));
            value = Race.ConvertRange(0, 100, 0, 4, 55);
            Assert.AreEqual(2, Race.ConvertRange(0, 4,  4, 0, value));
            value = Race.ConvertRange(0, 100, 0, 4, 78);
            Assert.AreEqual(1, Race.ConvertRange(0, 4,  4, 0, value));
            value = Race.ConvertRange(0, 100, 0, 4, 105);
            Assert.AreEqual(0, Race.ConvertRange(0, 4,  4, 0, value));
        }

        [Test]
        public void Race_CannotUpdateRounds_ForNonExistingParticipant()
        {
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);

            IParticipant participant = new Driver(name: "Klaas", points: 200, equipment: defaultCar,
                teamColor: TeamColors.Red);
            
            Assert.IsFalse(this._race.UpdateRounds(participant, 2));
        }

        [Test]
        public void Race_FixParticipantsEquipment()
        {
            while (!this._race.ShouldFixParticipantEquipment())
            {
                continue;
            }
            
            Assert.IsTrue(true);
        }

        [Test]
        public void Race_CanRemoveParticipantsOnTrackCompletion()
        {
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar,
                teamColor: TeamColors.Red);
            
            SectionData sectionData = new SectionData(new Section(SectionTypes.Finish), participant, 0, null, 0);
            Assert.IsNotNull(sectionData.Left);
            
            sectionData = this._race.RemoveParticipantsOnTrackCompletion(sectionData, participant, 3);
            Assert.IsNull(sectionData.Left);
        }

        [Test]
        public void Race_CanGetRounds()
        {
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar,
                teamColor: TeamColors.Red);

            int rounds = this._race.GetRounds(participant);
            Assert.AreEqual(0, rounds);
            this._race.UpdateRounds(participant, 2);
            Assert.AreEqual(2, this._race.GetRounds(participant));
        }

        [Test]
        public void Race_CanPlaceAllParticipants()
        {
            SectionTypes[] route =
            {
                SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish
            };

            Race race = new Race(new Track("test", route), this._participants);
            race.PlaceParticipantsOnStartPositions();

            int participantsCount = 0;
            foreach (KeyValuePair<Section,SectionData> racePosition in race.Positions)
            {
                if (racePosition.Value.Left != null)
                {
                    participantsCount++;
                }

                if (racePosition.Value.Right != null)
                {
                    participantsCount++;
                }
            }
            
            Assert.AreEqual(race.Participants.Count, participantsCount);
            Assert.AreNotEqual(race.Participants.Count - 1, participantsCount);
        }
        
        [Test]
        public void Race_CannotPlaceAllParticipants()
        {
            SectionTypes[] route =
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.Finish
            };

            Race race = new Race(new Track("test", route), this._participants);
            race.PlaceParticipantsOnStartPositions();

            int participantsCount = 0;
            foreach (KeyValuePair<Section,SectionData> racePosition in race.Positions)
            {
                if (racePosition.Value.Left != null)
                {
                    participantsCount++;
                }

                if (racePosition.Value.Right != null)
                {
                    participantsCount++;
                }
            }
            
            Assert.AreNotEqual(this._participants.Count, participantsCount);
            Assert.AreEqual(2, participantsCount);
        }

        [Test]
        public void Race_CanOnlyPlaceParticipantsOnStartGrid()
        {
            SectionTypes[] route =
            {
                SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish
            };

            Race race = new Race(new Track("test", route), this._participants);
            race.PlaceParticipantsOnStartPositions();
            
            foreach (KeyValuePair<Section,SectionData> racePosition in race.Positions)
            {
                if (racePosition.Key.SectionType is SectionTypes.Finish or SectionTypes.Straight)
                {
                    Assert.IsNull(racePosition.Value.Left);
                    Assert.IsNull(racePosition.Value.Right);
                }
                else if (racePosition.Key.SectionType == SectionTypes.StartGrid)
                {
                    Assert.IsNotNull(racePosition.Value.Left ?? racePosition.Value.Right);
                }
            }
        }
        
    }
}
