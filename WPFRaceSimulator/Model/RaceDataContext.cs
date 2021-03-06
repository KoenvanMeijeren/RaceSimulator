using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Controller;
using Model;
using Section = Model.Section;

namespace WPFRaceSimulator.Model
{
    public class RaceDataContext : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public string TrackName { get; private set; }
        public string ActiveTrackName { get; private set; }
        public string FinishedParticipants { get; private set; }
        public string FinishedTracks { get; private set; }

        public List<Section> Sections { get; private set; }
        public List<IParticipant> Participants { get; private set; }

        public List<Race> Races { get; private set; }

        public RaceDataContext()
        {
            MainWindow.RaceChanged += this.OnRaceChanged;
            MainWindow.RefreshScreen += this.OnRaceChanged;
        }

        private void OnRaceChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            Race race = eventArgs.Race;
            if (race == null)
            {
                return;
            }

            Track track = race.Track;

            this.TrackName = track.Name;
            this.ActiveTrackName = $"Huidige track: {this.TrackName}";
            this.FinishedParticipants = $"Auto's gefinisht: {race.FinishedParticipants}";
            if (eventArgs.RaceEnded)
            {
                this.TrackName = "De races zijn afgelopen.";
                this.ActiveTrackName = this.TrackName;
            }

            this.Participants = race.Participants;
            this.Sections = race.Track.Sections.ToList();

            if (this.Races == null)
            {
                this.Races = (from subTrack in Data.TracksList select new Race(subTrack, Data.Participants)).ToList();
                this.Races.Add(Data.CurrentRace);

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Races"));
            }

            int finishedTracks = this.Races.Count - Data.Tracks.Count;
            if (!race.AllParticipantsFinished())
            {
                finishedTracks--;
            }

            this.FinishedTracks = $"Tracks gefinisht: {finishedTracks}";

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrackName"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveTrackName"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sections"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Participants"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FinishedParticipants"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FinishedTracks"));
        }

    }
}