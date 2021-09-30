using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Controller;
using Model;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();

            CVisualization.Initialize();
            CVisualization.DrawTrack(Data.CurrentRace);
            Data.CurrentRace.Start();
            
            Data.NextRace();
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
