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

        private SectionData _sectionData;

        [SetUp]
        public void Setup()
        {
            Car car = new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed);
            Driver driverLeft = new Driver("Klaas", 100, car, TeamColors.Blue);
            Driver driverRight = new Driver("Jan", 100, car, TeamColors.Blue);

            this._sectionData = new SectionData(new Section(SectionTypes.StartGrid), driverLeft, 10, driverRight, 20);
        }
        
        [Test]
        public void SectionData_CanCreate()
        {
            Assert.AreEqual(SectionTypes.StartGrid, this._sectionData.Section.SectionType);
            Assert.AreEqual("Klaas", this._sectionData.Left.Name);
            Assert.AreEqual("Jan", this._sectionData.Right.Name);
            Assert.AreEqual(10, this._sectionData.DistanceLeft);
            Assert.AreEqual(20, this._sectionData.DistanceRight);
        }

        [Test]
        public void SectionData_CanRead_CanCompare()
        {
            Car car = new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed);
            Driver driverLeft = new Driver("Klaas", 100, car, TeamColors.Blue);
            Driver driverRight = new Driver("Jan", 100, car, TeamColors.Blue);
            
            Assert.IsFalse(this._sectionData.Left.Equals(this._sectionData.Right));
            Assert.IsFalse(this._sectionData.Left.Equals(driverRight));
            Assert.IsFalse(this._sectionData.Left.Equals(null));
            Assert.IsFalse(this._sectionData.Left.Equals(car));
            
            Assert.IsTrue(this._sectionData.Left.Equals(driverLeft));
            Assert.IsTrue(this._sectionData.Left.Equals(new Driver("Klaas", 100, car, TeamColors.Blue)));
        }

        [Test]
        public void SectionData_CanClear_BothParticipants()
        {
            this._sectionData.Clear(this._sectionData.Left, this._sectionData.Right);
            
            Assert.IsNull(this._sectionData.Left);
            Assert.IsNull(this._sectionData.Right);
            Assert.AreEqual(0, this._sectionData.DistanceLeft);
            Assert.AreEqual(0, this._sectionData.DistanceRight);
        }
        
        [Test]
        public void SectionData_CanClear_OneParticipants()
        {
            this._sectionData.Clear(null);
            this._sectionData.Clear(this._sectionData.Left);
            
            Assert.IsNull(this._sectionData.Left);
            Assert.IsNotNull(this._sectionData.Right);
            Assert.AreEqual(0, this._sectionData.DistanceLeft);
            Assert.AreNotEqual(0, this._sectionData.DistanceRight);
        }

        [Test]
        public void SectionData_CanMoveParticipants()
        {
            int previousDistance = this._sectionData.DistanceLeft;
            this._sectionData.MoveLeft();
            Assert.AreNotEqual(previousDistance, this._sectionData.DistanceLeft);
            
            previousDistance = this._sectionData.DistanceRight;
            this._sectionData.MoveRight();
            Assert.AreNotEqual(previousDistance, this._sectionData.DistanceRight);

            SectionData sectionData = new SectionData();
            previousDistance = sectionData.DistanceLeft;
            sectionData.MoveLeft();
            Assert.AreEqual(previousDistance, sectionData.DistanceLeft);
            
            previousDistance = sectionData.DistanceRight;
            sectionData.MoveRight();
            Assert.AreEqual(previousDistance, sectionData.DistanceRight);
        }

        [Test]
        public void Participants_CanBreakLeftEquipment()
        {
            Assert.IsFalse(this._sectionData.Left.Equipment.IsBroken);
            this._sectionData.BreakEquipmentLeft();
            Assert.IsTrue(this._sectionData.Left.Equipment.IsBroken);
        }
        
        [Test]
        public void Participants_CanBreakRightEquipment()
        {
            Assert.IsFalse(this._sectionData.Right.Equipment.IsBroken);
            this._sectionData.BreakEquipmentRight();
            Assert.IsTrue(this._sectionData.Right.Equipment.IsBroken);
        }
        
        [Test]
        public void Participants_CannotBreakEquipment()
        {
            SectionData sectionData = new SectionData();
            sectionData.BreakEquipmentLeft();
            sectionData.BreakEquipmentRight();
        }
        
        [Test]
        public void Participants_CanFixLeftEquipment()
        {
            this._sectionData.BreakEquipmentLeft();
            Assert.IsTrue(this._sectionData.Left.Equipment.IsBroken);
            this._sectionData.FixEquipmentLeft();
            Assert.IsFalse(this._sectionData.Left.Equipment.IsBroken);
        }
        
        [Test]
        public void Participants_CanFixRightEquipment()
        {
            this._sectionData.BreakEquipmentRight();
            Assert.IsTrue(this._sectionData.Right.Equipment.IsBroken);
            this._sectionData.FixEquipmentRight();
            Assert.IsFalse(this._sectionData.Right.Equipment.IsBroken);
        }
        
        [Test]
        public void Participants_CannotFixEquipment()
        {
            SectionData sectionData = new SectionData();
            sectionData.FixEquipmentLeft();
            sectionData.FixEquipmentRight();
        }
        
    }
}
