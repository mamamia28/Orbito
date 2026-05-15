using System;
using System.Collections.Generic;
using System.Text;
using LibOrbito;

namespace OrbitoTests
{
    public class StatistiquesTests
    {
        [Fact]
        public void Test_Constructeur_Initialisation()
        {
            // Arrange 
            int partiesJouees = 10;
            int victoires = 6;
            int defaites = 3;

            // Act
            var stats = new Statistiques(partiesJouees, victoires, defaites);

            // Assert
            Assert.Equal(partiesJouees, stats.PartiesJouees);
            Assert.Equal(victoires, stats.Victoires);
            Assert.Equal(defaites, stats.Defaites);
        }

        [Fact]
        public void Test_Constructeur_CalculTauxVictoire()
        {
            // Arrange
            int partiesJouees = 10;
            int victoires = 5;
            int defaites = 3;

            // Act
            var stats = new Statistiques(partiesJouees, victoires, defaites);

            // Assert
            Assert.Equal(50, stats.TauxVictoire);
        }

        [Fact]
        public void Test_Constructeur_CalculTauxSansParties()
        {
            // Arrange
            int partiesJouees = 0;
            int victoires = 0;
            int defaites = 0;

            // Act
            var stats = new Statistiques(partiesJouees, victoires, defaites);
            
            // Assert
            Assert.Equal(0, stats.TauxVictoire);
        }

        [Fact]
        public void Test_ToString()
        {
            // Arrange
            int partiesJouees = 10;
            int victoires = 4;
            int defaites = 4;
            var stats = new Statistiques(partiesJouees, victoires, defaites);

            // Act
            string resultat = stats.ToString();

            // Assert
            string attendu = "Stats : 10 parties -> 4 Victoires / 4 Defaites / 2 Egalité -> 40% Taux de Victoire";
            Assert.Equal(attendu, resultat);
        }
    }
}
