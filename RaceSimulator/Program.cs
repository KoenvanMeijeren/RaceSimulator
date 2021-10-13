using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Controller;
using Model;

namespace RaceSimulator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Data.Initialize();
            Program.StartRace();

            for (;;)
            {
                Thread.Sleep(100);
            }
        }

        private static void OnRaceEnded(object source, DriversChangedEventArgs eventArgs)
        {
            Data.NextRace();
            if (Data.CurrentRace == null)
            {
                string racesEnded = "De races zijn afgelopen.";
                Console.SetCursorPosition(CVisualization.CenteredTextCursorEastStartPosition(racesEnded), 5);
                Console.WriteLine(racesEnded);
                return;
            }
            
            Program.StartRace();
        }

        private static void StartRace()
        {
            CVisualization.Initialize(Data.CurrentRace);
            CVisualization.DrawTrack(Data.CurrentRace);
            Data.CurrentRace.Start();
            
            Data.CurrentRace.RaceEnded += Program.OnRaceEnded;
        }
        
    }
}
