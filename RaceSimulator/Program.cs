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

            foreach (IParticipant participant in Data.Participants)
            {
                Console.WriteLine($"Participant: {participant.Name}");
            }

            Console.WriteLine($"\nCurrent track: {Data.CurrentRace.Track.Name}");
            Data.NextRace();
            Console.WriteLine($"Next track: {Data.CurrentRace.Track.Name}");
            Data.NextRace();
            Console.WriteLine($"Next track 1: {Data.CurrentRace.Track.Name}");
            Data.NextRace();
            Console.WriteLine($"Next tracks: {Data.CurrentRace}");

            for (; ; )
            {
                Thread.Sleep(100);
            }

        }
    }
}
