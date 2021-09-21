using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ModelTests
{
    [TestFixture]
    class Model_SectionShould
    {

        [Test]
        public void CanCreateSection()
        {
            Section sectionOne = new Section(SectionTypes.StartGrid);
            Section sectionTwo = new Section(SectionTypes.LeftCorner);
            Section sectionThree = new Section(SectionTypes.Straight);
            Section sectionFour = new Section(SectionTypes.RightCorner);
            Section sectionFive = new Section(SectionTypes.Finish);

            Assert.AreEqual(SectionTypes.StartGrid, sectionOne.SectionType);
            Assert.AreEqual(SectionTypes.LeftCorner, sectionTwo.SectionType);
            Assert.AreEqual(SectionTypes.Straight, sectionThree.SectionType);
            Assert.AreEqual(SectionTypes.RightCorner, sectionFour.SectionType);
            Assert.AreEqual(SectionTypes.Finish, sectionFive.SectionType);
        }

    }
}
