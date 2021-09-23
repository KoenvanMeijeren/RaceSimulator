using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NUnit.Framework;

namespace ModelTests
{
    [TestFixture]
    class Model_TrackShould
    {

        private Track _track;

        [SetUp]
        public void Setup()
        {
            SectionTypes[] route = {
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish,
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid
            };

            this._track = new Track("Test Track", route);
        }

        [Test]
        public void Track_CanCreate()
        {
            SectionTypes[] route = {
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish,
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid
            };

            Track track = new Track("Test Track 2", route);

            Assert.AreEqual("Test Track 2", track.Name);
            Assert.AreEqual(-1, track.EastStartPosition);
            Assert.AreEqual(-1, track.NorthStartPosition);
        }

        [Test]
        public void Track_CanRead_Sections()
        {
            Assert.AreEqual(8, this._track.Sections.Count);
            Assert.AreEqual(SectionTypes.LeftCorner, this._track.Sections.First.Value.SectionType);
            Assert.AreEqual(SectionTypes.StartGrid, this._track.Sections.ElementAt(1).SectionType);
            Assert.AreEqual(SectionTypes.LeftCorner, this._track.Sections.ElementAt(2).SectionType);
            Assert.AreEqual(SectionTypes.Finish, this._track.Sections.ElementAt(3).SectionType);

            Assert.AreEqual(SectionTypes.LeftCorner, this._track.Sections.ElementAt(4).SectionType);
            Assert.AreEqual(SectionTypes.StartGrid, this._track.Sections.ElementAt(5).SectionType);
            Assert.AreEqual(SectionTypes.LeftCorner, this._track.Sections.ElementAt(6).SectionType);
            Assert.AreEqual(SectionTypes.StartGrid, this._track.Sections.ElementAt(7).SectionType);

        }

        [Test]
        public void Track_CanCreate_WithDifferentStartPosition()
        {
            SectionTypes[] route = {
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish,
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid
            };

            Track track = new Track("Test Track 2", route, 25, 10);

            Assert.AreEqual("Test Track 2", track.Name);
            Assert.AreEqual(25, track.EastStartPosition);
            Assert.AreEqual(10, track.NorthStartPosition);
            Assert.AreEqual(-1, Track.StartPositionUndefined);

            Track trackTwo = new Track("Test Track 3", route, Track.StartPositionUndefined, 40);
            Assert.AreEqual("Test Track 3", trackTwo.Name);
            Assert.AreEqual(-1, trackTwo.EastStartPosition);
            Assert.AreEqual(40, trackTwo.NorthStartPosition);
            Assert.AreEqual(-1, Track.StartPositionUndefined);
        }

    }
}
