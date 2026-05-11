using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LibOrbito
{
    public class OrbitoManager
    {
        private readonly Partie _partie;
        private Position? _positionSource;
        public PhaseTour PhaseActuelle;
        public string MessagePhase
        {
            get
            {
                string nom = _partie.JoueurCourant.Pseudo;
                if (_partie.CompteurTours > 16 && PhaseActuelle == PhaseTour.AttenteBouton)
                {
                    int rotationsRestantes = 22 - _partie.CompteurTours; 
                    return $"Plateau plein ! {nom}, actionnez l'orbite ({rotationsRestantes} rotations restantes).";
                }
                return PhaseActuelle switch
                {
                    PhaseTour.ChoixDeplacementAdverse => $"{nom}, choisissez une bille adverse à déplacer (ou passez (4 4)).",
                    PhaseTour.DestinationDeplacement => "Choisissez la destination du déplacement.",
                    PhaseTour.PlacementBille => $"{nom}, placez l'une de vos billes.",
                    PhaseTour.AttenteBouton => "Appuyez sur le bouton Orbito pour terminer le tour.(Entrée)",
                    _=>""
                };
            }
        }
        public event Action<string>? OnActionIADetaillee;
        public string MessageEtat
        {
            get
            {
                return _partie.Etat.ToString();
            }
        }

        public string MessageErreur { get; private set; } = "";
        public string NomJoueur { get => _partie.JoueurCourant.Pseudo; }

        public string NomGagnant { get => _partie.JoueurGagnant?.Pseudo ?? "Aucun"; }

        public string AffichagePlateau
        {
            get => _partie.AffichagePlateau();
        }

        public bool EstTerminee()
        {
            return _partie.EstTerminee();
        }

        public bool EstTourIA => _partie.JoueurCourant.EstIA;

        /// <summary>
        /// Initialise une nouvelle instance de la classe OrbitoManager pour une partie entre deux joueurs avec les
        /// avatars et le mode de jeu spécifiés.
        /// Note : La phase du tour est initialisée au placement car impossible de déplacer une bille adverse au premier tour
        /// </summary>
        /// <param name="nomJ1">Le nom du premier joueur. Ne peut pas être null ou vide.</param>
        /// <param name="avatarJ1">L'avatar associé au premier joueur. Ne peut pas être null ou vide.</param>
        /// <param name="nomJ2">Le nom du second joueur. Ne peut pas être null ou vide.</param>
        /// <param name="avatarJ2">L'avatar associé au second joueur. Ne peut pas être null ou vide.</param>
        /// <param name="mode">Le mode de jeu à utiliser pour la partie.</param>
        public OrbitoManager(string nomJ1, string avatarJ1, string nomJ2, string avatarJ2, ModeJeu mode, DifficulteIA? difficulte)
        {
            _partie = new Partie(nomJ1, avatarJ1, nomJ2, avatarJ2, mode, difficulte);
            PhaseActuelle = PhaseTour.PlacementBille;

        }
        public void ActionnerBouton()
        {
            if (PhaseActuelle != PhaseTour.AttenteBouton) return;
            if (!_partie.AppuyerBoutonOrbito()) return;

            if (!_partie.EstTerminee())
            {
                if (_partie.CompteurTours <= 2)
                {
                    PhaseActuelle = PhaseTour.PlacementBille;
                }
                else if (_partie.CompteurTours <= 16)
                {
                    PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
                }
                else
                {
                    PhaseActuelle = PhaseTour.AttenteBouton;
                }
            }
        }



        private void TraiterSelectionAdverse(Position p)
        {
            if (p.X < 0 || p.X > 3 || p.Y < 0 || p.Y > 3)
            {
                PhaseActuelle = PhaseTour.PlacementBille;
                MessageErreur = "";
                return;
            }

            if (_partie.EstBilleAdverse(p))
            {
                _positionSource = p;
                PhaseActuelle = PhaseTour.DestinationDeplacement;
                MessageErreur = "";
            }
            else
            {
                MessageErreur = "Sélection invalide : la case est vide ou contient votre propre bille.";
            }
        }

        private void TraiterDestinationAdverse(Position p)
        {
            if (_positionSource is null)
            {
                return;
            }
            if (_positionSource.X == p.X && _positionSource.Y == p.Y)
            {
                PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
                MessageErreur = "";
                _positionSource = null;
                return;
            }

            if (_partie.DeplacerBilleAdverse(_positionSource, p))
            {
                PhaseActuelle = PhaseTour.PlacementBille;
                MessageErreur = "";
                _positionSource = null;
            }
            else
            {
                PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
                MessageErreur = "Déplacement invalide. Veuillez resélectionner une bille adverse (ou -1 -1 pour passer).";
                _positionSource = null;
            }
        }
        private void TraiterPlacementBille(Position p)
        {
            if (p is null) return;
            if (_partie.PoserBille(p))
            {
                PhaseActuelle = PhaseTour.AttenteBouton;
                MessageErreur = ""; 
            }
            else
            {
                MessageErreur = "Action impossible : La case est déjà occupée ou invalide.";
            }
        }


        public void GererAction(int X, int Y) 
        {
            switch(PhaseActuelle)
            {
                case PhaseTour.ChoixDeplacementAdverse:
                    if (_partie.CompteurTours < 2) 
                    { 
                        PhaseActuelle = PhaseTour.PlacementBille;
                        return;
                    }
                    TraiterSelectionAdverse(new Position(X, Y));
                    break;
                case PhaseTour.DestinationDeplacement:
                    TraiterDestinationAdverse(new Position(X, Y));
                    break;
                case PhaseTour.PlacementBille:
                    TraiterPlacementBille(new Position(X, Y));
                    break;
                case PhaseTour.AttenteBouton:
                    break;
            }
        }
        public void FaireJouerIA()
        {
            if (!EstTourIA) return;

            OnActionIADetaillee?.Invoke($"{NomJoueur} réfléchit à sa stratégie...");
            Action coupIA = _partie.JoueurCourant.ComportementIA!.CalculerCoup(_partie);

            if (coupIA.origineDeplacement != null && coupIA.destinationDeplacement != null)
            {
                OnActionIADetaillee?.Invoke($"{NomJoueur} déplace une bille de ({coupIA.origineDeplacement.X}, {coupIA.origineDeplacement.Y}) vers ({coupIA.destinationDeplacement.X}, {coupIA.destinationDeplacement.Y}).");
                _partie.DeplacerBilleAdverse(coupIA.origineDeplacement, coupIA.destinationDeplacement);
            }
            else if (_partie.CompteurTours > 2 && _partie.CompteurTours <= 16)
            {
                OnActionIADetaillee?.Invoke($"{NomJoueur} décide de ne déplacer aucune bille.");
            }

            if (coupIA.placement != null)
            {
                OnActionIADetaillee?.Invoke($"{NomJoueur} pose une bille en ({coupIA.placement.X}, {coupIA.placement.Y}).");
                bool poseReussie = _partie.PoserBille(coupIA.placement);
                if (!poseReussie)
                {
                    MessageErreur = "L'IA a tenté un coup illégal.";
                    return;
                }
            }

            OnActionIADetaillee?.Invoke($"{NomJoueur} appuie sur le bouton Orbito !");
            _partie.AppuyerBoutonOrbito();

            if (!_partie.EstTerminee())
            {
                if (_partie.CompteurTours <= 2)
                {
                    PhaseActuelle = PhaseTour.PlacementBille;
                }
                else if (_partie.CompteurTours <= 16)
                {
                    PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
                }
                else
                {
                    PhaseActuelle = PhaseTour.AttenteBouton;
                }
            }
        }

    }

    public enum PhaseTour
    {
        ChoixDeplacementAdverse,
        DestinationDeplacement,
        PlacementBille,
        AttenteBouton
    }
}
