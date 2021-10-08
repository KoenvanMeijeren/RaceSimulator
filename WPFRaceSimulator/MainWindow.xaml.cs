using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace WPFRaceSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.Initialize();
            MainWindow.StartRace();

            Race.DriversChanged += MainWindow.OnDriversChanged;
            Race.RaceEnded += MainWindow.OnRaceEnded;

            // @todo: Find out what this method does and what the event methods should do.
            this.TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.TrackImage.Source = null;
                    this.TrackImage.Source = WPFVisualization.DrawTrack(Data.CurrentRace);
                })
            );
        }

        private static void OnDriversChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            WPFVisualization.DrawTrack(eventArgs.Race);
        }

        private static void OnRaceEnded(object source, DriversChangedEventArgs eventArgs)
        {
            Data.NextRace();
            if (Data.CurrentRace == null)
            {
                Race.DestructAllEvents();
                Console.WriteLine("De races zijn afgelopen.");
                return;
            }

            MainWindow.StartRace();
        }

        private static void StartRace()
        {
            WPFVisualization.Initialize();
            Data.CurrentRace.Start();
        }

    }
}