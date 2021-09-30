using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IEquipment
    {

        public const int MinimumQuality = 0;
        public const int MaximumQuality = 100;

        public const int MinimumPerformance = 1;
        public const int MaximumPerformance = 2;

        public const int MinimumSpeed = 0;
        public const int MaximumSpeed = 100;

        public int Quality { get; }
        public int Performance { get; }
        public int Speed { get; }
        
        public bool IsBroken { get; }

        public int GetRealSpeed();
        
        public void SetRandomPerformance();
        public void SetRandomQuality();

    }
}
