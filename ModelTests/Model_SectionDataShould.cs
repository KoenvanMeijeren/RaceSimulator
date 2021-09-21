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
    public class Model_SectionDataShould
    {

        [Test]
        public void SectionDate_CanCreate()
        {
            Car car = new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed);
            Driver driverLeft = new Driver("Klaas", 100, car, TeamColors.Blue);
            Driver driverRight = new Driver("Jan", 100, car, TeamColors.Blue);

            SectionData sectionData = new SectionData(driverLeft, 10, driverRight, 20);

            Assert.AreEqual("Klaas", sectionData.Left.Name);
            Assert.AreEqual("Jan", sectionData.Right.Name);
            Assert.AreEqual(10, sectionData.DistanceLeft);
            Assert.AreEqual(20, sectionData.DistanceRight);
        }

    }
}
