using System.ComponentModel;
using Controller;

namespace WPFRaceSimulator
{
    public class RaceDataContext : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public string TrackName { get; private set; }

        public RaceDataContext()
        {
            MainWindow.RaceChanged += this.OnRaceChanged;
        }

        private void OnRaceChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            this.TrackName = eventArgs.Race?.Track.Name;
            if (eventArgs.RaceEnded)
            {
                this.TrackName = "De races zijn afgelopen.";
            }
            
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrackName"));
        }

    }
}