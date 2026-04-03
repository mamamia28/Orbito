using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    internal class Position
    {
        private int x;
        private int y;


        public int GetX()
        {
            return x;
        }
        public int SetX(int value)
        {
            x = value;
            return x;
        }

        public int GetY()
        {
            return y;
        }
        public int SetY(int value)
        {
            y = value;
            return y;
        }

        public Position(int x, int y)
        {

            this.x = x; this.y = y;
        }
    }
}
