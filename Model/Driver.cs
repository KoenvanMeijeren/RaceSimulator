using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; private set; }

        public int Points { get; private set; }

        public IEquipment Equipment { get; private set; }

        public TeamColors TeamColor { get; private set; }


        public Driver(string name, int points, IEquipment equipment, TeamColors teamColor)
        {
            this.Name = name;
            this.Points = points;
            this.Equipment = equipment;
            this.TeamColor = teamColor;
        }

    }
}
