using System.ComponentModel;
using Controller;

namespace WPFRaceSimulator
{
    public class RaceDataContext : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _trackName;
        public string TrackName
        {
            get => this._trackName;
            private set
            {
                this._trackName = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrackName"));
            }
        }

        public RaceDataContext()
        {
            this.TrackName = "test";
        }

        private void OnDriversChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            this.TrackName = eventArgs.Race.Track.Name;
        }

    }
}