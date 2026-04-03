using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Billes
    {
        private bool couleur { get; set; }
        
        public Billes(bool couleur) 
        {
        this.couleur = couleur;
        }

        public bool IsNotNull()
        {
            if (couleur == null) return false;
            return true;
        }

        public override string ToString()
        {
            string name = couleur ? "Blanc" : "Noir";
            return $"La couleur de la bille est {name}.";
        }

    }
}
