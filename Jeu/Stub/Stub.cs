using System;
using LibOrbito;
using Action = LibOrbito.Action;

namespace Stub
{
    public class Stub : Historique
    {
        private IAFacile _cerveauFacile = new IAFacile();
        public Stub()
        {
            GenererDonnees();
        }

        private void GenererDonnees()
        { 
            for (int i = 0; i < 15; i++)
            {
                Partie p = new Partie("Bob", "bob.png", "Ali", "chat.png", ModeJeu.HumainVsHumain);
                SimulerPartie(p, _cerveauFacile, _cerveauFacile);
                this.AjouterPartie(p);
            }
            for (int i = 0; i < 10; i++)
            { 
                Partie p = new Partie("Bob", "bob.png", "IA", "IA.png", ModeJeu.HumainVsIA, DifficulteIA.Facile);
                SimulerPartie(p, _cerveauFacile, null);
                this.AjouterPartie(p);
            }
        }

        /// <summary>
        /// Joue une partie pour la stocker dans l'historique.
        /// </summary>
        private void SimulerPartie(Partie partie, IComportementIA cerveauJ1, IComportementIA? cerveauJ2)
        {
            while (!partie.EstTerminee() && partie.CompteurTours <= 21)
            {
                Action coup;
                if (partie.JoueurCourant.EstIA)
                {
                    coup = partie.JoueurCourant.ComportementIA!.CalculerCoup(partie);
                }
                else
                {
                    if (partie.JoueurCourant.CouleurBilles == Couleur.Blanc)
                    {
                        coup = cerveauJ1.CalculerCoup(partie);
                    }
                    else
                    {
                        coup = cerveauJ2!.CalculerCoup(partie);
                    }
                }

                if (coup.origineDeplacement != null && coup.destinationDeplacement != null)
                {
                    partie.DeplacerBilleAdverse(coup.origineDeplacement, coup.destinationDeplacement);
                }

                if (coup.placement != null)
                {
                    partie.PoserBille(coup.placement);
                }

                partie.AppuyerBoutonOrbito();
            }
        }
    }
}