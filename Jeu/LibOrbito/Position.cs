using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }


        public Position(int x, int y)
        {

            X = x; 
            Y = y;
        }

        
      public bool EstAdjacente(Position autre)
        {
            if (this.X == autre.X && this.Y == autre.Y)
            {
                return false;
            }
            if (this.X == autre.X)
            {
                return (autre.Y == this.Y - 1) || (autre.Y == this.Y) || (autre.Y == this.Y + 1);
            }
            if (this.Y == autre.Y)
            {
                return (autre.X == this.X - 1) || (autre.X == this.X) || (autre.X == this.X + 1);
            }
            return false;
        }
        
    }
}