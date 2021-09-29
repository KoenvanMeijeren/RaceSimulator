#nullable enable
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

        public string GetInitials(int initialsLength = 2)
        {
            char[] chars = this.Name.ToCharArray();

            StringBuilder initials = new StringBuilder(initialsLength);
            for (int delta = 0; delta < chars.Length && delta < initialsLength; delta++)
            {
                initials.Append(chars.ElementAtOrDefault(delta).ToString());
            }

            return initials.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is IParticipant participant)
            {
                return this.Name.Equals(participant.Name);
            }

            return false;
        }
    }
}
