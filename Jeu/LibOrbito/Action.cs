using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Action
    {
        protected Position? origineDeplacement;
        protected Position? destinationDeplacement;
        protected Position placement;

        public Action(Position? origineDeplacement, Position? destinationDeplacement, Position placement)
        {
            this.origineDeplacement = origineDeplacement;
            this.destinationDeplacement = destinationDeplacement;
            this.placement = placement;
        }
    }
}
