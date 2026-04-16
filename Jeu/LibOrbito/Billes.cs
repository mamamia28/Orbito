using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Billes
    {
        private Couleur _couleur;

        public Couleur Couleur
        {
            get
            {
                return _couleur;
            }
            set
            {
                _couleur = Couleur;
            }
        }
        
        public Billes(Couleur couleur) 
        {
            this.Couleur = couleur;
        }

 /*       public bool IsNotNull()
        {
            if (couleur == null) return false;
            return true;
        }

        public override string ToString()
        {
            string name = couleur ? "Blanc" : "Noir";
            return $"La couleur de la bille est {name}.";
        }
 */

    }
}
