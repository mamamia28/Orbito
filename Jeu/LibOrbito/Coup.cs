using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente un coup complet joué par un joueur lors d'un tour. Hérite de <see cref="Action"/>.
    /// </summary>
    [DataContract]
    public class Coup : Action
    {
        /// <summary>
        /// Obtient le joueur responsable de l'exécution de ce coup.
        /// </summary>
        [DataMember]
        public Joueur Responsable { get; private set; }
        /// <summary>
        /// Obtient une valeur indiquant si la rotation de l'orbite a été effectuée dans le sens inverse (variante).
        /// </summary>
        public bool SensRotationInverse { get; private set; }
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Coup"/>.
        /// </summary>
        /// <param name="origine">La position d'origine du déplacement.</param>
        /// <param name="destination">La position de destination du déplacement.</param>
        /// <param name="placement">La position de placement de la bille.</param>
        /// <param name="responsable">Le joueur qui a joué le coup.</param>
        /// <param name="sensRotationInverse">Détermine si le bouton Orbito a tourné dans le sens inverse.</param>
        public Coup(Position? origine, Position? destination, Position? placement, Joueur responsable, bool sensRotationInverse = false) : base(origine,destination,placement)
        {
            this.Responsable = responsable;
            this.SensRotationInverse = sensRotationInverse;
        }
    }
}
