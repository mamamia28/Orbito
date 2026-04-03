using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Swift;
using System.Text;

namespace LibOrbito
{
    public class Plateau
    {
        Position[,] tabJeu;

        int _rayon;
        public int Rayon
        {
            get => _rayon;
            set
            {
                if (value == 2)
                {
                    _rayon = value;
                }
            }
        }
        Plateau(int rayon=2)
        {
            this.Rayon = rayon;
            tabJeu = new Position [4, 4];


        }

    }
}
