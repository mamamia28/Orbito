using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
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
            private set { _compteurTours = value; }
        }

        /*
        private Vector<Coup> coupsJoues;

        private List<Coup> historiquesCoups;
        */
        private Plateau LePlateau { get; set; }

        private Joueur Joueur1 { get; set; }
        private Joueur Joueur2 { get; set; }
        public Joueur JoueurCourant { get; set; }
        private Joueur? JoueurGagnant { get; set; }



        public Partie(string nomJ1, string avatarJ1, string nomJ2, string avatarJ2, ModeJeu mode)
        {
            Mode = mode;
            Joueur1 = new Joueur(nomJ1,avatarJ1,Couleur.Blanc);
            Joueur2 = new Joueur(nomJ2,avatarJ2,Couleur.Noir);

            LePlateau = new Plateau();

            JoueurCourant = Joueur1;


            CompteurTours = 1;
            LePlateau.Initialiser();
            Etat = EtatPartie.EnCours;
        }

        
        
        
        public Joueur? VerifierVictoire()
        {
            Couleur? gagnant;
            gagnant=LePlateau.DetecterAlignement();
            
            if(gagnant == null && CompteurTours>20) { Etat = EtatPartie.Egalite; return null; } 
            if (gagnant == null)
            {
                return null; 
            }
            return (Joueur1.CouleurBilles == gagnant) ? Joueur1 : Joueur2;  
        }

        public bool EstTerminee()
        {
            Joueur? statut = VerifierVictoire();
            if (statut == null && Etat == EtatPartie.Egalite || statut !=null)
            {
                return true;
            }
            return false;
        }

            

        public void ChangerJoueurCourant()
        {
            JoueurCourant = (JoueurCourant == Joueur1) ? Joueur2 : Joueur1;
        }


        /// <summary>
        /// Pose une nouvelle bille sur le plateau. Vérifie que le Joueur Courant a encore assez de billes.
        /// </summary>
        /// <param name="c">Couleur de la bille</param>
        /// <param name="p">Position à laquelle la future bille sera posée</param>
        public bool PoserBille(Position p)
        {
            if (Etat == EtatPartie.Terminee) return false;
            try
            {
                JoueurCourant.placerBille();
            }
            catch (Exception)
            {
                return false;
            }
            return LePlateau.AjouterBille(new Bille(JoueurCourant.CouleurBilles),p);
        }


        /// <summary>
        /// Déplace une bille adverse de la position spécifiée vers une nouvelle position sur le plateau, si le
        /// déplacement est autorisé.
        /// </summary>
        /// <remarks>Aucun déplacement n'est effectué si la case source est vide ou si elle contient une
        /// bille appartenant au joueur courant.</remarks>
        /// <param name="source">La position actuelle de la bille adverse à déplacer. Doit contenir une bille appartenant à l'adversaire.</param>
        /// <param name="dest">La position de destination vers laquelle la bille adverse doit être déplacée. Doit contenir une case vide</param>
        public bool DeplacerBilleAdverse(Position source, Position dest)
        {
            if (Etat == EtatPartie.Terminee) return false;
            if (LePlateau.EstCaseLibre(source)){
                return false; 
            }
            if (LePlateau.At(source)!.CouleurB == JoueurCourant.CouleurBilles)
            {
                return false;
            }
            return LePlateau.DeplacerBille(source, dest);
        }
        public bool AppuyerBoutonOrbito()
        {
            
            if (!LePlateau.ActionnerOrbite()) return false ;

            if (EstTerminee())
            {
                FinDePartie();
                return true;
            }

            ChangerJoueurCourant();
            CompteurTours++;
            return true;
        }
        public void FinDePartie()
        {
            JoueurGagnant = VerifierVictoire();
            Etat = EtatPartie.Terminee;


        }
        public string AffichagePlateau()
        {
            return LePlateau.ToString();
        }
    }
}
