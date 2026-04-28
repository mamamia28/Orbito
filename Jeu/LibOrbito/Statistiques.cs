using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Statistiques
    {
        private int _partieJouees;
        public int PartieJouees
        {
            get { return _partieJouees; }
        }

        private int _victoires;

        public int Victoires
        {
            get
            {
                return _victoires;
            }
            set
            {
                _victoires = value;
            }
        }

        private int _defaites;

        public int Defaites
        {
            get
            {
                return _defaites;
            }
            set
            {
                _defaites = value;
            }
        }

        private float _tauxVictoire;

        public float TauxVictoire
        {
            get
            {
                return _tauxVictoire;
            } 
            set
            {
                _tauxVictoire = value;
            }
        }

        void incrementerVictoire()
        {
            Victoires++;
        }

        void incrementerDefaite()
        {
            Defaites++;
        }
        float calculerTaux()
        {
            return TauxVictoire = Victoires / PartieJouees;
        }
    }
}
