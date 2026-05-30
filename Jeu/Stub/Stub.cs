using System;
using LibOrbito;
using Action = LibOrbito.Action;

namespace Stub
{
    public static class StubGenerator
    {
        public static Historique CreerHistoriqueBouchon()
        {
            Historique historique = new Historique();
            IAFacile cerveauFacile = new IAFacile();

            for (int i = 0; i < 15; i++)
            {
                Partie p = new Partie("Bob", "chien.png", "Ali", "chat.png", ModeJeu.HumainVsHumain);
                SimulerPartie(p, cerveauFacile, cerveauFacile);
                historique.AjouterPartie(p);
            }
            for (int i = 0; i < 10; i++)
            { 
                Partie p = new Partie("Bob", "chien.png", "IA", "ia.png", ModeJeu.HumainVsIA, DifficulteIA.Facile);
                SimulerPartie(p, cerveauFacile, null);
                historique.AjouterPartie(p);
            }
            return historique;
        }

        /// <summary>
        /// Joue une partie pour la stocker dans l'historique.
        /// </summary>
        private static void SimulerPartie(Partie partie, IComportementIA cerveauJ1, IComportementIA? cerveauJ2)
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