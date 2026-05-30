using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente une action effectuée par un joueur, contenant potentiellement un déplacement et/ou un placement.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Coup))]
    public class Action
    {
        /// <summary>
        /// Obtient la position d'origine pour le déplacement d'une bille adverse (si applicable).
        /// </summary>
        [DataMember]
        public Position? origineDeplacement { get; private set; }
        /// <summary>
        /// Obtient la position de destination pour le déplacement d'une bille adverse (si applicable).
        /// </summary>
        [DataMember]
        public Position? destinationDeplacement { get; private set; }
        /// <summary>
        /// Obtient la position où une nouvelle bille a été placée (si applicable).
        /// </summary>
        [DataMember]
        public Position? placement { get; private set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Action"/>.
        /// </summary>
        /// <param name="origineDeplacement">La position d'origine de la bille déplacée.</param>
        /// <param name="destinationDeplacement">La position de destination de la bille déplacée.</param>
        /// <param name="placement">La position de la bille posée sur le plateau.</param>
        public Action(Position? origineDeplacement, Position? destinationDeplacement, Position? placement)
        {
            this.origineDeplacement = origineDeplacement;
            this.destinationDeplacement = destinationDeplacement;
            this.placement = placement;
        }
    }
}
