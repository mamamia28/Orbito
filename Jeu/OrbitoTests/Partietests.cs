using LibOrbito;
using System;
using Xunit;

namespace OrbitoTests
{
    public class PartieTests
    {
        [Fact]
        public void Test_Initialisation()
        {
            var partie = new Partie("J1", "avatar1", "J2", "avatar2", ModeJeu.HumainVsHumain);

            Assert.Equal(EtatPartie.EnCours, partie.Etat);
            Assert.Equal(1, partie.CompteurTours);
            Assert.Equal(Couleur.Blanc, partie.JoueurCourant.CouleurBilles);
            Assert.Equal(8, partie.JoueurCourant.BillesRestantes);
            Assert.False(partie.EstTerminee());
            Assert.Null(partie.JoueurGagnant);
        }

        [Fact]
        public void Test_ChangerJoueurCourant()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.ChangerJoueurCourant();

            Assert.Equal(Couleur.Noir, partie.JoueurCourant.CouleurBilles);
        }

        [Fact]
        public void Test_PoserBille_Valide_Et_DecrementeBille()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);
            var pos = new Position(0, 0);

            bool resultat = partie.PoserBille(pos);

            Assert.True(resultat);
            Assert.Equal(7, partie.JoueurCourant.BillesRestantes);
        }

        [Fact]
        public void Test_PoserBille_CaseOccupee_False()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);
            var pos = new Position(0, 0);

            partie.PoserBille(pos);
            bool resultat = partie.PoserBille(pos);

            Assert.False(resultat);
            Assert.Equal(7, partie.JoueurCourant.BillesRestantes);
        }

        [Fact]
        public void Test_DeplacerBilleAdverse_Valide()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.PoserBille(new Position(1, 1));
            partie.ChangerJoueurCourant();

            bool resultat = partie.DeplacerBilleAdverse(new Position(1, 1), new Position(1, 2));

            Assert.True(resultat);
        }

        [Fact]
        public void Test_DeplacerBilleAdverse_BillePropre_False()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.PoserBille(new Position(1, 1));

            bool resultat = partie.DeplacerBilleAdverse(new Position(1, 1), new Position(1, 2));

            Assert.False(resultat);
        }

        [Fact]
        public void Test_DeplacerBilleAdverse_NonAdjacente_False()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);
            partie.PoserBille(new Position(0, 0));
            partie.ChangerJoueurCourant();

            bool resultat = partie.DeplacerBilleAdverse(new Position(0, 0), new Position(2, 2));

            Assert.False(resultat);
        }

        [Fact]
        public void Test_AppuyerBoutonOrbito_PasseAuTourSuivant()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            bool resultat = partie.AppuyerBoutonOrbito();

            Assert.True(resultat);
            Assert.Equal(2, partie.CompteurTours);
            Assert.Equal(Couleur.Noir, partie.JoueurCourant.CouleurBilles);
        }

        [Fact]
        public void Test_VerifierVictoire_Egalite_TourMax()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            for (int i = 0; i < 21; i++)
            {
                partie.AppuyerBoutonOrbito();
            }

            Assert.Equal(EtatPartie.Egalite, partie.Etat);
            Assert.True(partie.EstTerminee());
            Assert.Null(partie.JoueurGagnant);
            Assert.Equal(ModeJeu.HumainVsHumain, partie.Mode);
        }

        [Fact]
        public void Test_EstBilleAdverse_VraiEtFaux()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);
            var pos = new Position(1, 1);

            Assert.False(partie.EstBilleAdverse(pos));

            partie.PoserBille(pos);

            Assert.False(partie.EstBilleAdverse(pos));

            partie.ChangerJoueurCourant();

            Assert.True(partie.EstBilleAdverse(pos));

            Assert.False(partie.EstBilleAdverse(new Position(5, 5)));
            Assert.False(partie.EstBilleAdverse(new Position(-1, 0)));
        }

        [Fact]
        public void Test_VerifierVictoire_DeuxGagnants()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.PoserBille(new Position(0, 0));
            partie.PoserBille(new Position(1, 0));
            partie.PoserBille(new Position(2, 0));
            partie.PoserBille(new Position(3, 0));

            partie.ChangerJoueurCourant();

            partie.PoserBille(new Position(0, 1));
            partie.PoserBille(new Position(1, 1));
            partie.PoserBille(new Position(2, 1));
            partie.PoserBille(new Position(3, 1));

            var gagnant = partie.VerifierVictoire();

            Assert.Null(gagnant);
            Assert.Equal(EtatPartie.Egalite, partie.Etat);
            Assert.True(partie.EstTerminee());
        }

        [Fact]
        public void Test_VerifierVictoire_AucunGagnant_EnCours()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            var gagnant = partie.VerifierVictoire();

            Assert.Null(gagnant);
            Assert.Equal(EtatPartie.EnCours, partie.Etat);
            Assert.False(partie.EstTerminee());
        }

        [Fact]
        public void Test_VerifierVictoire_BlancGagne()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.PoserBille(new Position(0, 0));
            partie.PoserBille(new Position(1, 0));
            partie.PoserBille(new Position(2, 0));
            partie.PoserBille(new Position(3, 0));

            var gagnant = partie.VerifierVictoire();

            Assert.NotNull(gagnant);
            Assert.Equal(Couleur.Blanc, gagnant.CouleurBilles);
            Assert.Equal(EtatPartie.Terminee, partie.Etat);
            Assert.True(partie.EstTerminee());
        }

        [Fact]
        public void Test_VerifierVictoire_NoirGagne()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.ChangerJoueurCourant();

            partie.PoserBille(new Position(0, 0));
            partie.PoserBille(new Position(1, 0));
            partie.PoserBille(new Position(2, 0));
            partie.PoserBille(new Position(3, 0));

            var gagnant = partie.VerifierVictoire();

            Assert.NotNull(gagnant);
            Assert.Equal(Couleur.Noir, gagnant.CouleurBilles);
            Assert.Equal(EtatPartie.Terminee, partie.Etat);
            Assert.True(partie.EstTerminee());
        }


        [Fact]
        public void Test_PoserBille_ExceptionCatch()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            

            partie.PoserBille(new Position(0, 0));
            partie.PoserBille(new Position(1, 0));
            partie.PoserBille(new Position(2, 0));
            partie.PoserBille(new Position(3, 0));
            partie.PoserBille(new Position(0, 1));
            partie.PoserBille(new Position(1, 1));
            partie.PoserBille(new Position(2, 1));
            partie.PoserBille(new Position(3, 1));

            Assert.False(partie.PoserBille(new Position(2, 2)));
        }

        [Fact]
        public void Test_AffichagePlateau()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);

            partie.PoserBille(new Position(0, 0));

            string resultat = partie.AffichagePlateau();

            Assert.NotNull(resultat);
            Assert.Contains("   0 1 2 3 ", resultat);
            Assert.Contains("0 ", resultat);
            Assert.Contains("1 ", resultat);
            Assert.Contains("2 ", resultat);
            Assert.Contains("3 ", resultat);
        }

        [Fact]
        public void Test_DeplacerBilleAdverse_BilleAbsenteSource_False()
        {
            var partie = new Partie("J1", "av1", "J2", "av2", ModeJeu.HumainVsHumain);


            bool resultat = partie.DeplacerBilleAdverse(new Position(1, 1), new Position(1, 2));

            Assert.False(resultat);

            partie.Etat = EtatPartie.Terminee;

            Assert.False(partie.PoserBille(new Position(1, 1)));
            Assert.False(partie.DeplacerBilleAdverse(new Position(1, 1), new Position(1, 2)));
        }
    }
}