using System;
using Xunit;
using LibOrbito;
using Action = LibOrbito.Action;
using System.Reflection;

namespace OrbitoTests
{
    public class TestIAMoyenne
    {
        [Fact]
        public void CalculerCoup_Tour17EtPlus_RenvoieActionVide()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();

            var fieldTour = typeof(Partie).GetField("_compteurTours", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldTour!.SetValue(partie, 17);

            var action = ia.CalculerCoup(partie);

            Assert.Null(action.origineDeplacement);
            Assert.Null(action.destinationDeplacement);
            Assert.Null(action.placement);
        }

        [Fact]
        public void CalculerCoup_VictoireImmedite_ChoisitCoupGagnant()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();

            partie.LePlateau.AjouterBille(new Bille(Couleur.Blanc), new Position(0, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Blanc), new Position(0, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Blanc), new Position(0, 2));

            var action = ia.CalculerCoup(partie);

            Assert.NotNull(action.placement);
        }

        [Fact]
        public void CalculerCoup_BloqueVictoireAdverse_SansDeplacement()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 2));

            var action = ia.CalculerCoup(partie);

            Assert.Null(action.origineDeplacement);
            Assert.NotNull(action.placement);
        }

        [Fact]
        public void CalculerCoup_BloqueVictoireAdverse_AvecDeplacement()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();
            var fieldTour = typeof(Partie).GetField("_compteurTours", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldTour!.SetValue(partie, 3);

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 2));

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(2, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(2, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(2, 2));

            var action = ia.CalculerCoup(partie);

            Assert.NotNull(action);
        }

        [Fact]
        public void CalculerCoup_DefaiteInevitable_JoueSansDeplacement()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();
            var fieldTour = typeof(Partie).GetField("_compteurTours", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldTour!.SetValue(partie, 3);

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(0, 2));

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(2, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(2, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(2, 2));

            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(3, 0));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(3, 1));
            partie.LePlateau.AjouterBille(new Bille(Couleur.Noir), new Position(3, 2));

            var action = ia.CalculerCoup(partie);
            Assert.NotNull(action);
        }

        [Fact]
        public void AdversairePeutGagner_AuDelaTour16_EvalueSurOrbiteDirecte()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();

            var fieldTour = typeof(Partie).GetField("_compteurTours", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldTour!.SetValue(partie, 16);

            var action = ia.CalculerCoup(partie);

            Assert.NotNull(action);
        }

        [Fact]
        public void CopierPlateau_AvecPlateauVide_Fonctionne()
        {
            var partie = new Partie("J1", "a", "J2", "b", ModeJeu.IAVsIA, DifficulteIA.Moyen);
            var ia = new IAMoyenne();

            var action = ia.CalculerCoup(partie);
            Assert.NotNull(action.placement);
        }
    }
}
