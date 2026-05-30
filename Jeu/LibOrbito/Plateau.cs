using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Swift;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente le plateau de jeu et contient la logique de placement, de déplacement et de rotation (orbite).
    /// </summary>
    [DataContract]
    public class Plateau
    {
        private Bille?[,]? _tabJeu;
        /// <summary>
        /// Obtient la grille bidimensionnelle représentant l'état actuel du plateau.
        /// </summary>
        public Bille?[,] TabJeu { get => _tabJeu!; }
        [DataMember(Name = "TabJeuSauvegarde")]
        private Bille?[][]? _tabJeuSauvegarde;
        [DataMember]
        private int _rayon;
        /// <summary>
        /// Obtient le rayon défini pour le plateau (par défaut 2 pour Orbito).
        /// </summary>
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

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            if (_tabJeu != null)
            {
                _tabJeuSauvegarde = new Bille?[4][];
                for (int i = 0; i < 4; i++)
                {
                    _tabJeuSauvegarde[i] = new Bille?[4];
                    for (int j = 0; j < 4; j++)
                    {
                        _tabJeuSauvegarde[i][j] = _tabJeu[i, j];
                    }
                }
            }
        }
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            _tabJeu = new Bille?[4, 4];
            if (_tabJeuSauvegarde != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        _tabJeu[i, j] = _tabJeuSauvegarde[i][j];
                    }
                }
            }
        }
        /// <summary>
        /// Réinitialise le plateau de jeu en vidant toutes les cases.
        /// </summary>
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
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Plateau"/> avec un rayon spécifié.
        /// </summary>
        /// <param name="rayon">Le rayon du plateau.</param>
        public Plateau(int rayon = 2)
        {
            Rayon = rayon;
            Initialiser();
        }


        /// <summary>
        /// Vérifie si la case à la position indiquée est libre (vide).
        /// </summary>
        /// <param name="p">La position à vérifier.</param>
        /// <returns><c>true</c> si la case est vide et dans les limites, sinon <c>false</c>.</returns>
        public bool EstCaseLibre(Position p)
        {
            if (p.X < 0 || p.X >= TabJeu.GetLength(0) ||
                p.Y < 0 || p.Y >= TabJeu.GetLength(1))
            {
                return false;
            }
            return TabJeu[p.Y, p.X] == null;
        }

        /// <summary>
        /// Ajoute une bille à la position spécifiée si la case est libre.
        /// </summary>
        /// <param name="b">La bille à ajouter.</param>
        /// <param name="p">La position cible.</param>
        /// <returns><c>true</c> si l'ajout a réussi, sinon <c>false</c>.</returns>
        public bool AjouterBille(Bille b, Position p)
        {
            if (!EstCaseLibre(p))
            {
                
                return false;
            }
            
            TabJeu[p.Y, p.X] = b;
            return true;
            
        }


        /// <summary>
        /// Déplace une bille d'une position d'origine vers une position de destination.
        /// </summary>
        /// <param name="origine">La position de départ.</param>
        /// <param name="destination">La position d'arrivée.</param>
        /// <returns><c>true</c> si le déplacement a réussi, sinon <c>false</c>.</returns>
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
        /// <summary>
        /// Retourne la bille située à une position spécifique.
        /// </summary>
        /// <param name="p">La position cible.</param>
        /// <returns>La <see cref="Bille"/> si elle existe, ou <c>null</c> si la case est vide.</returns>
        public Bille? At(Position p)
        {
            return TabJeu[p.Y, p.X];
        }

        /// <summary>
        /// Retourne une représentation textuelle du plateau (pour la console).
        /// </summary>
        /// <returns>Une chaîne de caractères formatée du plateau.</returns>
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

        /// <summary>
        /// Déclenche la mécanique de l'orbite, faisant pivoter toutes les billes selon les anneaux du plateau.
        /// </summary>
        /// <param name="varianteInverse">Indique si la rotation doit se faire dans le sens inverse (variante du jeu).</param>
        /// <returns><c>true</c> si la rotation a été effectuée avec succès.</returns>
        public bool ActionnerOrbite(bool varianteInverse = false)
        {
            (int Y, int X)[][] orbites = {
                new[] { (0,0), (0,1), (0,2), (0,3), (1,3), (2,3), (3,3), (3,2), (3,1), (3,0), (2,0), (1,0) },
                new[] { (1,1), (1,2), (2,2), (2,1) }
            };

            if (varianteInverse)
            {
                foreach (var anneau in orbites)
                {
                    Bille? temp = TabJeu[anneau[^1].Y, anneau[^1].X];

                    for (int i = anneau.Length-1; i > 0; i--)
                    {
                        TabJeu[anneau[i].Y, anneau[i].X] = TabJeu[anneau[i-1].Y, anneau[i-1].X];
                    }

                    TabJeu[anneau[0].Y, anneau[0].X] = temp;
                }
            } 
            else
            {
                foreach (var anneau in orbites)
                {
                    Bille? temp = TabJeu[anneau[0].Y, anneau[0].X];

                for (int i = 0; i < anneau.Length - 1; i++)
                {
                    TabJeu[anneau[i].Y, anneau[i].X] = TabJeu[anneau[i + 1].Y, anneau[i + 1].X];
                }

                TabJeu[anneau[^1].Y, anneau[^1].X] = temp;
                }
            }
            return true;
        }
        /// <summary>
        /// Détecte s'il existe un alignement gagnant de 4 billes (horizontal, vertical ou diagonal).
        /// </summary>
        /// <returns>Un tuple contenant deux booléens : (VictoireBlanc, VictoireNoir).</returns>
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
