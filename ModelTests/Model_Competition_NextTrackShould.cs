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

        private Competition _emptyCompetition;
        private Competition _competition;

        private Queue<Track> Tracks = new Queue<Track>();

        [SetUp]
        public void Setup()
        {
            IEquipment defaultCar = new Car(quality: 100, performance: 150, speed: 25);
            IEquipment toyota = new Car(quality: 65, performance: 34, speed: 10);

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

            this.Tracks = new Queue<Track>();
            this.Tracks.Enqueue(new Track(name: "Circuit Zwolle", sections: routeZwolle));
            this.Tracks.Enqueue(new Track(name: "TT Assen", sections: routeElburg));
            this.Tracks.Enqueue(new Track(name: "Monaco", sections: routeAmsterdam));

            this._emptyCompetition = new Competition(new List<IParticipant>(), new Queue<Track>());
            this._competition = new Competition(new List<IParticipant>(), this.Tracks);
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = this._emptyCompetition.NextTrack();
            Assert.IsNull(result);
            Assert.AreEqual(0, this._emptyCompetition.Tracks.Count);
        }

        [Test]
        public void NextTrack_TracksInQueue()
        {
            Assert.AreEqual(3, this._competition.Tracks.Count);

            Assert.IsInstanceOf(typeof(Track), this._competition.NextTrack());
            Assert.AreEqual(2, this._competition.Tracks.Count);

            Assert.IsInstanceOf(typeof(Track), this._competition.NextTrack());
            Assert.AreEqual(1, this._competition.Tracks.Count);

            Assert.IsInstanceOf(typeof(Track), this._competition.NextTrack());
            Assert.AreEqual(0, this._competition.Tracks.Count);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var result = this._competition.NextTrack();
            Assert.IsInstanceOf(typeof(Track), result);
        }

    }
}
