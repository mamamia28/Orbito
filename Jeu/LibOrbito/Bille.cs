using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente une bille placée sur le plateau de jeu.
    /// </summary>
    [DataContract]
    public class Bille
    {

        [DataMember(Name = "CouleurB")]
        private Couleur couleur;
        /// <summary>
        /// Obtient ou définit la couleur de la bille. Seules les couleurs Blanc et Noir sont autorisées.
        /// </summary>
        public Couleur CouleurB
        {
           get
            {
                return couleur;
            }
            set
            {
                if (value == Couleur.Blanc || value == Couleur.Noir)
                {
                    couleur = value;
                }
            }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Bille"/> avec la couleur spécifiée.
        /// </summary>
        /// <param name="c">La couleur de la bille.</param>
        public Bille(Couleur c)
        {
            CouleurB = c;
        }
    }
}
