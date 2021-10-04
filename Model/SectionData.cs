﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {

        public Section Section { get; private set; }
        
        public IParticipant Left { get; private set; }
        public int DistanceLeft { get; private set; }

        public IParticipant Right { get; private set; }
        public int DistanceRight { get; private set; }

        public SectionData()
        {

        }

        public SectionData(Section section, IParticipant left, int distanceLeft, IParticipant right, int distanceRight)
        {
            this.Section = section;
            this.Left = left;
            this.DistanceLeft = distanceLeft;

            this.Right = right;
            this.DistanceRight = distanceRight;
        }

        public void MoveLeft()
        {
            if (this.Left == null || this.Left.Equipment.IsBroken)
            {
                return;
            }
            
            this.DistanceLeft += this.Left.Equipment.GetRealSpeed();
        }

        public void MoveRight()
        {
            if (this.Right == null || this.Right.Equipment.IsBroken)
            {
                return;
            }
            
            this.DistanceRight += this.Right.Equipment.GetRealSpeed();
        }

        public SectionData Clear(IParticipant participant)
        {
            if (participant == null)
            {
                return this;
            }

            if (this.Left != null && this.Left.Equals(participant))
            {
                this.Left = null;
                this.DistanceLeft = 0;
            }
            else if (this.Right != null && this.Right.Equals(participant))
            {
                this.Right = null;
                this.DistanceRight = 0;
            }

            return this;
        }

        public void Clear(IParticipant left, IParticipant right)
        {
            this.Clear(left).Clear(right);

        }

    }
}
