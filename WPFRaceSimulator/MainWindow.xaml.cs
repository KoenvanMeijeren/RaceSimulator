﻿using System;
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
using Application = System.Windows.Application;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace WPFRaceSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private RaceStatistics _raceStatisticsWindow;
        private CompetitionStatistics _competitionStatisticsWindow;

        public MainWindow()
        {
            this.InitializeComponent();

            Data.Initialize();
            this.StartRace();
        }

        private void OnDriversChanged(object sender, DriversChangedEventArgs eventArgs)
        {
            // Dispatches an action for drawing the track until the program closes. When the current race changes, the
            // drawn track will also change.
            this.TrackImage.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.TrackImage.Source = null;
                    this.TrackImage.Source = WPFVisualization.DrawTrack(Data.CurrentRace);
                })
            );
        }

        private void OnRaceEnded(object source, DriversChangedEventArgs eventArgs)
        {
            Data.NextRace();
            if (Data.CurrentRace == null)
            {
                Console.WriteLine("De races zijn afgelopen.");
                return;
            }

            this.StartRace();
        }

        private void StartRace()
        {
            Race.DriversChanged += OnDriversChanged;
            Data.CurrentRace.RaceEnded += OnRaceEnded;

            WPFVisualization.Initialize();
            Data.CurrentRace.Start();
        }

        private void MenuItem_RaceStatistics_Click(object sender, RoutedEventArgs e)
        {
            this._raceStatisticsWindow = new RaceStatistics();
            this._raceStatisticsWindow.Show();
        }
        private void MenuItem_CompetitionStatistics_Click(object sender, RoutedEventArgs e)
        {
            this._competitionStatisticsWindow = new CompetitionStatistics();
            this._competitionStatisticsWindow.Show();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }
}