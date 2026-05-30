using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Représente un joueur (humain ou intelligence artificielle) participant à la partie.
    /// </summary>
    [KnownType(typeof(IAFacile))]
    [KnownType(typeof(IAMoyenne))]
    [DataContract(IsReference=true)]
    public class Joueur
    {
        [DataMember(Name = "Pseudo")]
        private string _pseudo = null!;

        /// <summary>
        /// Obtient le pseudonyme du joueur.
        /// </summary>
        public string Pseudo{
            get
            {
                return _pseudo;
            }
            private set
            {
                _pseudo = value;
            }
        }

        [DataMember(Name = "Avatar")]
        private string _avatar = null!;
        /// <summary>
        /// Obtient ou définit le chemin ou le nom de l'avatar du joueur.
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set
            {
                _avatar = value;
            }
        }

        
        private Couleur _couleurBilles;
        /// <summary>
        /// Obtient la couleur des billes assignée à ce joueur.
        /// </summary>
        [DataMember]
        public Couleur CouleurBilles
        {
            get
            {
                return _couleurBilles;
            }
            private set
            {
                _couleurBilles = value;
            }
        }

        
        private int _billesRestantes;
        /// <summary>
        /// Obtient le nombre de billes restantes que le joueur peut encore placer.
        /// </summary>
        [DataMember]
        public int BillesRestantes
        {
            get
            {
                return _billesRestantes;
            }
            private set
            {
                _billesRestantes = value;
            }
        }
        /// <summary>
        /// Obtient l'interface définissant le comportement de l'IA de ce joueur. Retourne null si le joueur est humain.
        /// </summary>
        [DataMember]
        public IComportementIA? ComportementIA { get; private set; }
        /// <summary>
        /// Obtient une valeur indiquant si le joueur est contrôlé par une intelligence artificielle.
        /// </summary>
        public bool EstIA => ComportementIA != null;
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Joueur"/>.
        /// </summary>
        /// <param name="pseudo">Le pseudonyme du joueur.</param>
        /// <param name="avatar">L'image de profil ou avatar du joueur.</param>
        /// <param name="couleur">La couleur de bille attribuée au joueur.</param>
        /// <param name="comportement">Le comportement d'IA associé, ou null pour un joueur humain.</param>
        public Joueur(string pseudo, string avatar, Couleur couleur, IComportementIA? comportement = null)
        {
            CouleurBilles = couleur;
            ComportementIA = comportement;
            BillesRestantes = 8;
            Avatar = avatar;
            Pseudo = (comportement is not null) ? "IA" : pseudo;
        }
        /// <summary>
        /// Retire une bille de la réserve du joueur lors d'un placement.
        /// </summary>
        /// <exception cref="Exception">Levée si le joueur tente de placer une bille alors qu'il n'en possède plus.</exception>
        public void placerBille()
        {
            if (_billesRestantes > 0)
            {
                _billesRestantes--;
            }
            else
            {
                throw new Exception("Ce joueur n'a plus de billes.");
            }
        }
    }
}
