using System;
using Xunit;
using LibOrbito;
using Action = LibOrbito.Action;
using System.Reflection;

namespace OrbitoTests
{
    public class TestIAFacile
    {
        [Fact]
        public void CalculerCoup_Tours1_2_AucunDeplacement()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Facile);
            var ia = new IAFacile();

            var action = ia.CalculerCoup(partie);

            Assert.Null(action.origineDeplacement);
            Assert.Null(action.destinationDeplacement);
            Assert.NotNull(action.placement);
        }

        [Fact]
        public void CalculerCoup_SansBilleRestante_PlacementNull()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Facile);
            var ia = new IAFacile();

            var joueurCourant = partie.JoueurCourant;
            for (int i = 0; i < 8; i++) joueurCourant.placerBille();

            var action = ia.CalculerCoup(partie);

            Assert.Null(action.placement);
        }

        [Fact]
        public void CalculerCoup_PlateauRempli_PlacementNull()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Facile);
            var ia = new IAFacile();

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    partie.LePlateau.AjouterBille(new Bille(Couleur.Blanc), new Position(x, y));
                }
            }

            var action = ia.CalculerCoup(partie);
            Assert.Null(action.placement);
            Assert.Null(action.origineDeplacement);
        }

        [Fact]
        public void CalculerCoup_Tours3_16_DeplacementPossible()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Facile);
            var ia = new IAFacile();

            var fieldTour = typeof(Partie).GetField("_compteurTours", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldTour!.SetValue(partie, 3);

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(1, 1));

            bool foundDeplacement = false;
            for (int i = 0; i < 50; i++)
            {
                var action = ia.CalculerCoup(partie);
                if (action.origineDeplacement != null)
                {
                    foundDeplacement = true;
                    Assert.Equal(1, action.origineDeplacement.X);
                    Assert.Equal(1, action.origineDeplacement.Y);
                    Assert.NotNull(action.destinationDeplacement);
                    Assert.True(action.origineDeplacement.EstAdjacente(action.destinationDeplacement));
                    break;
                }
            }

            Assert.True(foundDeplacement);
        }
    }
}
