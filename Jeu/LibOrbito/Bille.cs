using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Bille
    {
        private Couleur couleur;
        public Couleur Couleur
        {
           get
            {
                return couleur;
            }
            set
            {
                if (value == Couleur.Blanc | value == Couleur.Noir)
                {
                    couleur = value;
                }
            }
        }
    }
}
