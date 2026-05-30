using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace LibOrbito
{
    /// <summary>
    /// Représente une intelligence artificielle de niveau moyen.
    /// Contrairement à l'IA facile, cette IA analyse les coups : elle cherche d'abord un coup gagnant immédiat,
    /// puis évalue les réponses possibles de l'adversaire pour privilégier des coups "sécurisés" (qui ne permettent pas à l'adversaire de gagner au tour suivant).
    /// </summary>
    [DataContract]
    public class IAMoyenne : IComportementIA
    {
        /// <summary>
        /// Générateur de nombres pseudo-aléatoires utilisé pour départager les coups équivalents ou faire des choix par défaut.
        /// </summary>
        [DataMember]
        private Random _rnd = new Random();
        /// <summary>
        /// Calcule le meilleur coup possible en fonction de l'état actuel de la partie.
        /// </summary>
        /// <param name="partie">L'instance de la partie en cours.</param>
        /// <returns>Une instance de <see cref="Action"/> représentant le coup choisi.</returns>
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
        /// <summary>
        /// Simule le tour de l'adversaire pour déterminer s'il possède un coup gagnant à partir d'un état de plateau donné.
        /// </summary>
        /// <param name="plateau">L'état du plateau à tester.</param>
        /// <param name="couleurAdverse">La couleur de l'adversaire.</param>
        /// <param name="maCouleur">La couleur de l'IA.</param>
        /// <param name="tour">Le numéro du tour simulé.</param>
        /// <returns><c>true</c> si l'adversaire peut gagner au prochain tour ; sinon, <c>false</c>.</returns>
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
        /// <summary>
        /// Crée une copie profonde (clone) du plateau pour effectuer des simulations sans altérer l'état réel de la partie.
        /// </summary>
        /// <param name="original">Le plateau original à copier.</param>
        /// <returns>Une nouvelle instance de <see cref="Plateau"/> identique à l'original.</returns>
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
        /// <summary>
        /// Applique une action complète (déplacement optionnel, placement de bille et déclenchement de l'orbite) sur un plateau.
        /// </summary>
        /// <param name="plateau">Le plateau sur lequel appliquer le coup.</param>
        /// <param name="action">Le coup à jouer.</param>
        /// <param name="couleur">La couleur de la bille placée.</param>
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
        /// <summary>
        /// Génère de manière exhaustive toutes les actions légales (combinaisons de déplacements et de placements) pour un tour donné.
        /// </summary>
        /// <param name="plateau">L'état actuel du plateau.</param>
        /// <param name="couleurJoueur">La couleur du joueur simulant le coup.</param>
        /// <param name="couleurAdverse">La couleur de l'adversaire (pour identifier les billes déplaçables).</param>
        /// <param name="tour">Le numéro du tour actuel.</param>
        /// <returns>Une liste exhaustive de toutes les <see cref="Action"/> légales possibles.</returns>
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
        /// <summary>
        /// Parcourt le plateau pour identifier et retourner la liste de toutes les cases non occupées.
        /// </summary>
        /// <param name="plateau">Le plateau de jeu à analyser.</param>
        /// <returns>Une liste de <see cref="Position"/> représentant les coordonnées des cases vides.</returns>
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
