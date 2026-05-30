using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace LibOrbito
{
    [DataContract]
    public class IAMoyenne : IComportementIA
    {
        [DataMember]
        private Random _rnd = new Random();

        public Action CalculerCoup(Partie partie)
        {
            if (partie.CompteurTours >= 17)
            {
                return new Action(null, null, null);
            }

            Couleur maCouleur = partie.JoueurCourant.CouleurBilles;
            Couleur couleurAdverse = (maCouleur == Couleur.Blanc) ? Couleur.Noir : Couleur.Blanc;

            var allMoves = GenererTousLesCoupsLegaux(partie.LePlateau, maCouleur, couleurAdverse, partie.CompteurTours);

            foreach (var move in allMoves)
            {
                Plateau sim = CopierPlateau(partie.LePlateau);
                AppliquerCoup(sim, move, maCouleur);
                var (blancGagne, noirGagne) = sim.DetecterAlignement();
                
                bool jeGagne = (maCouleur == Couleur.Blanc && blancGagne && !noirGagne) ||
                               (maCouleur == Couleur.Noir && noirGagne && !blancGagne);
                               
                if (jeGagne)
                {
                    return move;
                }
            }

            var movesSansDeplacement = allMoves.Where(m => m.origineDeplacement == null).ToList();
            var movesAvecDeplacement = allMoves.Where(m => m.origineDeplacement != null).ToList();

            List<Action> safeMovesSansDeplacement = new List<Action>();
            foreach (var move in movesSansDeplacement)
            {
                Plateau sim = CopierPlateau(partie.LePlateau);
                AppliquerCoup(sim, move, maCouleur);
                if (!AdversairePeutGagner(sim, couleurAdverse, maCouleur, partie.CompteurTours + 1))
                {
                    safeMovesSansDeplacement.Add(move);
                }
            }

            if (safeMovesSansDeplacement.Count > 0)
            {
                return safeMovesSansDeplacement[_rnd.Next(safeMovesSansDeplacement.Count)];
            }
            if (movesAvecDeplacement.Count > 0)
            {
                List<Action> safeMovesAvecDeplacement = new List<Action>();
                foreach (var move in movesAvecDeplacement)
                {
                    Plateau sim = CopierPlateau(partie.LePlateau);
                    AppliquerCoup(sim, move, maCouleur);
                    if (!AdversairePeutGagner(sim, couleurAdverse, maCouleur, partie.CompteurTours + 1))
                    {
                        safeMovesAvecDeplacement.Add(move);
                    }
                }

                if (safeMovesAvecDeplacement.Count > 0)
                {
                    return safeMovesAvecDeplacement[_rnd.Next(safeMovesAvecDeplacement.Count)];
                }
            }

            if (movesSansDeplacement.Count > 0)
            {
                return movesSansDeplacement[_rnd.Next(movesSansDeplacement.Count)];
            }
            return allMoves[_rnd.Next(allMoves.Count)];
        }

        private bool AdversairePeutGagner(Plateau plateau, Couleur couleurAdverse, Couleur maCouleur, int tour)
        {
            if (tour >= 17)
            {
                Plateau sim = CopierPlateau(plateau);
                sim.ActionnerOrbite();
                var (b, n) = sim.DetecterAlignement();
                return (couleurAdverse == Couleur.Blanc && b && !n) || (couleurAdverse == Couleur.Noir && n && !b);
            }

            var allAdvMoves = GenererTousLesCoupsLegaux(plateau, couleurAdverse, maCouleur, tour);
            foreach (var move in allAdvMoves)
            {
                Plateau sim = CopierPlateau(plateau);
                AppliquerCoup(sim, move, couleurAdverse);
                var (b, n) = sim.DetecterAlignement();
                bool advGagne = (couleurAdverse == Couleur.Blanc && b && !n) || (couleurAdverse == Couleur.Noir && n && !b);
                if (advGagne)
                {
                    return true;
                }
            }
            return false;
        }

        private Plateau CopierPlateau(Plateau original)
        {
            Plateau copie = new Plateau(original.Rayon);
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Position p = new Position(x, y);
                    Bille? b = original.At(p);
                    if (b != null)
                    {
                        copie.AjouterBille(new Bille(b.CouleurB), p);
                    }
                }
            }
            return copie;
        }

        private void AppliquerCoup(Plateau plateau, Action action, Couleur couleur)
        {
            if (action.origineDeplacement != null && action.destinationDeplacement != null)
            {
                plateau.DeplacerBille(action.origineDeplacement, action.destinationDeplacement);
            }
            if (action.placement != null)
            {
                plateau.AjouterBille(new Bille(couleur), action.placement);
            }
            plateau.ActionnerOrbite();
        }

        private List<Action> GenererTousLesCoupsLegaux(Plateau plateau, Couleur couleurJoueur, Couleur couleurAdverse, int tour)
        {
            List<Action> actions = new List<Action>();
            
            List<(Position?, Position?)> deplacements = new List<(Position?, Position?)>();
            deplacements.Add((null, null)); 

            if (tour > 2 && tour <= 16)
            {
                var casesVides = ObtenirCasesVides(plateau);
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        Position source = new Position(x, y);
                        if (!plateau.EstCaseLibre(source) && plateau.At(source)!.CouleurB == couleurAdverse)
                        {
                            foreach (var dest in casesVides)
                            {
                                if (source.EstAdjacente(dest))
                                {
                                    deplacements.Add((source, dest));
                                }
                            }
                        }
                    }
                }
            }

            foreach (var dep in deplacements)
            {
                var casesVides = ObtenirCasesVides(plateau);
                
                if (dep.Item1 != null && dep.Item2 != null)
                {
                    casesVides.RemoveAll(p => p.X == dep.Item2.X && p.Y == dep.Item2.Y);
                    casesVides.Add(new Position(dep.Item1.X, dep.Item1.Y));
                }

                foreach (var placement in casesVides)
                {
                    actions.Add(new Action(dep.Item1, dep.Item2, placement));
                }
            }

            return actions;
        }

        private List<Position> ObtenirCasesVides(Plateau plateau)
        {
            var casesVides = new List<Position>();
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Position p = new Position(x, y);
                    if (plateau.EstCaseLibre(p))
                    {
                        casesVides.Add(p);
                    }
                }
            }
            return casesVides;
        }
    }
}
