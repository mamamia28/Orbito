using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LibOrbito
{
    /// <summary>
    /// Représente une intelligence artificielle de niveau facile.
    /// Ses choix sont principalement aléatoires : elle place une bille sur une case vide au hasard, 
    /// et possède une chance sur deux de déplacer au préalable une bille adverse vers une case adjacente.
    /// </summary>
    [DataContract]
    public class IAFacile : IComportementIA
    {
        /// <summary>
        /// Générateur de nombres pseudo-aléatoires utilisé pour les décisions de l'IA.
        /// </summary>
        [DataMember]
        private Random _rnd = new Random();

        /// <summary>
        /// Calcule et retourne le prochain coup à jouer pour l'IA en fonction de l'état actuel de la partie.
        /// </summary>
        /// <param name="partie">L'instance de la partie en cours contenant le plateau et l'état des joueurs.</param>
        /// <returns>Une instance de <see cref="Action"/> représentant le coup calculé (déplacement éventuel et placement de la bille).</returns>
        public Action CalculerCoup(Partie partie)
        {
            Plateau plateau = partie.LePlateau;
            Couleur maCouleur = partie.JoueurCourant.CouleurBilles;
            Couleur couleurAdverse = (maCouleur == Couleur.Blanc) ? Couleur.Noir : Couleur.Blanc;

            Position? origine = null;
            Position? destination = null;

            if (partie.CompteurTours > 2 && partie.CompteurTours <= 16 && _rnd.Next(2) == 0)
            {
                var deplacementsPossibles = ObtenirDeplacementsValides(plateau, couleurAdverse);

                if (deplacementsPossibles.Count > 0)
                {
                    var choix = deplacementsPossibles[_rnd.Next(deplacementsPossibles.Count)];
                    origine = choix.Item1;
                    destination = choix.Item2;
                }
            }

            Position? placement = null;
            var casesVides = ObtenirCasesVides(plateau);

            if (destination != null)
            {
                casesVides.RemoveAll(p => p.X == destination.X && p.Y == destination.Y);
                if (origine != null) casesVides.Add(origine);
            }

            if (casesVides.Count > 0 && partie.JoueurCourant.BillesRestantes > 0)
            {
                placement = casesVides[_rnd.Next(casesVides.Count)];
            }

            return new Action(origine, destination, placement);
        }
        /// <summary>
        /// Parcourt le plateau pour trouver et retourner la liste de toutes les cases actuellement libres.
        /// </summary>
        /// <param name="plateau">Le plateau de jeu à analyser.</param>
        /// <returns>Une liste de <see cref="Position"/> correspondant aux coordonnées des cases vides.</returns>
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
        /// <summary>
        /// Identifie tous les déplacements valides possibles pour les billes de l'adversaire vers des cases vides adjacentes.
        /// </summary>
        /// <param name="plateau">Le plateau de jeu à analyser.</param>
        /// <param name="couleurAdverse">La couleur des billes appartenant à l'adversaire.</param>
        /// <returns>Une liste de tuples contenant la position d'origine et la position de destination pour chaque déplacement valide trouvé.</returns>
        private List<(Position, Position)> ObtenirDeplacementsValides(Plateau plateau, Couleur couleurAdverse)
        {
            var deplacements = new List<(Position, Position)>();
            var casesVides = ObtenirCasesVides(plateau);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Position source = new Position(x, y);

                    if (!plateau.EstCaseLibre(source) && plateau.At(source)!.CouleurB == couleurAdverse)
                    {
                        foreach (var caseVide in casesVides)
                        {
                            if (source.EstAdjacente(caseVide))
                            {
                                deplacements.Add((source, caseVide));
                            }
                        }
                    }
                }
            }
            return deplacements;
        }
    }
}
