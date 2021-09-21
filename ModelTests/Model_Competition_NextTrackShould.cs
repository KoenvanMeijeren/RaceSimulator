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

        private Queue<Track> _tracks = new Queue<Track>();

        [SetUp]
        public void Setup()
        {
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

            this._tracks = new Queue<Track>();
            this._tracks.Enqueue(new Track(name: "Circuit Zwolle", sections: routeZwolle));
            this._tracks.Enqueue(new Track(name: "TT Assen", sections: routeElburg));
            this._tracks.Enqueue(new Track(name: "Monaco", sections: routeAmsterdam));

            this._emptyCompetition = new Competition(new List<IParticipant>(), new Queue<Track>());
            this._competition = new Competition(new List<IParticipant>(), this._tracks);
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = this._emptyCompetition.NextTrack();
            Assert.IsNull(result);
            Assert.AreEqual(0, this._emptyCompetition.Tracks.Count);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var result = this._competition.NextTrack();
            Assert.IsInstanceOf(typeof(Track), result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] routeZwolle = {
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish,
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid
            };
            Queue<Track> tracks = new Queue<Track>();
            tracks.Enqueue(new Track(name: "Circuit Zwolle", sections: routeZwolle));

            Competition testCompetition = new Competition(new List<IParticipant>(), tracks);

            Assert.AreEqual("Circuit Zwolle", testCompetition.NextTrack().Name);
            Assert.IsNull(testCompetition.NextTrack());
            Assert.IsNull(testCompetition.NextTrack());
        }

        [Test]
        public void NextTrack_TracksInQueue_ReturnNextTrack()
        {
            Assert.AreEqual(3, this._competition.Tracks.Count);

            Track currentTrack = this._competition.NextTrack();
            Assert.IsInstanceOf(typeof(Track), currentTrack);
            Assert.AreEqual(2, this._competition.Tracks.Count);
            Assert.AreEqual("Circuit Zwolle", currentTrack.Name);

            currentTrack = this._competition.NextTrack();
            Assert.IsInstanceOf(typeof(Track), currentTrack);
            Assert.AreEqual(1, this._competition.Tracks.Count);
            Assert.AreEqual("TT Assen", currentTrack.Name);

            currentTrack = this._competition.NextTrack();
            Assert.IsInstanceOf(typeof(Track), currentTrack);
            Assert.AreEqual(0, this._competition.Tracks.Count);
            Assert.AreEqual("Monaco", currentTrack.Name);
        }

    }
}
