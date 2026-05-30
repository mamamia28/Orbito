using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente une position en deux dimensions (X, Y) sur le plateau de jeu.
    /// </summary>
    [DataContract]
    public class Position
    {
        /// <summary>
        /// Obtient ou définit la coordonnée horizontale (colonne).
        /// </summary>
        [DataMember]
        public int X { get; set; }
        /// <summary>
        /// Obtient ou définit la coordonnée verticale (ligne).
        /// </summary>
        [DataMember]
        public int Y { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Position"/> avec les coordonnées spécifiées.
        /// </summary>
        /// <param name="x">La coordonnée horizontale.</param>
        /// <param name="y">La coordonnée verticale.</param>
        public Position(int x, int y)
        {

            X = x; 
            Y = y;
        }

        /// <summary>
        /// Vérifie si la position actuelle est adjacente (horizontalement ou verticalement) à une autre position.
        /// </summary>
        /// <param name="autre">L'autre position à comparer.</param>
        /// <returns><c>true</c> si les positions sont adjacentes, sinon <c>false</c>.</returns>
        public bool EstAdjacente(Position autre)
        {
            if (this.X == autre.X && this.Y == autre.Y)
            {
                return false;
            }
            if (this.X == autre.X)
            {
                return (autre.Y == this.Y - 1) || (autre.Y == this.Y + 1);
            }
            if (this.Y == autre.Y)
            {
                return (autre.X == this.X - 1) || (autre.X == this.X + 1);
            }
            return false;
        }
        /// <summary>
        /// Retourne une chaîne de caractères représentant la position.
        /// </summary>
        /// <returns>Une chaîne au format "X Y".</returns>
        public override string ToString()
        {
            return X.ToString() +" "+ Y.ToString();
        }
        
    }
}