﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {

        public IParticipant Left { get; private set; }
        public int DistanceLeft { get; private set; }

        public IParticipant Right { get; private set; }
        public int DistanceRight { get; private set; }

        public SectionData(IParticipant left, int distanceLeft, IParticipant right, int distanceRight)
        {
            this.Left = left;
            this.DistanceLeft = distanceLeft;

            this.Right = right;
            this.DistanceRight = distanceRight;
        }

    }
}
