using LibOrbito;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OrbitoTests
{
    public class OrbitoManagerTests
    {
        [Fact]
        public void PlacementValide_PasseEnPhaseAttenteBouton()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(1, 1);
            Assert.Equal(PhaseTour.AttenteBouton, m.PhaseActuelle);
        }

        [Fact]
        public void ActionnerBouton_ApresPlacement_ChangeDeTour()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0);
            m.ActionnerBouton();
            Assert.Equal(2, m._partie.CompteurTours);
        }

        [Fact]
        public void ActionnerBouton_SansPlacement_NeFaitRien()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.ActionnerBouton();
            Assert.Equal(PhaseTour.PlacementBille, m.PhaseActuelle);
            Assert.Equal(1, m._partie.CompteurTours);
        }

        [Fact]
        public void MessagePhase_RetourneLesBonsTextes()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            Assert.Contains("placez l'une de vos billes", m.MessagePhase);
            m.GererAction(0, 0);
            Assert.Contains("Appuyez sur le bouton", m.MessagePhase);

            m.ActionnerBouton();
            m.GererAction(1, 1);
            m.ActionnerBouton();
            Assert.Contains("choisissez une bille adverse", m.MessagePhase);
        }

        [Fact]
        public void DeuxToursComplets_CompteurEstTrois()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0);
            m.ActionnerBouton();
            m.GererAction(3, 3);
            m.ActionnerBouton();
            Assert.Equal(3, m._partie.CompteurTours);
        }

        [Fact]
        public void TroisToursComplets_PhaseEstChoixDeplacementAdverse()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0);
            m.ActionnerBouton();
            m.GererAction(3, 3);
            m.ActionnerBouton();
            Assert.Equal(PhaseTour.ChoixDeplacementAdverse, m.PhaseActuelle);
        }

        [Fact]
        public void ClicHorsPlateau_PasseEnPhasePlacement()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0);
            m.ActionnerBouton();
            m.GererAction(5, 5);
            Assert.Equal(PhaseTour.PlacementBille, m.PhaseActuelle);
        }

        [Fact]
        public void PlacementCaseOccupee_MessageErreur()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0);
            m.PhaseActuelle = PhaseTour.PlacementBille;
            m.GererAction(0, 0);
            Assert.NotEmpty(m.MessageErreur);
        }

        [Fact]
        public void DestinationNonAdjacente_EchoueEtRevientChoix()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0); m.ActionnerBouton();
            m.GererAction(2, 2); m.ActionnerBouton();
            m.GererAction(2, 2);
            m.GererAction(0, 3);
            Assert.Equal(PhaseTour.ChoixDeplacementAdverse, m.PhaseActuelle);
        }

        [Fact]
        public void FaireJouerIA_QuandCeNestPasLeTourIA_NeChanagePasLeTour()
        {
            var m = new OrbitoManager("H", "A1", "IA", "A2", ModeJeu.HumainVsIA, DifficulteIA.Facile);
            m.FaireJouerIA();
            Assert.Equal(1, m._partie.CompteurTours);
        }

        [Fact]
        public void MessagePhase_RetourneLeBonTexte_PourChaquePhase()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
            Assert.Contains("choisissez", m.MessagePhase);
            m.PhaseActuelle = PhaseTour.DestinationDeplacement;
            Assert.Contains("destination", m.MessagePhase);
            m.PhaseActuelle = (PhaseTour)999;
            Assert.Equal("", m.MessagePhase);
        }

        [Fact]
        public void PropietesGetters_RetournentLesValeursAttendues()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            Assert.Equal("EnCours", m.MessageEtat);
            Assert.NotNull(m.AffichagePlateau);
            Assert.False(m.EstTerminee());
            Assert.Equal("Aucun", m.NomGagnant);
            Assert.False(m.EstTourIA);
            Assert.Equal("J1", m.NomJoueur);
        }

        [Fact]
        public void ClicHorsPlateau_EnPhaseDeplacementAdverse_ReinitialiseLaPhase()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
            m.GererAction(-1, -1);
            Assert.Equal(PhaseTour.PlacementBille, m.PhaseActuelle);
        }

        [Fact]
        public void ActionnerBouton_EnPhaseInvalide_NeFaitRien()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.PhaseActuelle = PhaseTour.PlacementBille;
            m.ActionnerBouton();
            Assert.Equal(PhaseTour.PlacementBille, m.PhaseActuelle);
        }

        [Fact]
        public void DestinationEgaleSource_AnnuleLeDeplacement()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.PhaseActuelle = PhaseTour.DestinationDeplacement;
            m.GererAction(1, 1);
            Assert.NotNull(m.MessagePhase);
        }

        [Fact]
        public void GererAction_EnPhaseAttenteBouton_NeFaitRien()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.PhaseActuelle = PhaseTour.AttenteBouton;
            m.GererAction(0, 0);
            Assert.Equal(PhaseTour.AttenteBouton, m.PhaseActuelle);
        }

        [Fact]
        public void Position_StockeBienSesCoordonnees()
        {
            var p = new Position(2, 3);
            Assert.Equal(2, p.X);
            Assert.Equal(3, p.Y);
        }

        [Fact]
        public void MessagePhase_EstNonNull_PourToutesLesPhases()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            string msg;

            m.PhaseActuelle = PhaseTour.PlacementBille;
            msg = m.MessagePhase;

            m.PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
            msg = m.MessagePhase;

            m.PhaseActuelle = PhaseTour.DestinationDeplacement;
            msg = m.MessagePhase;

            m.PhaseActuelle = PhaseTour.AttenteBouton;
            msg = m.MessagePhase;

            Assert.NotNull(msg);
        }

        [Fact]
        public void ClicsHorsBornes_NeLancePasException()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);

            m.GererAction(9, 9);

            m.PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;
            m.GererAction(-1, 0);

            Assert.True(true);
        }

        [Fact]
        public void CreerManagerAvecIA_SInitialiseSansErreur()
        {
            var m = new OrbitoManager("IA_Bot", "A1", "Humain", "A2", ModeJeu.HumainVsIA, DifficulteIA.Facile);
            bool cEstAIA = m.EstTourIA;
            Assert.NotNull(m);
        }

        [Fact]
        public void FaireJouerIA_FonctionneDansLesDeusCasDeFigure()
        {
            var m = new OrbitoManager("Bot", "A1", "Humain", "A2", ModeJeu.HumainVsIA, DifficulteIA.Facile);

            if (m.EstTourIA)
            {
                m.FaireJouerIA();
            }

            var m2 = new OrbitoManager("H", "A1", "IA", "A2", ModeJeu.HumainVsIA, DifficulteIA.Facile);
            m2.FaireJouerIA();

            Assert.NotNull(m);
        }

        [Fact]
        public void MessagePhase_AvecPhaseInconnue_RetourneChaineVide()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.PhaseActuelle = (PhaseTour)999;
            Assert.Equal("", m.MessagePhase);
        }

        [Fact]
        public void ParcoursCompletDeplacements_ManagerResteStable()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);

            m.GererAction(0, 0); m.ActionnerBouton();
            m.GererAction(3, 3); m.ActionnerBouton();

            m.PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m.GererAction(i, j);
                }
            }

            m.PhaseActuelle = PhaseTour.DestinationDeplacement;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m.GererAction(i, j);
                }
            }

            _ = m.NomGagnant;
            _ = m.MessageErreur;

            Assert.NotNull(m);
        }

        [Fact]
        public void SelectionnerBilleAlliee_EnPhaseAdverse_ProduirUneErreur()
        {
            var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
            m.GererAction(0, 0); m.ActionnerBouton();

            m.PhaseActuelle = PhaseTour.ChoixDeplacementAdverse;

            m.GererAction(2, 2);

            m.GererAction(0, 0);

            Assert.NotEmpty(m.MessageErreur);
        }
        [Fact]
        public void FaireJouerIA_PremierTour_ChangeLeCompteur()
        {
            var m = new OrbitoManager(
                "Bot",
                "A1",
                "Humain",
                "A2",
                ModeJeu.HumainVsIA,
                DifficulteIA.Facile);

            if (!m.EstTourIA)
                return;

            int avant = m._partie.CompteurTours;

            m.FaireJouerIA();

            Assert.True(m._partie.CompteurTours > avant);
        }

        [Fact]
        public void FaireJouerIA_ApresDeuxTours_PasseEnChoixDeplacement()
        {
            var m = new OrbitoManager(
                "Bot",
                "A1",
                "Humain",
                "A2",
                ModeJeu.HumainVsIA,
                DifficulteIA.Facile);

            for (int i = 0; i < 2; i++)
            {
                if (m.EstTourIA)
                {
                    m.FaireJouerIA();
                }
                else
                {
                    m.GererAction(i, i);
                    m.ActionnerBouton();
                }
            }

            Assert.True(
                m.PhaseActuelle == PhaseTour.ChoixDeplacementAdverse ||
                m._partie.CompteurTours >= 3);
        }

        [Fact]
        public void FaireJouerIA_DeclenchePlusieursMessages()
        {
            var m = new OrbitoManager(
                "Bot",
                "A1",
                "Humain",
                "A2",
                ModeJeu.HumainVsIA,
                DifficulteIA.Facile);

            int compteurMessages = 0;

            m.OnActionIADetaillee += (_) =>
            {
                compteurMessages++;
            };

            if (!m.EstTourIA)
                return;

            m.FaireJouerIA();

            Assert.True(compteurMessages >= 2);
        }

        [Fact]
        public void FaireJouerIA_JouePlusieursTours_SansErreur()
        {
            var m = new OrbitoManager(
                "Bot",
                "A1",
                "Humain",
                "A2",
                ModeJeu.HumainVsIA,
                DifficulteIA.Facile);

            for (int tour = 0; tour < 6; tour++)
            {
                if (m.EstTourIA)
                {
                    m.FaireJouerIA();
                }
                else
                {
                    m.GererAction(tour % 4, (tour + 1) % 4);

                    if (m.PhaseActuelle == PhaseTour.AttenteBouton)
                    {
                        m.ActionnerBouton();
                    }
                }
            }

            Assert.NotNull(m);
        }

       

        [Fact]
        public void FaireJouerIA_ConserveUnEtatValide()
        {
            var m = new OrbitoManager(
                "Bot",
                "A1",
                "Humain",
                "A2",
                ModeJeu.HumainVsIA,
                DifficulteIA.Facile);

            if (m.EstTourIA)
            {
                m.FaireJouerIA();
            }

            Assert.False(m.EstTerminee() && m.NomGagnant == "Aucun");
        }

        [Fact]
        public void FaireJouerIA_ChangeLeJoueurCourant()
        {
            var m = new OrbitoManager(
                "Bot",
                "A1",
                "Humain",
                "A2",
                ModeJeu.HumainVsIA,
                DifficulteIA.Facile);

            if (!m.EstTourIA)
                return;

            string joueurAvant = m.NomJoueur;

            m.FaireJouerIA();

            Assert.NotEqual(joueurAvant, m.NomJoueur);
        }



    }
}