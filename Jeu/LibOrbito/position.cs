using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Position
    {
        private int x { get; set; }
        private int y { get; set; }


        public Position(int x, int y)
        {

            this.x = x; this.y = y;
        }
    }
}
