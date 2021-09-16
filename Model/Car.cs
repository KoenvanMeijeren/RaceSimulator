using System;

namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; private set; }

        public int Performance { get; private set; }

        public int Speed { get; private set; }

        public bool IsBroken { get; private set; }


        public Car(int quality, int performance, int speed)
        {
            this.Quality = quality;
            this.Performance = performance;
            this.Speed = speed;
            this.IsBroken = false;
        }

    }
}
