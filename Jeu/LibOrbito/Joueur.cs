using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Joueur
    {
        private string _pseudo;
        public string Pseudo{
            get
            {
                return _pseudo;
            }
        }

        private string _avatar;

        public string Avatar
        {
            get
            {
                return _avatar;
            }
            set
            {
                _avatar = value;
            }
        }

        private int _billesRestantes;
        public int BillesRestantes
        {
            get
            {
                return _billesRestantes;
            }
        }

        private List<Billes> _sesBilles;

        public List<Billes> SesBilles
        {
            get
            {
                return _sesBilles;
            }
        }

        private Couleur _saCouleur;

        public Couleur SaCouleur
        {
            get
            {
                return _saCouleur;
            }
        }

        public Joueur(string pseudo, string avatar, Couleur couleur)
        {
            _pseudo= pseudo;
            _avatar= avatar;
            _billesRestantes=8;
            _sesBilles = new List<Billes>();
            _saCouleur = couleur;
        }

        public static bool PoserBille(Position p)
        {
            Billes[,] plateau = new Billes[4,4];
            if (p == null) return false;
            plateau[p.X,p.Y] = Billes.couleur((du joueur))
            return false;
        }

        public static bool DeplacerBilleAdverse(Position depart, Position destination)
        {
            Billes[,] plateau = new Billes[4, 4];
            if (depart == null && destination ==null) return false;
            var pos = plateau.(plateau.[destination.X][destination.Y]);
                 // = plateau.[depart.X][depart.Y];
            //plateau.Rayon[depart.X][depart.Y] =  
            return false;
        }

        public void ActionnerOrbito()
        {
            
        }
    }
}
