using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Statistiques
    {
        private int _partiesJoueees;
        private int _victoires;
        private int _defaites;
        private float _tauxVictoire;

        public int PartiesJouees => _partiesJoueees;
        public int Victoires => _victoires;
        public int Defaites => _defaites;
        public float TauxVictoire => _tauxVictoire;

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
