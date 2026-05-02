using LibOrbito;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitoTests
{
    public class PositionTests
    {
        [Fact]
        public void Test_Adjacence_Bas_True()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(1, 2);

            Assert.True(refe.EstAdjacente(p1));

        }

        [Fact]
        public void Test_Adjacence_Haut_True()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(1, 0);

            Assert.True(refe.EstAdjacente(p1));

        }

        [Fact]
        public void Test_Adjacence_Gauche_True()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(0, 1);

            Assert.True(refe.EstAdjacente(p1));

        }

        [Fact]
        public void Test_Adjacence_Droite_True()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(2, 1);

            Assert.True(refe.EstAdjacente(p1));

        }

        [Fact]
        public void Test_Adjacence_DiagHaut_False()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(0, 0);

            Assert.False(refe.EstAdjacente(p1));

        }

        [Fact]
        public void Test_Adjacence_Opposé_False()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(3, 3);

            Assert.False(refe.EstAdjacente(p1));

        }


        [Fact]
        public void Test_Adjacence_Pareil_False()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(1, 1);

            Assert.False(refe.EstAdjacente(p1));

        }

        [Fact]
        public void Test_Adjacence_DiagBas_False()
        {
            var refe = new Position(1, 1);
            var p1 = new Position(2,2);

            Assert.False(refe.EstAdjacente(p1));

        }


    }
}
