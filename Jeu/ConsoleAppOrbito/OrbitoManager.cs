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
        private PhaseTour phaseActuelle;
        private string MessageEtat
        {
            get
            {
                return "";
                //return ToString(_partie.Etat);
            }
        }


        public OrbitoManager(string nomJ1, string avatarJ1, string nomJ2, string avatarJ2, ModeJeu mode)
        {
            _partie = new Partie(nomJ1, avatarJ1, nomJ2, avatarJ2, mode);
            phaseActuelle = PhaseTour.ChoixDeplacementAdverse;

        }
        public void ActionnerBouton()
        {
            if (phaseActuelle != PhaseTour.AttenteBouton) return;
            if (!_partie.AppuyerBoutonOrbito()) return;
            
            if (!_partie.EstTerminee())
            {
                phaseActuelle = PhaseTour.ChoixDeplacementAdverse;
            }
        }



        private void TraiterSelectionAdverse(Position p)
        {
            _positionSource = p;
            phaseActuelle = PhaseTour.DestinationDeplacement;
        }
        private void TraiterDestinationAdverse(Position p)
        {
            if (_positionSource is null)
            {
                return;
            }
            if (_positionSource == p)
            {
                return;
            }
            if (_partie.DeplacerBilleAdverse(_positionSource, p))
            {
                phaseActuelle = PhaseTour.PlacementBille;
            }
            
            _positionSource = null;
        }
        private void TraiterPlacementBille(Position p)
        {
            if (p is null) return;
            if (_partie.PoserBille(p))
            {
                phaseActuelle = PhaseTour.AttenteBouton;
            }
            
        }


        public void GererAction(int X, int Y) 
        {
            switch(phaseActuelle)
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
