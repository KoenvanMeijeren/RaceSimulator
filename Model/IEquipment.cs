using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IEquipment
    {

        public int Quality { get; }
        public int Performance { get; }
        public int Speed { get; }
        
        public bool IsBroken { get; }

    }
}
