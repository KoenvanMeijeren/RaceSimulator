using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IParticipant
    {

        public string Name { get; }

        public int Points { get; }

        public IEquipment Equipment { get; }

        public TeamColors TeamColor { get; }

        public string GetInitials(int initialsLength = 2);

    }

    public enum TeamColors
    {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }

}
