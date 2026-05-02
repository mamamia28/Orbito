using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Swift;
using System.Text;

namespace LibOrbito
{
    public class Plateau
    {
        private Bille?[,]? _tabJeu;
        public Bille?[,] TabJeu { get => _tabJeu!; }

        private int _rayon;
        public int Rayon
        {
            get => _rayon;
            private set
            {
                if (value == 2)
                {
                    _rayon = value;
                }
            }
        }

        public void Initialiser()
        {
            _tabJeu = new Bille[4, 4];
            for (int i = 0; i < TabJeu.GetLength(0); i++)
            {
                for (int j = 0; j <TabJeu.GetLength(1); j++)
                {
                    TabJeu[i, j] = null;
                }
            }
        }

        public Plateau(int rayon = 2)
        {
            Rayon = rayon;
            Initialiser();
        }



        public bool EstCaseLibre(Position p)
        {
            if (p.X < 0 || p.X >= TabJeu.GetLength(0) ||
                p.Y < 0 || p.Y >= TabJeu.GetLength(1))
            {
                return false;
            }
            return TabJeu[p.Y, p.X] == null;
        }


        public bool AjouterBille(Bille b, Position p)
        {
            if (!EstCaseLibre(p))
            {
                
                return false;
            }
            
            TabJeu[p.Y, p.X] = b;
            return true;
            
        }


        public bool DeplacerBille(Position origine, Position destination)
        {
            if (origine.X == destination.X && origine.Y == destination.Y)
            {
                return false;
            }
            else if (EstCaseLibre(origine))
            {
                return false;
            }
            else if (!EstCaseLibre(destination))
            {
                return false;
            }
            TabJeu[destination.Y, destination.X] = TabJeu[origine.Y, origine.X];
            TabJeu[origine.Y, origine.X] = null;
            return true;
        }

        public Bille? At(Position p)
        {
            return TabJeu[p.Y, p.X];
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            
            sb.Append("   ");
            for (int col = 0; col < TabJeu.GetLength(1); col++)
            {
                sb.Append($"{col} ");
            }
            sb.AppendLine(" ");

            
            for (int i = 0; i < TabJeu.GetLength(0); i++)
            {
                sb.Append($"{i} "); 

                for (int j = 0; j < TabJeu.GetLength(1); j++)
                {
                    Bille? b = TabJeu[i, j];

                    if (b == null)
                    {
                        sb.Append("​✖️");
                    }
                    else
                    { 
                        string emoji = (b.CouleurB == Couleur.Blanc) ? "​⚪​" : "​⚫​";
                        sb.Append($"{emoji}");
                    }
                }
                sb.AppendLine(" "); 
            }

            return sb.ToString();
        }


        public bool ActionnerOrbite()
        {
            (int Y, int X)[][] orbites = {
        new[] { (0,0), (0,1), (0,2), (0,3), (1,3), (2,3), (3,3), (3,2), (3,1), (3,0), (2,0), (1,0) },
        new[] { (1,1), (1,2), (2,2), (2,1) }
    };

            foreach (var anneau in orbites)
            {
                Bille? temp = TabJeu[anneau[0].Y, anneau[0].X];

                for (int i = 0; i < anneau.Length - 1; i++)
                {
                    TabJeu[anneau[i].Y, anneau[i].X] = TabJeu[anneau[i + 1].Y, anneau[i + 1].X];
                }

                TabJeu[anneau[^1].Y, anneau[^1].X] = temp;
            }

            return true;
        }
        public (bool, bool) DetecterAlignement()
        {
            bool blancGagne = false, noirGagne = false;

            void Verifier(Bille? b1, Bille? b2, Bille? b3, Bille? b4)
            {
                if (b1 != null && b1.CouleurB == b2?.CouleurB && b1.CouleurB == b3?.CouleurB && b1.CouleurB == b4?.CouleurB)
                {
                    if (b1.CouleurB == Couleur.Blanc) blancGagne = true;
                    else noirGagne = true;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Verifier(TabJeu[i, 0], TabJeu[i, 1], TabJeu[i, 2], TabJeu[i, 3]);
                Verifier(TabJeu[0, i], TabJeu[1, i], TabJeu[2, i], TabJeu[3, i]);
            }

            Verifier(TabJeu[0, 0], TabJeu[1, 1], TabJeu[2, 2], TabJeu[3, 3]);
            Verifier(TabJeu[3, 0], TabJeu[2, 1], TabJeu[1, 2], TabJeu[0, 3]);

            return (blancGagne, noirGagne);
        }


    }
    
}
