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
            get => this._quality;

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
            get => this._performance;

            private set
            {
                if (value < IEquipment.MinimumPerformance || value > IEquipment.MaximumPerformance)
                {
                    throw new ArgumentOutOfRangeException("Performance", value, $"The value must be equal to or higher then: {IEquipment.MinimumPerformance} and lower then or equal to {IEquipment.MaximumPerformance}.");
                }

                this._performance = value;
            }
        }

        public int Speed
        {
            get => this._speed;

            private set
            {
                if (value < IEquipment.MinimumSpeed || value > IEquipment.MaximumSpeed)
                {
                    throw new ArgumentOutOfRangeException("Speed", value, $"The value must be equal to or higher then: {IEquipment.MinimumSpeed} and lower then or equal to {IEquipment.MaximumSpeed}.");
                }

                this._speed = value;
            }
        }

        public bool IsBroken { get; set; }

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

        public void DecreasePerformance()
        {
            if (IEquipment.MinimumPerformance >= this._performance)
            {
                this._performance = IEquipment.MinimumPerformance;
                return;
            }

            this._performance--;
        }
        
        public void DecreaseSpeed()
        {
           int number = this._randominizer.Next(10, 20);
           if (IEquipment.MinimumSpeed >= (number - this._speed))
           {
               this._speed = IEquipment.MinimumSpeed;
               return;
           }

           this._speed -= number;
        }

    }
}
