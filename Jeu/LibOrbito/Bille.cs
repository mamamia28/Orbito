using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Bille
    {
        private Couleur couleur;
        public Couleur CouleurB
        {
           get
            {
                return couleur;
            }
            set
            {
                if (value == Couleur.Blanc || value == Couleur.Noir)
                {
                    couleur = value;
                }
            }
        }
        public Bille(Couleur c)
        {
            CouleurB = c;
        }
    }
}
