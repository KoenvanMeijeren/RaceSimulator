using System;

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
