using System;

namespace Controller
{
    public class DriversChangedEventArgs : EventArgs
    {

        public Race Race { get; private set; }
        
        public bool RaceEnded { get; set; }

        public DriversChangedEventArgs(Race race, bool raceEnded = false)
        {
            this.Race = race;
            this.RaceEnded = raceEnded;
        }

    }
}
