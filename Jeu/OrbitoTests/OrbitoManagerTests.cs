using LibOrbito;
using System;
using System.Collections.Generic;
using System.Text;

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
            public void ClicHorsPlateau_PasseEnPhasePlacement()
            {
                var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
                m.GererAction(0, 0);
                m.ActionnerBouton();

                m.GererAction(5, 5);

                Assert.Equal(PhaseTour.PlacementBille, m.PhaseActuelle);
            }

            [Fact]
            public void PlacementCaseOccupee_ResteEnPhasePlacement()
            {
                var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);
                m.GererAction(1, 1);
                m.ActionnerBouton();
                m.GererAction(2, 2);
                m.ActionnerBouton();

                m.GererAction(1, 1);

                Assert.Equal(PhaseTour.ChoixDeplacementAdverse, m.PhaseActuelle);
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
            public void DestinationNonAdjacente_EchoueEtRevientChoix()
            {
                var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);

                m.GererAction(0, 0);
                m.ActionnerBouton();
                m.GererAction(2, 2);
                m.ActionnerBouton();
                m.GererAction(2, 1);
                m.GererAction(0, 3);

                Assert.Equal(PhaseTour.ChoixDeplacementAdverse, m.PhaseActuelle);
            }

            [Fact]
            public void ActionnerBouton_SansPlacement_NeFaitRien()
            {
                var m = new OrbitoManager("J1", "A1", "J2", "A2", ModeJeu.HumainVsHumain, null);

                m.ActionnerBouton();

                Assert.Equal(PhaseTour.PlacementBille, m.PhaseActuelle);
                Assert.Equal(1, m._partie.CompteurTours);
            }
        }
    }


