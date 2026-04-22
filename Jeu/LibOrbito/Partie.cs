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

        private Joueur Joueur1;
        private Joueur Joueur2;
        private Joueur JoueurCourant;



        public Partie(ModeJeu mode)
        {
            Mode = mode;

        }
        public void demarrer()
        {
            if (Joueur1 == null || Joueur2 == null)
            {
                throw new Exception("Il faut 2 joueurs pour démarrer.");
            }
            
            lePlateau.Initialiser();
            Etat = EtatPartie.EnCours;
            Console.WriteLine("La partie a démarré."); ;
        }
        public void Jouer()
        {
            if (Etat != EtatPartie.EnCours) return;
            CompteurTours++;
        }
        public Joueur VerifierVictoire()
        {
            Couleur gagnant;
            gagnant=lePlateau.DetecterAlignement();
            
            if(gagnant == Couleur.Aucune ) { Etat = EtatPartie.Egalite; return null; } 
            
            Etat = EtatPartie.Terminee;
            if(gagnant == Joueur1.CouleurBilles)
            {
                return Joueur1; 
            }
            return Joueur2;  
            }

       
        }

            
            
       
    
}
