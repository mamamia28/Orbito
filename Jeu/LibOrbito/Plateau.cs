using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Swift;
using System.Text;

namespace LibOrbito
{
    public class Plateau
    {
        private Bille[,] _tabJeu;
        public Bille[,] TabJeu { get => _tabJeu; }

        private int _rayon;
        public int Rayon
        {
            get => _rayon;
            private set
            {
                if (value == 2)
                {
                    _rayon = value;
                }
            }
        }

        public void Initialiser()
        {
            _tabJeu = new Bille[4, 4];
        }

        public Plateau(int rayon = 2)
        {
            Rayon = rayon;
            Initialiser();
        }
    }
}
