using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LibOrbito
{
    public class Coup : Action
    {
        public Joueur Responsable { get; private set; }
        public Coup(Position? origine, Position? destination, Position placement, Joueur responsable) : base(origine,destination,placement)
        {
            this.Responsable = responsable;
        }
    }
}
