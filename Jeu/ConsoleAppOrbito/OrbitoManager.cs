using LibOrbito;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppOrbito
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
                    PhaseTour.ChoixDeplacementAdverse => $"{nom}, choisissez une bille adverse à déplacer (ou passez).",
                    PhaseTour.DestinationDeplacement => "Choisissez la destination du déplacement.",
                    PhaseTour.PlacementBille => $"{nom}, placez l'une de vos billes.",
                    PhaseTour.AttenteBouton => "Appuyez sur le bouton Orbito pour terminer le tour.",
                    _=>""
                };
            }
        }
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


        public OrbitoManager(string nomJ1, string avatarJ1, string nomJ2, string avatarJ2, ModeJeu mode)
        {
            _partie = new Partie(nomJ1, avatarJ1, nomJ2, avatarJ2, mode);
            PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;

        }
        public void ActionnerBouton()
        {
            if (PhaseActuelle != PhaseTour.AttenteBouton) return;
            if (!_partie.AppuyerBoutonOrbito()) return;
            
            if (!_partie.EstTerminee())
            {
                if (_partie.CompteurTours <= 16)
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

    }

    public enum PhaseTour
    {
        ChoixDeplacementAdverse,
        DestinationDeplacement,
        PlacementBille,
        AttenteBouton
    }
}
