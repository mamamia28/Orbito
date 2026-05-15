using LibOrbito;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitoTests
{
    public class HistoriqueTests
    {
        [Fact]
        public void Test_AjouterPartie_AjoutePartieEtExtraitJoueursUniques()
        {
            // Arrange
            var historique = new Historique();
            var partie = new Partie("Alice", "a", "Bob", "b", ModeJeu.HumainVsHumain, DifficulteIA.Facile);

            // Act
            historique.AjouterPartie(partie);

            // Assert
            Assert.Equal(1, historique.NbParties);
            Assert.Equal(2, historique.lesJoueurs.Count);
        }

        [Fact]
        public void Test_RecupererHistorique_FiltreBienLesPartiesDuJoueur()
        {
            // Arrange
            var historique = new Historique();
            var partie1 = new Partie("Alice", "a", "Bob", "b", ModeJeu.HumainVsHumain, DifficulteIA.Facile);
            var partie2 = new Partie("Charlie", "c", "Bob", "b", ModeJeu.HumainVsHumain, DifficulteIA.Facile);

            historique.AjouterPartie(partie1);
            historique.AjouterPartie(partie2);

            // Act
            var partiesAlice = historique.RecupererHistorique("Alice");
            var partiesBob = historique.RecupererHistorique("Bob");

            // Assert
            Assert.Single(partiesAlice);
            Assert.Equal(2, partiesBob.Count);
        }

        [Fact]
        public void Test_AjouterPartie_NeFaitRienSiPartieNulle()
        {
            // Arrange
            var historique = new Historique();

            // Act
            historique.AjouterPartie(null!);

            // Assert
            Assert.Equal(0, historique.NbParties);
            Assert.Empty(historique.lesJoueurs);
        }
    }
}
