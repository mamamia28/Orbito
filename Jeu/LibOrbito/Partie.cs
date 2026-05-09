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

        private List<Coup> _coupsJoues = new List<Coup>();
        public IReadOnlyList<Coup> CoupsJoues => _coupsJoues.AsReadOnly();

        private Position? _bufferOrigine = null;
        private Position? _bufferDestination = null;
        private Position? _bufferPlacement = null;
        public DateTime DateDeCreation { get; } = DateTime.Now;
        private Plateau LePlateau { get; set; }

        private Joueur Joueur1 { get; set; }
        private Joueur Joueur2 { get; set; }
        public Joueur JoueurCourant { get; set; }
        public Joueur? JoueurGagnant { get; private set; }



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
            var (blancGagne, noirGagne) = LePlateau.DetecterAlignement();

            if (blancGagne && noirGagne)
            {
                Etat = EtatPartie.Egalite;
                return null;
            }

            if (!blancGagne && !noirGagne)
            {
                if (CompteurTours >= 21)
                {
                    Etat = EtatPartie.Egalite;
                }
                return null;
            }

            Etat = EtatPartie.Terminee;

            if (blancGagne)
            {
                JoueurGagnant = (Joueur1.CouleurBilles == Couleur.Blanc) ? Joueur1 : Joueur2;
            }
            else
            {
                JoueurGagnant = (Joueur1.CouleurBilles == Couleur.Noir) ? Joueur1 : Joueur2;
            }

            return JoueurGagnant;
        }

        public bool EstTerminee()
        {
            return Etat == EtatPartie.Terminee || Etat == EtatPartie.Egalite;
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
            if (!LePlateau.EstCaseLibre(p)) return false;

            try
            {
                JoueurCourant.placerBille();
            }
            catch (Exception)
            {
                return false;
            }
            if (LePlateau.AjouterBille(new Bille(JoueurCourant.CouleurBilles), p))
            {
                _bufferPlacement = p;
                return true;
            }
            return false;
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
            if (!source.EstAdjacente(dest)) return false;

            if (LePlateau.EstCaseLibre(source))
            {
                return false;
            }
            if (LePlateau.At(source)!.CouleurB == JoueurCourant.CouleurBilles)
            {
                return false;
            }
            if (LePlateau.DeplacerBille(source, dest))
            {
                _bufferOrigine = source;
                _bufferDestination = dest;
                return true;
            }
            return false;
        }
        public bool AppuyerBoutonOrbito()
        {
            if (_bufferPlacement == null && CompteurTours <= 16)
            {
                throw new InvalidOperationException("Impossible d'actionner l'orbite sans avoir posé de bille.");
            }

            if (!LePlateau.ActionnerOrbite()) return false;

            if (_bufferPlacement != null)
            {
                Coup nouveauCoup = new Coup(_bufferOrigine, _bufferDestination, _bufferPlacement, JoueurCourant);
                _coupsJoues.Add(nouveauCoup);
            }

            _bufferOrigine = null;
            _bufferDestination = null;
            _bufferPlacement = null;

            Joueur? gagnant = VerifierVictoire();

            if (EstTerminee())
            {
                return true;
            }

            ChangerJoueurCourant();
            CompteurTours++;
            return true;
        }

        public string AffichagePlateau()
        {
            return LePlateau.ToString();
        }

        public bool EstBilleAdverse(Position p)
        {
            if (p.X < 0 || p.X > 3 || p.Y < 0 || p.Y > 3) return false;

            if (LePlateau.EstCaseLibre(p)) return false;

            return LePlateau.At(p)!.CouleurB != JoueurCourant.CouleurBilles;
        }
    }
}
