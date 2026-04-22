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
                throw new IndexOutOfRangeException();
            }

            return TabJeu[p.Y, p.X] == null;
        }


        public void AjouterBille(Bille b, Position p)
        {
            if (!EstCaseLibre(p))
            {
                throw new InvalidProgramException("La case demandée a être occupée est déjà occupée");
            }
            else
            {
                TabJeu[p.Y, p.X] = b;
            }
        }


        public void DeplacerBille(Position origine, Position destination)
        {
            if (origine == destination)
            {
                return;
            }
            else if (EstCaseLibre(origine))
            {
                return;
            }
            else if (!EstCaseLibre(destination))
            {
                return;
            }
            TabJeu[destination.Y, destination.X] = TabJeu[origine.Y, origine.X];
            TabJeu[origine.Y, origine.X] = null;
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


        public void ActionnerOrbite()
        {
            Bille? tempExt = TabJeu[0, 0];

            TabJeu[0, 0] = TabJeu[0, 1];
            TabJeu[0, 1] = TabJeu[0, 2];
            TabJeu[0, 2] = TabJeu[0, 3];

            TabJeu[0, 3] = TabJeu[1, 3];
            TabJeu[1, 3] = TabJeu[2, 3];
            TabJeu[2, 3] = TabJeu[3, 3];

            TabJeu[3, 3] = TabJeu[3, 2];
            TabJeu[3, 2] = TabJeu[3, 1];
            TabJeu[3, 1] = TabJeu[3, 0];

            TabJeu[3, 0] = TabJeu[2, 0];
            TabJeu[2, 0] = TabJeu[1, 0];

            TabJeu[1, 0] = tempExt;

            Bille? tempInt = TabJeu[1, 1];

            TabJeu[1, 1] = TabJeu[1, 2];
            TabJeu[1, 2] = TabJeu[2, 2];
            TabJeu[2, 2] = TabJeu[2, 1];

            TabJeu[2, 1] = tempInt;
        }
    }
}


/*
Plateau p = new Plateau();

Bille b = new Bille(Couleur.Blanc);
Position pos = new Position(0, 0);
Position pos2 = new Position(1, 0);

p.AjouterBille(b, pos);



Console.WriteLine(p.EstCaseLibre(pos));

pos.X = 1;

Console.WriteLine(p.EstCaseLibre(pos));

pos.X = 0;

p.DeplacerBille(pos,pos2);

Console.WriteLine(p.EstCaseLibre(pos));

pos.X = 1;

Console.WriteLine(p.EstCaseLibre(pos));

Console.OutputEncoding = System.Text.Encoding.UTF8;


p.AjouterBille(new Bille(Couleur.Noir), new Position(3, 1));


p.AjouterBille(new Bille(Couleur.Blanc), new Position(2, 0));

p.AjouterBille(new Bille(Couleur.Noir), new Position(3, 0));

Console.WriteLine(p);

p.ActionnerOrbite();


Console.WriteLine(p); 
 
 
 */
