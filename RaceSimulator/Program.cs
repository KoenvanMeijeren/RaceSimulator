using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Controller;
using Model;

namespace RaceSimulator
{
    class Program
    {
        [ExcludeFromCodeCoverage]
        static void Main(string[] args)
        {
            // tests for the classes
            Data.Initialize();

            CVisualization.Initialize();
            CVisualization.DrawTrack(Data.CurrentRace);
            Data.CurrentRace.Start();

            for (;;)
            {
                Thread.Sleep(100);
            }

        }
    }
}
