using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Coup
    {
        private Position _origine;
        public Position Origine
        {
            get { return _origine; }
            set { _origine = value; }
        }

        private Position _destination;

        public Position Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        private Position _orbitoActionne;

        public Position OrbitoActionne
        {
            get
            {
                return _orbitoActionne;
            }
            set { _orbitoActionne = value;}
        }

        private Joueur _responsable;

        public Joueur Responsable
        {
            get
            {
                return _responsable;
            }
            set
            {
                _responsable = value;
            }
        }
        public Coup(Position origine, Position destination, Position orbitoActionne, Joueur responsable)
        {
            Origine = origine;
            Destination = destination;
            OrbitoActionne = orbitoActionne;
            Responsable = responsable;
        }
    }
}
