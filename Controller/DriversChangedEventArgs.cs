using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class DriversChangedEventArgs : EventArgs
    {

        public Race Race { get; private set; }

        public DriversChangedEventArgs(Race race)
        {
            this.Race = race;
        }

    }
}
