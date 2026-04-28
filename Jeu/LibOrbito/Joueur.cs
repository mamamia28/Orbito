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
            set
            {
                _pseudo = value;
            }
        }

        private Couleur _couleurBilles;

        public Couleur CouleurBilles
        {
            get
            {
                return _couleurBilles;
            }
            set
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
            set
            {
                _billesRestantes = value;
            }
        }

        private bool _estIA;

        public bool EstIa
        {
            get
            {
                return _estIA;
            }
            set
            {
                _estIA = value;
            }
        }

        public Joueur(string pseudo, Couleur couleur, bool estIa)
        {
            CouleurBilles = couleur;
            EstIa = estIa;
            BillesRestantes = 8;
            if (EstIa == true)
            {
                Pseudo = "Joueur IA";
            } else
            {
                Pseudo = pseudo;
            }
        }

        public void placerBille(Position pos)
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
