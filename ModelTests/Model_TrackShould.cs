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
        public void Track_CanCount_Sections()
        {
            SectionTypes[] route = {
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
            };
            
            Track track = new Track("Test Track 2", route);

            SectionTypes[] routeTest = {
                SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight,
                SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner,
                SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                SectionTypes.StartGrid, SectionTypes.StartGrid
                
            };
            
            Track trackTest = new Track("Test Track 2", routeTest);

            SectionTypes[] routeTestTwo =
            {
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
            };

            Track trackTestTwo = new Track("Test track 3", routeTestTwo);
            
            Assert.Fail();
        }

    }
}
