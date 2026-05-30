using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente les statistiques d'un joueur (nombre de parties, de victoires, de défaites et son taux de victoire)
    /// </summary>
    public class Statistiques
    {
        private int _partiesJoueees;
        private int _victoires;
        private int _defaites;
        private float _tauxVictoire;

        /// <summary>
        /// Nombre total de parties jouées par le joueur.
        /// </summary>
        public int PartiesJouees => _partiesJoueees;
        /// <summary>
        /// Nombre total de victoires du joueur.
        /// </summary>
        public int Victoires => _victoires;
        /// <summary>
        /// Nombre total de défaites du joueur.
        /// </summary>
        public int Defaites => _defaites;
        /// <summary>
        /// Le taux de victoire du joueur (en pourcentage).
        /// </summary>
        public float TauxVictoire => _tauxVictoire;

        /// <summary>
        /// Inititalise une nouvelle instance de la classe Statistiques et calcule le taux de victoire.
        /// </summary>
        /// <param name="partieJoueees">Le nombre total de parties jouées.</param>
        /// <param name="victoires">Le nombre total de parties gagnées.</param>
        /// <param name="defaites">Le nombre total de parties perdues.</param>
        public Statistiques( int partieJoueees, int victoires, int defaites)
        {
            _partiesJoueees = partieJoueees;
            _victoires = victoires;
            _defaites = defaites;

            if(partieJoueees > 0)
            {
                _tauxVictoire = (float)victoires / partieJoueees * 100;
            } else
            {
                _tauxVictoire = 0;
            }
        }

        public override string ToString()
        {
            return $"Stats : {_partiesJoueees} parties -> {_victoires} Victoires / {_defaites} Defaites / {_partiesJoueees-(_victoires+_defaites)} Egalité -> {_tauxVictoire}% Taux de Victoire";
        }
    }
}
