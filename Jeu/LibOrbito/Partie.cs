using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LibOrbito
{
    public class Partie
    {
        /*
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        */

        private ModeJeu _mode;
        public ModeJeu Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private EtatPartie _etat;

        public EtatPartie Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }
        private int _compteurTours;
        public int CompteurTours
        {
            get { return _compteurTours; }
            set { _compteurTours = value; }
        }

        /*
        private Vector<Coup> coupsJoues;

        private List<Coup> historiquesCoups;
        */
        private Plateau lePlateau { get; set; }

        private Joueur Joueur1 { get; set; }
        private Joueur Joueur2 { get; set; }
        private Joueur JoueurCourant { get; set; }
        private Joueur? JouerGagnant { get; set; }



        public Partie(ModeJeu mode, Joueur j1, Joueur j2)
        {
            Mode = mode;
            Joueur1 = j1;
            Joueur2 = j2;

            lePlateau = new Plateau();
            Etat = EtatPartie.Configuration;
        }

        public void demarrer()
        {
            if (Joueur1 == null || Joueur2 == null)
            {
                throw new Exception("Il faut 2 joueurs pour démarrer.");
            }

            if (Joueur1.CouleurBilles == Couleur.Blanc)
            {
                JoueurCourant = Joueur1;
            } else
            {
                JoueurCourant = Joueur2;
            }

            CompteurTours = 0;
            lePlateau.Initialiser();
            Etat = EtatPartie.EnCours;
        }
        public void JouerTour(Position posAjoutBille, Position? posOrigineBille, Position? posDestinationBille)
        {
            if (Etat != EtatPartie.EnCours) return;
            CompteurTours++;

            if(posOrigineBille != null && posDestinationBille != null)
            {
                lePlateau.DeplacerBille(posOrigineBille, posDestinationBille);
            }

            JoueurCourant.placerBille(posAjoutBille);
            lePlateau.AjouterBille(new Bille(JoueurCourant.CouleurBilles), posAjoutBille);

            lePlateau.ActionnerOrbite();

            VerifierVictoire();

            if(EtatPartie.EnCours == Etat)
            {
                JoueurCourant = (JoueurCourant == Joueur1) ? Joueur2 : Joueur1;
                CompteurTour++;
            }
        }
        
        public Joueur? VerifierVictoire()
        {
            Couleur? gagnant;
            gagnant=lePlateau.DetecterAlignement();
            
            if(gagnant == null ) { Etat = EtatPartie.Egalite; return null; } 
            
            Etat = EtatPartie.Terminee;
            if(gagnant == Joueur1.CouleurBilles)
            {
                return Joueur1; 
            }
            return Joueur2;  
            }
    }
}
