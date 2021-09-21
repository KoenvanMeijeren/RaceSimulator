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

        [Test]
        public void TrackCanCreate()
        {
            SectionTypes[] route = {
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.Finish,
                SectionTypes.LeftCorner, SectionTypes.StartGrid, SectionTypes.LeftCorner, SectionTypes.StartGrid
            };

            Track track = new Track("Test Track", route);

            Assert.AreEqual("Test Track", track.Name);
            Assert.AreEqual(route, track.Sections);
        }

    }
}
