using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Action
    {
        public Position? origineDeplacement { get; private set; }
        public Position? destinationDeplacement { get; private set; }
        public Position? placement { get; private set; }

        public Action(Position? origineDeplacement, Position? destinationDeplacement, Position? placement)
        {
            this.origineDeplacement = origineDeplacement;
            this.destinationDeplacement = destinationDeplacement;
            this.placement = placement;
        }
    }
}
