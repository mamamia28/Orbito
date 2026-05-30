using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    [DataContract]
    public class Historique
    {

        public float TauxVictoireHumainContreIA => CalculerTauxVictoireHumainContreIA();

        [DataMember]
        public List<Joueur> lesJoueurs { get; set; } = new List<Joueur>();
        [DataMember]
        private List<PartieTerminee> _lesSessions = new List<PartieTerminee>();

        public IReadOnlyList<PartieTerminee> LesParties => _lesSessions.AsReadOnly();
        public int NbParties => _lesSessions.Count;
        public void AjouterPartie(Partie partie)
        {
            if (partie != null)
            {
                PartieTerminee archive = new PartieTerminee(partie);

                _lesSessions.Insert(0, archive);
                ActualiserListeJoueurs(archive);
            }
        }
        public List<PartieTerminee> RecupererHistorique(string joueur)
        {
            List<PartieTerminee> resultat = new List<PartieTerminee>();

            foreach (var partie in _lesSessions)
            {
                if (EstPresentDansPartie(partie, joueur))
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
                if (EstPresentDansPartie(partie, joueur))
                {
                    total++;
                    if (partie.JoueurGagnant != null)
                    {
                        if (partie.JoueurGagnant.Pseudo == joueur)
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
        private bool EstPresentDansPartie(PartieTerminee p, string pseudo)
        {
            Joueur j1 = p.Joueur1;
            Joueur j2 = p.Joueur2;

            return (j1 != null && j1.Pseudo == pseudo) || (j2 != null && j2.Pseudo == pseudo);
        }

        private void AjouterSiExistant(Joueur j)
        {
            bool existe = false;
            foreach (var joueurExistant in lesJoueurs)
            {
                if (joueurExistant.Pseudo == j.Pseudo)
                {
                    existe = true;
                    joueurExistant.Avatar = j.Avatar;
                    break;
                }
            }
            if (!existe) lesJoueurs.Add(j);
        }
        private void ActualiserListeJoueurs(PartieTerminee p)
        {
            if (p.Joueur1 != null) AjouterSiExistant(p.Joueur1);
            if (p.Joueur2 != null) AjouterSiExistant(p.Joueur2);
        }

        private float CalculerTauxVictoireHumainContreIA()
        {
            int totalPartiesIA = 0;
            int victoiresHumain = 0;

            foreach (var partie in _lesSessions)
            {
                if (partie.Mode == ModeJeu.HumainVsIA)
                {
                    totalPartiesIA++;
                    if (partie.JoueurGagnant != null && partie.JoueurGagnant.ComportementIA == null)
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
            sb.AppendLine($"- Nombre total de parties : {NbParties}");
            sb.AppendLine($"- Joueurs enregistrés : {lesJoueurs.Count}");
            sb.AppendLine($"- Taux de victoire (Humain vs IA) : {TauxVictoireHumainContreIA:F2}%");
            return sb.ToString();
        }
    }
}