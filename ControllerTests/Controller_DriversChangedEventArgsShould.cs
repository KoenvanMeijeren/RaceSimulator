using System.Collections.Generic;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTests
{
    [TestFixture]
    public class Controller_DriversChangedEventArgsShould
    {

        [Test]
        public void DriversChangedEventArgs_CanCreate()
        {
            SectionTypes[] route =
            {
                SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.RightCorner,
                SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.StartGrid
            };
            List<IParticipant> participants = new List<IParticipant>();
            IEquipment defaultCar = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);
            IEquipment toyota = new Car(quality: IEquipment.MaximumQuality, performance: IEquipment.MaximumPerformance, speed: IEquipment.MaximumSpeed);

            participants.Add(new Driver(name: "Koen van Meijeren", points: 200, equipment: defaultCar, teamColor: TeamColors.Red));
            participants.Add(new Driver(name: "Klaas van Meijeren", points: 190, equipment: toyota, teamColor: TeamColors.Blue));
            participants.Add(new Driver(name: "Jan van Meijeren", points: 195, equipment: defaultCar, teamColor: TeamColors.Green));

            DriversChangedEventArgs eventArgs = new DriversChangedEventArgs(new Race(new Track(name: "Monaco", sections: route), participants));

            Assert.AreEqual("Monaco", eventArgs.Race.Track.Name);
        }

    }
}
