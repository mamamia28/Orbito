using LibOrbito;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitoTests
{
    public class PlateauTests
    {
        [Fact]
        public void Test_RayonGet()
        {
            var p = new Plateau();

            Assert.Equal(2,p.Rayon);
        }
        [Fact]
        public void Test_CaseLibre_True() 
        {
            var p = new Plateau();

            Assert.True(p.EstCaseLibre(new Position(0, 0)));
        }

        [Fact]
        public void Test_CaseLibre_False()
        {
            var p = new Plateau();

            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));

            Assert.False(p.EstCaseLibre(new Position(0, 0)));
        }

        [Fact]
        public void Test_CaseLibreHorsPlateau_False()
        {
            var p = new Plateau();

            Assert.False(p.EstCaseLibre(new Position(p.TabJeu.GetLength(0)+2, p.TabJeu.GetLength(1) + 2)));
        }


        [Fact]
        public void Test_AjouterBille_True()
        {
            var p = new Plateau();

            var b = new Bille(Couleur.Blanc);

            

            Assert.True(p.AjouterBille(b, new Position(0, 0)));
        }


        [Fact]
        public void Test_AjouterBille_False()
        {
            var p = new Plateau();

            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));
            b.CouleurB = Couleur.Noir;


            Assert.False(p.AjouterBille(b, new Position(0, 0)));
        }


        [Fact]
        public void Test_DeplacerBille_Pareil_False()
        {
            var p = new Plateau();

            Assert.False(p.DeplacerBille(new Position(0,0), new Position(0, 0)));
        }


        [Fact]
        public void Test_DeplacerBille_True()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));


            Assert.True(p.DeplacerBille(new Position(0, 0), new Position(1, 0)));
            Assert.Equal(p.At(new Position(1, 0)), b);
        }

        [Fact]
        public void Test_DeplacerBille_OrigineLibre_False()
        {
            var p = new Plateau();
            

            Assert.False(p.DeplacerBille(new Position(0, 0), new Position(3, 0)));
        }

        [Fact]
        public void Test_DeplacerBille_DestOcc_False()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));
            p.AjouterBille(b, new Position(1, 0));


            Assert.False(p.DeplacerBille(new Position(1, 0), new Position(0, 0)));
        }


        [Fact]
        public void Test_AtReele_True()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));

            Assert.Equal(p.At(new Position(0, 0)), p.TabJeu[0, 0]);
        }

        [Fact]
        public void Test_AtNull_True()
        {
            var p = new Plateau();

            

            Assert.Null(p.At(new Position(0, 0)));
        }


        [Fact]
        public void Test_ToString_PlateauVide()
        {
            var p = new Plateau();

            string resultat = p.ToString();

            Assert.Contains("   0 1 2 3 ", resultat);

            Assert.Contains("0 ​✖️​✖️​✖️​✖️", resultat);
            Assert.Contains("1 ​✖️​✖️​✖️​✖️", resultat);
            Assert.Contains("2 ​✖️​✖️​✖️​✖️", resultat);
            Assert.Contains("3 ​✖️​✖️​✖️​✖️", resultat);
        }

        [Fact]
        public void Test_ToString_AvecBilles()
        {
            var p = new Plateau();


            var billeBlanche = new Bille(Couleur.Blanc);
            p.AjouterBille(billeBlanche, new Position(0, 0));

            var billeNoire = new Bille(Couleur.Noir);
            p.AjouterBille(billeNoire, new Position(1, 1));

            string resultat = p.ToString();

            Assert.Contains("0 ​⚪​​✖️​✖️​✖️", resultat);
            Assert.Contains("1 ​✖️​⚫​​✖️​✖️", resultat);
            Assert.Contains("2 ​✖️​✖️​✖️​✖️", resultat);
            Assert.Contains("3 ​✖️​✖️​✖️​✖️", resultat);
        }

        [Fact]
        public void Test_ActionnerOrbite_DeplacementStandard()
        {
            var p = new Plateau();
            var b1 = new Bille(Couleur.Blanc);
            var b2 = new Bille(Couleur.Noir);

            p.AjouterBille(b1, new Position(1, 0));
            p.AjouterBille(b2, new Position(2, 1));

            p.ActionnerOrbite();

            Assert.Equal(b1, p.At(new Position(0, 0)));
            Assert.Null(p.At(new Position(1, 0)));
            Assert.Equal(b2, p.At(new Position(1, 1)));
            Assert.Null(p.At(new Position(2, 1)));
        }

        [Fact]
        public void Test_ActionnerOrbite_Bouclage()
        {
            var p = new Plateau();
            var b1 = new Bille(Couleur.Blanc);
            var b2 = new Bille(Couleur.Noir);

            p.AjouterBille(b1, new Position(0, 0));
            p.AjouterBille(b2, new Position(1, 1));

            p.ActionnerOrbite();

            Assert.Equal(b1, p.At(new Position(0, 1)));
            Assert.Null(p.At(new Position(0, 0)));
            Assert.Equal(b2, p.At(new Position(1, 2)));
            Assert.Null(p.At(new Position(1, 1)));
        }

        [Fact]
        public void Test_ActionnerOrbite_PlateauVide()
        {
            var p = new Plateau();

            bool resultat = p.ActionnerOrbite();

            Assert.True(resultat);
            Assert.Null(p.At(new Position(0, 0)));
        }

        [Fact]
        public void Test_DetecterAlignement_Aucun()
        {
            var p = new Plateau();
            Assert.Equal((false, false), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_Ligne_Blanc()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));
            p.AjouterBille(b, new Position(1, 0));
            p.AjouterBille(b, new Position(2, 0));
            p.AjouterBille(b, new Position(3, 0));

            Assert.Equal((true, false), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_Colonne_Noir()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Noir);

            p.AjouterBille(b, new Position(1, 0));
            p.AjouterBille(b, new Position(1, 1));
            p.AjouterBille(b, new Position(1, 2));
            p.AjouterBille(b, new Position(1, 3));

            Assert.Equal((false, true), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_DiagonaleDescendante_Blanc()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Blanc);

            p.AjouterBille(b, new Position(0, 0));
            p.AjouterBille(b, new Position(1, 1));
            p.AjouterBille(b, new Position(2, 2));
            p.AjouterBille(b, new Position(3, 3));

            Assert.Equal((true, false), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_DiagonaleMontante_Noir()
        {
            var p = new Plateau();
            var b = new Bille(Couleur.Noir);

            p.AjouterBille(b, new Position(0, 3));
            p.AjouterBille(b, new Position(1, 2));
            p.AjouterBille(b, new Position(2, 1));
            p.AjouterBille(b, new Position(3, 0));

            Assert.Equal((false, true), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_LesDeuxGagnent()
        {
            var p = new Plateau();
            var bBlanc = new Bille(Couleur.Blanc);
            var bNoir = new Bille(Couleur.Noir);

            p.AjouterBille(bBlanc, new Position(0, 0));
            p.AjouterBille(bBlanc, new Position(1, 0));
            p.AjouterBille(bBlanc, new Position(2, 0));
            p.AjouterBille(bBlanc, new Position(3, 0));

            p.AjouterBille(bNoir, new Position(0, 1));
            p.AjouterBille(bNoir, new Position(1, 1));
            p.AjouterBille(bNoir, new Position(2, 1));
            p.AjouterBille(bNoir, new Position(3, 1));

            Assert.Equal((true, true), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_Rupture_2Billes()
        {
            var p = new Plateau();
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(0, 0));
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(1, 0));

            Assert.Equal((false, false), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_Rupture_3Billes()
        {
            var p = new Plateau();
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(0, 0));
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(1, 0));
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(2, 0));

            Assert.Equal((false, false), p.DetecterAlignement());
        }

        [Fact]
        public void Test_DetecterAlignement_Rupture_DerniereCouleurDifferente()
        {
            var p = new Plateau();
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(0, 0));
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(1, 0));
            p.AjouterBille(new Bille(Couleur.Blanc), new Position(2, 0));
            p.AjouterBille(new Bille(Couleur.Noir), new Position(3, 0));

            Assert.Equal((false, false), p.DetecterAlignement());
        }
    }
}
