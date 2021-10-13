using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            Assert.AreSame("Test", this._emptyRace.Track.Name);
            Assert.AreNotSame("01/01/0001 00:00:00", this._emptyRace.StartTime.ToString(CultureInfo.InvariantCulture));
            Assert.AreNotSame("01/01/0001 00:00:00", this._race.StartTime.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void Race_CanRead_SectionData()
        {
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.First?.Value).Left);
            Assert.IsNull(this._race.GetSectionData(this._race.Track.Sections.First?.Value).Right);
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

            Assert.AreNotEqual("1-1-0001 00:00:00", this._race.StartTime.ToString(CultureInfo.InvariantCulture));
            Assert.AreNotEqual("1-1-0001 00:00:00", this._emptyRace.StartTime.ToString(CultureInfo.InvariantCulture));
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

        [Test]
        public void Race_CanMoveParticipantToNextSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, 0, null, 0);
            SectionData nextSectionData = new SectionData(section, null, 0, null, 0);
            
            SectionData updatedSection = this._race.MoveParticipantToNextSection(sectionData, section, nextSectionData, participant);

            Assert.IsNull(sectionData.Left);
            Assert.IsNull(sectionData.Right);
            Assert.IsNotNull(updatedSection.Left);
            Assert.IsNull(updatedSection.Right);
        }
        
        [Test]
        public void Race_CannotMoveParticipantToNextSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantLeft = new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantRight = new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, 0, null, 0);
            SectionData nextSectionData = new SectionData(section, participantLeft, 0, participantRight, 0);
            
            SectionData updatedSection = this._race.MoveParticipantToNextSection(sectionData, section, nextSectionData, participant);

            Assert.IsNotNull(sectionData.Left);
            Assert.IsNull(sectionData.Right);
            Assert.IsNotNull(updatedSection.Left);
            Assert.IsNotNull(updatedSection.Right);
        }
        
        [Test]
        public void Race_CanMoveParticipantsToNextSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantRight = new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, 0, participantRight, 0);
            SectionData nextSectionData = new SectionData(section, null, 0, null, 0);
            
            SectionData updatedSection = this._race.MoveParticipantsToNextSection(sectionData, section, nextSectionData, participant, participantRight);

            Assert.IsNull(sectionData.Left);
            Assert.IsNull(sectionData.Right);
            Assert.IsNotNull(updatedSection.Left);
            Assert.IsNotNull(updatedSection.Right);
        }
        
        [Test]
        public void Race_CannotMoveParticipantsToNextSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantRight = new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantThree = new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantFour = new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, 0, participantRight, 0);
            SectionData nextSectionData = new SectionData(section, participantThree, 0, participantFour, 0);
            
            SectionData updatedSection = this._race.MoveParticipantsToNextSection(sectionData, section, nextSectionData, participant, participantRight);

            Assert.IsNotNull(sectionData.Left);
            Assert.IsNotNull(sectionData.Right);
            Assert.IsNotNull(updatedSection.Left);
            Assert.IsNotNull(updatedSection.Right);
        }

        private bool _raceEndedEventFired;
        
        [Test]
        public void Race_CanFinish()
        {
            SectionTypes[] route =
            {
                SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish
            };

            List<IParticipant> participants = new List<IParticipant>();
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red));
            participants.Add(new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red));
            
            Track track = new Track("test", route);
            Race race = new Race(track, participants);
            race.RaceEnded += this.OnRaceEnded;
            
            race.Start();
            race.MoveParticipants();
            race.MoveParticipants();
            race.MoveParticipants();
            race.MoveParticipants();
            race.MoveParticipants();
            
            race.OnTimedEvent(this, null);
            race.OnTimedEvent(this, null);
            race.OnTimedEvent(this, null);
            
            Assert.IsTrue(this._raceEndedEventFired);
            Assert.IsTrue(race.AllParticipantsFinished());
        }

        private void OnRaceEnded(object source, DriversChangedEventArgs eventArgs)
        {
            this._raceEndedEventFired = true;
        }

        private bool _driversChangedEventFired;

        [Test]
        public void Race_CanFireDriversChangedEvent()
        {
            SectionTypes[] route =
            {
                SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish
            };

            List<IParticipant> participants = new List<IParticipant>();
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red));
            participants.Add(new Driver(name: "Test", points: 200, equipment: toyota, teamColor: TeamColors.Red));
            
            Track track = new Track("test", route);
            Race race = new Race(track, participants);
            race.DriversChanged += this.OnDriversChanged;
            
            race.Start();
            race.OnTimedEvent(this, null);
            
            Assert.IsTrue(this._driversChangedEventFired);
        }

        private void OnDriversChanged(object source, DriversChangedEventArgs eventArgs)
        {
            this._driversChangedEventFired = true;
        }

        [Test]
        public void Race_CanGetParticipantWhoShouldMoveToNextSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, Race.SectionLength, null, 0);
            SectionData secondSectionData = new SectionData(section, participant, Race.SectionLength + 10, null, 0);
            
            Assert.IsNotNull(this._race.GetParticipantWhoShouldMoveToNextSection(sectionData));
            Assert.IsNotNull(this._race.GetParticipantWhoShouldMoveToNextSection(secondSectionData));
        }
        
        [Test]
        public void Race_CannotGetParticipantWhoShouldMoveToNextSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, Race.SectionLength - 10, null, 0);
            
            Assert.IsNull(this._race.GetParticipantWhoShouldMoveToNextSection(sectionData));
        }

        [Test]
        public void Race_CannotSetParticipantOnSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantTwo = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, 0, participantTwo, 0);
            
            Assert.IsNull(this._race.ParticipantToSectionData(section, sectionData, participant));
        }
        
        [Test]
        public void Race_CannotSetParticipantsOnSection()
        {
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IParticipant participant = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            IParticipant participantTwo = new Driver(name: "Koen van Meijeren", points: 200, equipment: toyota, teamColor: TeamColors.Red);
            
            Section section = new Section(SectionTypes.Straight);
            SectionData sectionData = new SectionData(section, participant, 0, participantTwo, 0);
            
            Assert.IsNull(this._race.ParticipantsToSectionData(section, sectionData, participant, participant));
        }
        
    }
}
