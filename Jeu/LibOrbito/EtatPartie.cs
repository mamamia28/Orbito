using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LibOrbito
{
    public enum EtatPartie
    {
        Configuration,
        EnCours,
        Terminee,
        Egalite
    }

    public static class EtatPartieS
    {
        public static string toChaine(this EtatPartie etat)
        {
            return etat switch
            {
                EtatPartie.EnCours => "En Cours",
                _ => etat.ToString()
            };
        }
    }
}
