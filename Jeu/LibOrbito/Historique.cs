using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    public class Historique
    {

        public float TauxVictoireHumainContreIA => CalculerTauxVictoireHumainContreIA();
        public List<Joueur> lesJoueurs { get; set; } = new List<Joueur>();
        private List<Partie> _lesSessions = new List<Partie>();
        public int NbParties => _lesSessions.Count;

        public void AjouterPartie(Partie partie)
        {
            if(partie != null)
            {
                _lesSessions.Add(partie);
                ActualiserListeJoueurs(partie);
            }
        }

        public List<Partie> RecupererHistorique(string joueur)
        {
            List<Partie> resultat = new List<Partie>();
            
            foreach (var partie in _lesSessions)
            {
                if(EstPresentDansPartie(partie, joueur))
                {
                    resultat.Add(partie);
                }
            }
            return resultat;
        }

        public Statistiques RecupererStats(string joueur)
        {
            int total = 0;
            int victoires = 0;
            int defaites = 0;

            foreach (var partie in _lesSessions)
            {
                if(EstPresentDansPartie(partie, joueur))
                {
                    total++;
                    if (partie.JoueurGagnant != null)
                    {
                        if(partie.JoueurGagnant.Pseudo == joueur)
                        {
                            victoires++;
                        }
                        else
                        {
                            defaites++;
                        }
                    }
                }
            }
            return new Statistiques(total, victoires, defaites);
        }

        private bool EstPresentDansPartie(Partie p, string pseudo)
        {
            Joueur j1 = p.JoueurCourant;
            p.ChangerJoueurCourant();
            Joueur j2 = p.JoueurCourant;
            p.ChangerJoueurCourant();

            return j1.Pseudo == pseudo || j2.Pseudo == pseudo;
        }

        private void AjouterSiExistant(Joueur j)
        {
            bool existe = false;
            foreach (var joueurExistant in lesJoueurs)
            {
                if(joueurExistant.Pseudo == j.Pseudo)
                {
                    existe = true;
                    break;
                }
            }
            if (!existe) lesJoueurs.Add(j);
        }

        private void ActualiserListeJoueurs(Partie p)
        {
            Joueur j1 = p.JoueurCourant;
            p.ChangerJoueurCourant();
            Joueur j2 = p.JoueurCourant;
            p.ChangerJoueurCourant();

            AjouterSiExistant(j1);
            AjouterSiExistant(j2);
        }

        private float CalculerTauxVictoireHumainContreIA()
        {
            int totalPartiesIA = 0;
            int victoiresHumain = 0;

            foreach(var partie in _lesSessions)
            {
                if(partie.Mode == ModeJeu.HumainVsIA)
                {
                    totalPartiesIA++;
                    if(partie.JoueurGagnant != null && !partie.JoueurGagnant.EstIa)
                    {
                        victoiresHumain++;
                    }
                }
            }
            if (totalPartiesIA == 0) return 0f;

            return (float)victoiresHumain / totalPartiesIA * 100f;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("HISTORIQUE Local - Orbito");
            sb.AppendLine($"-Nombre total de parties : {NbParties}");
            sb.AppendLine($"-Joueurs enregistrés : {lesJoueurs.Count}");
            sb.AppendLine($"-Taux de victoire (Humain vs IA) : {TauxVictoireHumainContreIA}%");
            return sb.ToString();
        }
    }
}
