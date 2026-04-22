using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LibOrbito
{
    public class Partie
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private ModeJeu _mode;
        public ModeJeu Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private EtatPartie _etat;

        public EtatPartie Etat {
            get { return _etat; }
            set { _etat = value; }
        }
        private int _compteurTours;
        public int CompteurTours
        {
            get { return _compteurTours; }
            set { _compteurTours = value; }
        }

        private Vector<Coup> coupsJoues;

        private List<Coup> historiquesCoups;

        private Plateau lePlateau;

        private Joueur lesJoueurs;


        public Partie(ModeJeu mode)
        {
            Mode = mode;

        }
    }
}
