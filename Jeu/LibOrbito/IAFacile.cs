using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LibOrbito
{
    [DataContract]
    public class IAFacile : IComportementIA
    {
        [DataMember]
        private Random _rnd = new Random();

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
