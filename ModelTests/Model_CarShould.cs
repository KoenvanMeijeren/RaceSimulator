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
    class Model_CarShould
    {

        [Test]
        public void Car_CanCreate()
        {
            Car car = new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed);

            Assert.AreEqual(IEquipment.MaximumQuality, car.Quality);
            Assert.AreEqual(IEquipment.MaximumPerformance, car.Performance);
            Assert.AreEqual(IEquipment.MaximumSpeed, car.Speed);
            Assert.IsFalse(car.IsBroken);
        }
        
        [Test]
        public void Car_CannotCreate_QualityValueOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car(IEquipment.MaximumQuality + 1, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car(IEquipment.MinimumQuality - 1, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MinimumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality - 40, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed));
        }

        [Test]
        public void Car_CannotCreate_PerformanceValueOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance + 1, IEquipment.MaximumSpeed));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car(IEquipment.MaximumQuality, IEquipment.MinimumPerformance - 1, IEquipment.MinimumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MinimumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance - 1, IEquipment.MaximumSpeed));
        }

        [Test]
        public void Car_CannotCreate_SpeedValueOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed + 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MinimumSpeed - 1));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MinimumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed));
            Assert.DoesNotThrow(() => new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed - 40));
        }

        [Test]
        public void Car_CanCreate_CanSetRandomQuality()
        {
            Car car = new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed);

            int previousQuality = car.Quality;
            car.SetRandomQuality();
            Assert.AreNotSame(car.Quality, previousQuality);
            Assert.IsTrue(car.Quality >= IEquipment.MinimumQuality);
            Assert.IsTrue(car.Quality <= IEquipment.MaximumQuality);
        }

        [Test]
        public void Car_CanCreate_CanSetRandomPerformance()
        {
            Car car = new Car(IEquipment.MaximumQuality, IEquipment.MaximumPerformance, IEquipment.MaximumSpeed);

            int previousPerformance = car.Performance;
            car.SetRandomPerformance();
            Assert.AreNotSame(car.Performance, previousPerformance);
            Assert.IsTrue(car.Performance >= IEquipment.MinimumPerformance);
            Assert.IsTrue(car.Performance <= IEquipment.MaximumPerformance);
        }

        [Test]
        public void Car_CanRead_RealSpeed()
        {
            Car car = new Car(100, 2, 25);
            
            Assert.AreEqual(50, car.GetRealSpeed());
            Assert.AreNotEqual(2, car.GetRealSpeed());
            Assert.AreNotEqual(25, car.GetRealSpeed());
        } 

    }
}
