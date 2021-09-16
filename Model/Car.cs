using System;

namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; private set; }

        public int Performance { get; private set; }

        public int Speed { get; private set; }

        public bool IsBroken { get; private set; }

        private Random Randominizer;


        public Car(int quality, int performance, int speed)
        {
            this.Quality = quality;
            this.Performance = performance;
            this.Speed = speed;
            this.IsBroken = false;

            this.Randominizer = new Random(DateTime.Now.Millisecond);
        }

        public void SetRandomQuality()
        {
            this.Quality = this.Randominizer.Next(IEquipment.MinimumQuality, IEquipment.MaximumQuality);
        }

        public void SetRandomPerformance()
        {
            this.Performance = this.Randominizer.Next(IEquipment.MinimumPerformance, IEquipment.MaximumPerformance);
        }

    }
}
