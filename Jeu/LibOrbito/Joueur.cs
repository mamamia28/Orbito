using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Joueur
    {
        private string _pseudo = null!;
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

        private string _avatar = null!;
        public string Avatar
        {
            get => _avatar;
            private set
            {
                _avatar = value;
            }
        }

        private Couleur _couleurBilles;

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

        public IComportementIA? ComportementIA { get; private set; }
        public bool EstIA => ComportementIA != null;

        public Joueur(string pseudo, string avatar, Couleur couleur, IComportementIA? comportement = null)
        {
            CouleurBilles = couleur;
            ComportementIA = comportement;
            BillesRestantes = 8;
            Avatar = avatar;
            Pseudo = (comportement is not null) ? "IA" : pseudo;
        }

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
