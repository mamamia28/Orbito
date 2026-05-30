using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Gère l'historique des parties terminées, les joueurs enregistrés et permet de calculer des states globales.
    /// </summary>
    [DataContract]
    public class Historique
    {
        /// <summary>
        /// Taux de partie gagnée par des Humains contre des IA
        /// </summary>
        public float TauxVictoireHumainContreIA => CalculerTauxVictoireHumainContreIA();

        /// <summary>
        /// Liste de tous les joueurs ayant lancé au moins une partie.
        /// </summary>
        [DataMember]
        public List<Joueur> lesJoueurs { get; set; } = new List<Joueur>();

        /// <summary>
        /// Liste interne de toutes les parties terminées.
        /// </summary>
        [DataMember]
        private List<PartieTerminee> _lesSessions = new List<PartieTerminee>();

        /// <summary>
        /// Liste en lecture seule de toutes les parties terminées.
        /// </summary>
        public IReadOnlyList<PartieTerminee> LesParties => _lesSessions.AsReadOnly();

        /// <summary>
        /// Nombre de parties enregistrées.
        /// </summary>
        public int NbParties => _lesSessions.Count;

        /// <summary>
        /// Ajoute une nouvelle partie terminée à l'historique et met à jour la liste des joueurs.
        /// </summary>
        /// <param name="partie">La partie terminée</param>
        public void AjouterPartie(Partie partie)
        {
            if (partie != null)
            {
                PartieTerminee archive = new PartieTerminee(partie);

                _lesSessions.Insert(0, archive);
                ActualiserListeJoueurs(archive);
            }
        }
        /// <summary>
        /// Récupère la liste de toutes les parties d'un joueur.
        /// </summary>
        /// <param name="joueur">Le pseudo du joueur recherché.</param>
        /// <returns>Une liste contenant les parties du joueur recherché.</returns>
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

        /// <summary>
        /// Calcul les statistiques d'un joueur (Victoires, Défaites, Taux de Victoire)
        /// </summary>
        /// <param name="joueur">Le pseudo du joueur dont on souhaite obtenir les statistiques.</param>
        /// <returns>Un objet "Statistiques" contenant les données du joueur.</returns>
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
        /// <summary>
        /// Vérifier si un joueur donné est présent dans une partie donnée.
        /// </summary>
        /// <param name="p">La partie à analyser</param>
        /// <param name="pseudo">Le pseudo du joueur </param>
        /// <returns>True si le joueur a participé à la partie, False sinon</returns>
        private bool EstPresentDansPartie(PartieTerminee p, string pseudo)
        {
            Joueur j1 = p.Joueur1;
            Joueur j2 = p.Joueur2;

            return (j1 != null && j1.Pseudo == pseudo) || (j2 != null && j2.Pseudo == pseudo);
        }

        /// <summary>
        /// Ajoute un joueur à la liste des Joueurs si il est pas connu et met à jour son Avatar
        /// </summary>
        /// <param name="j">Le Joueur à ajouter ou mettre à jour</param>
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

        /// <summary>
        /// Calcule le pourcentage de victoires des joueurs contre des IA
        /// </summary>
        /// <returns>Le taux de victoire (en pourcentage)</returns>
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