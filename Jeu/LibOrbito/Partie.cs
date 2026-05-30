using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente une partie d'Orbito en gérant l'état global, les tours, les joueurs et l'historique des coups.
    /// </summary>
    [DataContract]
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

        [DataMember]
        private ModeJeu _mode;
        /// <summary>
        /// Obtient ou définit le mode de jeu actuel.
        /// </summary>
        public ModeJeu Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        [DataMember]
        private EtatPartie _etat;
        /// <summary>
        /// Obtient ou définit l'état actuel de la partie.
        /// </summary>
        public EtatPartie Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        [DataMember]
        private int _compteurTours;
        /// <summary>
        /// Obtient le nombre de tours joués depuis le début de la partie.
        /// </summary>
        public int CompteurTours
        {
            get { return _compteurTours; }
            private set { _compteurTours = value; }
        }

        [DataMember]
        private List<Coup> _coupsJoues = new List<Coup>();
        /// <summary>
        /// Obtient la liste en lecture seule de tous les coups joués pendant la partie.
        /// </summary>
        public IReadOnlyList<Coup> CoupsJoues => _coupsJoues.AsReadOnly();
        [DataMember]
        private Position? _bufferOrigine = null;
        [DataMember]
        private Position? _bufferDestination = null;
        [DataMember]
        private Position? _bufferPlacement = null;

        /// <summary>
        /// Obtient ou définit la date de création de la partie.
        /// </summary>
        [DataMember]
        public DateTime DateDeCreation { get; set; } = DateTime.Now;
        /// <summary>
        /// Obtient l'instance du plateau associée à la partie.
        /// </summary>
        [DataMember]
        public Plateau LePlateau { get; private set; }

        /// <summary>
        /// Obtient le joueur numéro 1 (généralement les billes Blanches).
        /// </summary>
        [DataMember]
        public Joueur Joueur1 { get; private set; }
        /// <summary>
        /// Obtient le joueur numéro 2 (généralement les billes Noires).
        /// </summary>
        [DataMember]
        public Joueur Joueur2 { get; private set; }
        /// <summary>
        /// Obtient ou définit le joueur dont c'est actuellement le tour.
        /// </summary>
        [DataMember]
        public Joueur JoueurCourant { get; set; }
        /// <summary>
        /// Obtient le joueur qui a remporté la partie (ou null si la partie est en cours / égalité).
        /// </summary>
        public Joueur? JoueurGagnant { get; private set; }
        /// <summary>
        /// Obtient le joueur qui a perdu la partie (ou null).
        /// </summary>
        public Joueur? JoueurPerdant { get; private set; }
        /// <summary>
        /// Obtient ou définit si la partie utilise la variante avec rotation inverse possible.
        /// </summary>
        [DataMember]
        public bool VarianteInverse { get; set; } = false;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Partie"/> avec les paramètres spécifiés.
        /// </summary>
        /// <param name="nomJ1">Nom du joueur 1.</param>
        /// <param name="avatarJ1">Avatar du joueur 1.</param>
        /// <param name="nomJ2">Nom du joueur 2.</param>
        /// <param name="avatarJ2">Avatar du joueur 2.</param>
        /// <param name="mode">Mode de jeu (Joueur Vs Joueur, etc.).</param>
        /// <param name="difficulte">Niveau de difficulté de l'IA (si un des joueurs est une IA).</param>
        public Partie(string nomJ1, string avatarJ1, string nomJ2, string avatarJ2, ModeJeu mode, DifficulteIA? difficulte = null)
        {
            Mode = mode;
            IComportementIA? GenererIA()  
            {
                switch (difficulte)
                {
                    case DifficulteIA.Facile:
                        return new IAFacile();
                    case DifficulteIA.Moyen:
                        return new IAMoyenne();   
                    
                }
                return null;   
            }

            IComportementIA? iaJ1 = (mode == ModeJeu.IAVsIA) ? GenererIA() : null;
            IComportementIA? iaJ2 = (mode == ModeJeu.HumainVsIA || mode == ModeJeu.IAVsIA) ? GenererIA() : null;

            Joueur1 = new Joueur(nomJ1, (iaJ1 == null) ? avatarJ1 : "ia.png", Couleur.Blanc, iaJ1);
            Joueur2 = new Joueur(nomJ2, (iaJ2 == null) ? avatarJ2 : "ia.png", Couleur.Noir, iaJ2);

            LePlateau = new Plateau();
            JoueurCourant = Joueur1;
            CompteurTours = 1;

            LePlateau.Initialiser();
            Etat = EtatPartie.EnCours;
        }



        /// <summary>
        /// Vérifie si les conditions de victoire sont remplies sur le plateau. Met à jour l'état de la partie en conséquence.
        /// </summary>
        /// <returns>Le joueur gagnant, ou null s'il n'y a pas de gagnant (en cours ou égalité).</returns>
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
                JoueurPerdant = (Joueur1.CouleurBilles == Couleur.Blanc) ? Joueur2 : Joueur1;
            }
            else
            {
                JoueurGagnant = (Joueur1.CouleurBilles == Couleur.Noir) ? Joueur1 : Joueur2;
                JoueurPerdant = (Joueur1.CouleurBilles == Couleur.Noir) ? Joueur2 : Joueur1;
            }

            return JoueurGagnant;
        }
        /// <summary>
        /// Vérifie si la partie est terminée (victoire ou égalité).
        /// </summary>
        /// <returns><c>true</c> si la partie est terminée, sinon <c>false</c>.</returns>
        public bool EstTerminee()
        {
            return Etat == EtatPartie.Terminee || Etat == EtatPartie.Egalite;
        }


        /// <summary>
        /// Change le joueur courant, passant le tour à l'adversaire.
        /// </summary>
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
        /// <summary>
        /// Déclenche la mécanique du bouton central Orbito, applique la rotation et enregistre le coup.
        /// </summary>
        /// <returns><c>true</c> si l'action s'est déroulée avec succès.</returns>
        /// <exception cref="InvalidOperationException">Levée si l'on tente d'actionner l'orbite sans avoir posé de bille.</exception>
        public bool AppuyerBoutonOrbito()
        {
            if (_bufferPlacement == null && CompteurTours <= 16)
            {
                throw new InvalidOperationException("Impossible d'actionner l'orbite sans avoir posé de bille.");
            }

            bool estRotationInverse = false;

            if (VarianteInverse)
            {
                estRotationInverse = new Random().Next(2) == 0;
                if (!LePlateau.ActionnerOrbite(estRotationInverse)) return false;
            }
            else
            {
                if (!LePlateau.ActionnerOrbite()) return false;
            }

            if (_bufferPlacement != null)
            {
                Coup nouveauCoup = new Coup(_bufferOrigine, _bufferDestination, _bufferPlacement, JoueurCourant, estRotationInverse);
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
        /// <summary>
        /// Retourne l'affichage console du plateau.
        /// </summary>
        /// <returns>Chaîne de caractères formatée.</returns>
        public string AffichagePlateau()
        {
            return LePlateau.ToString();
        }
        /// <summary>
        /// Vérifie si la position spécifiée contient une bille appartenant à l'adversaire du joueur courant.
        /// </summary>
        /// <param name="p">La position à vérifier.</param>
        /// <returns><c>true</c> si la case contient une bille adverse, sinon <c>false</c>.</returns>
        public bool EstBilleAdverse(Position p)
        {
            if (p.X < 0 || p.X > 3 || p.Y < 0 || p.Y > 3) return false;

            if (LePlateau.EstCaseLibre(p)) return false;

            return LePlateau.At(p)!.CouleurB != JoueurCourant.CouleurBilles;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_coupsJoues == null)
            {
                _coupsJoues = new List<Coup>();
            }
            if (LePlateau == null)
            {
                LePlateau = new Plateau();
                LePlateau.Initialiser();
            }
        }
    }
}
