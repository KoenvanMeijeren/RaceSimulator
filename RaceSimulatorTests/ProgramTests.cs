using NUnit.Framework;
using RaceSimulator;

namespace RaceSimulatorTests
{
    public class ProgramTests
    {

        [Test]
        public void Test1()
        {
            Assert.DoesNotThrow(() => Program.Run());
        }
    }
}