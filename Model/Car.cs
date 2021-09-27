using System;

namespace Model
{
    public class Car : IEquipment
    {

        private int _quality;
        private int _performance;
        private int _speed;
        public int Quality
        {
            get
            {
                return this._quality;
            }

            private set
            {
                if (value < IEquipment.MinimumQuality || value > IEquipment.MaximumQuality)
                {
                    throw new ArgumentOutOfRangeException("Quality", value, $"The value must be equal to or higher then: {IEquipment.MinimumQuality} and lower then or equal to {IEquipment.MaximumQuality}.");
                }

                this._quality = value;
            }
        }

        public int Performance
        {
            get
            {
                return this._performance;
            }

            private set
            {
                if (value < IEquipment.MinimumPerformance || value > IEquipment.MaximumPerformance)
                {
                    throw new ArgumentOutOfRangeException("Performance", value, $"The value must be equal to or higher then: {IEquipment.MinimumQuality} and lower then or equal to {IEquipment.MaximumQuality}.");
                }

                this._performance = value;
            }
        }

        public int Speed
        {
            get
            {
                return this._speed;
            }

            private set
            {
                if (value < IEquipment.MinimumSpeed || value > IEquipment.MaximumSpeed)
                {
                    throw new ArgumentOutOfRangeException("Speed", value, $"The value must be equal to or higher then: {IEquipment.MinimumQuality} and lower then or equal to {IEquipment.MaximumQuality}.");
                }

                this._speed = value;
            }
        }

        public bool IsBroken { get; }

        private Random _randominizer;


        public Car(int quality, int performance, int speed)
        {
            this.Quality = quality;
            this.Performance = performance;
            this.Speed = speed;
            this.IsBroken = false;

            this._randominizer = new Random(DateTime.Now.Millisecond);
        }

        public int GetRealSpeed()
        {
            return this.Performance * this.Speed;
        }

        public void SetRandomQuality()
        {
            this.Quality = this._randominizer.Next(IEquipment.MinimumQuality, IEquipment.MaximumQuality);
        }

        public void SetRandomPerformance()
        {
            this.Performance = this._randominizer.Next(IEquipment.MinimumPerformance, IEquipment.MaximumPerformance);
        }

    }
}
