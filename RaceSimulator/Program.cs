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
            
            Race.RaceEnded += Program.OnRaceEnded;

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
                Race.DestructAllEvents();
                Console.SetCursorPosition(5, 5);
                Console.WriteLine("De races zijn afgelopen.");
                return;
            }
            
            Program.StartRace();
        }

        private static void StartRace()
        {
            CVisualization.Initialize();
            CVisualization.DrawTrack(Data.CurrentRace);
            Data.CurrentRace.Start();
        }
        
    }
}
