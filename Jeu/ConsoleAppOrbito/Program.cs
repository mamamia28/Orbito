using System;
using System.Text;
using System.Threading;
using LibOrbito;
using Stub;
using Microsoft.Win32.SafeHandles;

namespace ConsoleAppOrbito
{
    class Program
    {
        static Historique historique = new Stub.Stub();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            bool quitterApp = false;
            while (!quitterApp)
            {
                Console.Clear();
                Console.WriteLine("   ACCUEIL ORBITO    ");
                Console.WriteLine("1. Jouer");
                Console.WriteLine("2. Statistiques");
                Console.WriteLine("3. Historique");
                Console.WriteLine("4. Quitter");
                Console.Write("\nChoix : ");

                string choix = Console.ReadLine() ?? "";
                if (choix == "1") MenuChoixMode();
                else if (choix == "2") MenuStats();
                else if (choix == "3") MenuHistorique();
                else if (choix == "4") quitterApp = true;
            }
        }

        static void MenuChoixMode()
        {
            Console.Clear();
            Console.WriteLine("    CHOIX MODE    ");
            Console.WriteLine("1. Humain VS Humain");
            Console.WriteLine("2. Humain VS IA");
            Console.WriteLine("3. IA VS IA");
            Console.WriteLine("4. Retour");

            string choix = Console.ReadLine() ?? "";
            if (choix == "1") LancerPartie(ModeJeu.HumainVsHumain, null);
            else if (choix == "2") MenuChoixDifficulteIA(ModeJeu.HumainVsIA);
            else if (choix == "3") MenuChoixDifficulteIA(ModeJeu.IAVsIA);
            else if (choix == "4") return;
            else MenuChoixMode();
        }
        static void MenuChoixDifficulteIA(ModeJeu mode)
        {
            Console.Clear();
            Console.WriteLine("    CHOIX DIFFICULTE    ");
            Console.WriteLine("1. Facile");
            Console.WriteLine("2. Moyen");
            Console.WriteLine("3. Retour");

            string choix = Console.ReadLine() ?? "";
            if (choix == "1") LancerPartie(mode, DifficulteIA.Facile);
            else if (choix == "2") LancerPartie(mode, DifficulteIA.Moyen);
            else if (choix == "3") return;
            else MenuChoixDifficulteIA(mode);
        }

        static void MenuStats()
        {
            Console.Clear();
            Console.WriteLine("    STATISTIQUES    ");
            Console.WriteLine("1. Stats Globales");
            Console.WriteLine("2. Stats des Joueurs");
            Console.WriteLine("3. Stats Joueur");
            Console.WriteLine("4. Retour");
            Console.Write("\nChoix : ");

            string choix = Console.ReadLine() ?? "";
            if (choix == "1") afficherStatsGlobales();
            else if (choix == "2") afficherStats();
            else if (choix == "3") afficherStatsJoueur();
            else if (choix == "3") return;
        }

        static void afficherStatsGlobales()
        {
            Console.Clear();
            Console.WriteLine("STATISTIQUES GLOBALES (LOCAL)\n");

            if(historique.NbParties <= 0)
            {
                Console.WriteLine("\nAucune partie enregistrée.");
            } 
            else
            {
                Console.WriteLine(historique.ToString());
            }

            Console.WriteLine("Appuyer sur Entrée pour revenir");
            Console.ReadLine();
        }
        static void afficherStats()
        {
            Console.Clear();
            Console.WriteLine("STATISTIQUES DES JOUEURS\n");

            if (historique.lesJoueurs.Count <= 0) 
            {
                Console.WriteLine("\nAucun joueur enregistrée.");
            } else
            {
                foreach (var j in historique.lesJoueurs)
                {
                    Console.Write($"- {j.Pseudo} ");
                    Statistiques stats = historique.RecupererStats(j.Pseudo);
                    Console.WriteLine(stats.ToString());
                }
            }

            Console.WriteLine("\nAppuyer sur Entrée pour revenir");
            Console.ReadLine();
        }

        static void afficherStatsJoueur()
        {
            Console.Clear();
            Console.Write("Entrez le pseudo du joueur : ");
            string pseudo = Console.ReadLine() ?? "";
            Statistiques stats = historique.RecupererStats(pseudo);
            Console.WriteLine();

            if(stats.PartiesJouees <= 0)
            {
                Console.WriteLine($"\nLe joueur {pseudo} n'a aucune partie enregistrée.");
            }
            else
            {
                Console.WriteLine($"STATISTIQUES DE {pseudo}");
                Console.WriteLine(stats.ToString());
            }

            Console.WriteLine("\nAppuyer sur Entrée pour revenir");
            Console.ReadLine();
        }

        static void MenuHistorique()
        {
            Console.Clear();
            Console.WriteLine("    HISTORIQUE    ");
            Console.WriteLine("1. Parties jouées");
            Console.WriteLine("2. 5 Dernieres parties jouées");
            Console.WriteLine("3. Parties joueur");
            Console.WriteLine("4. 5 Dernieres parties joueur");
            Console.WriteLine("5. Retour");
            Console.Write("\nChoix : ");

            string choix = Console.ReadLine() ?? "";
            if (choix == "1") AfficherParties();
            else if (choix == "2") AfficherDernieresParties();
            else if (choix == "3") AfficherPartiesJoueur();
            else if (choix == "4") AfficherDernieresPartiesJoueur();
            else if (choix == "5") return;
        }

        static void AfficherParties()
        {
            Console.Clear();
            Console.WriteLine("PARTIES JOUEES\n");
            var parties = historique.LesParties;

            if (parties.Count == 0)
            {
                Console.WriteLine("\nAucune partie enregistrée");
            }
            else
            {
                for (int i = parties.Count - 1; i >= 0; i--)
                {
                    AfficherLigneHistorique(parties[i]);
                }
            }

            Console.WriteLine("\nAppuyer sur Entrée pour revenir");
            Console.ReadLine();
        }

        static void AfficherDernieresParties()
        {
            Console.Clear();
            Console.WriteLine("5 DERNIERES PARTIES \n");
            var parties = historique.LesParties;

            if (parties.Count == 0)
            {
                Console.WriteLine("Aucune partie enregistrée.");
            }
            else
            {
                int aAfficher = Math.Min(5, parties.Count);
                for (int i = parties.Count - 1; i >= parties.Count - aAfficher; i--)
                {
                    AfficherLigneHistorique(parties[i]);
                }
            }

            Console.WriteLine("\nAppuyer sur Entrée pour revenir");
            Console.ReadLine();
        }

        static void AfficherPartiesJoueur()
        {
            Console.Clear();
            Console.Write("Entrez le pseudo du joueur : ");
            string pseudo = Console.ReadLine() ?? "";
            var parties = historique.RecupererHistorique(pseudo);
            Console.WriteLine();

            if (parties.Count == 0)
            {
                Console.WriteLine($"\nAucune partie trouvée pour {pseudo}.");
            }
            else
            {
                Console.WriteLine($"PARTIES DE {pseudo}");
                for (int i = parties.Count - 1; i >= 0; i--)
                {
                    AfficherLigneHistorique(parties[i]);
                }
            }

            Console.WriteLine("\nAppuyer sur Entrée pour revenir");
            Console.ReadLine();
        }
        static void AfficherDernieresPartiesJoueur()
        {
            Console.Clear();
            Console.Write("Entrez le pseudo : ");
            string pseudo = Console.ReadLine() ?? "";
            Console.WriteLine($"5 DERNIÈRES PARTIES DE {pseudo}\n");
            var parties = historique.RecupererHistorique(pseudo);

            if (parties.Count == 0)
            {
                Console.WriteLine($"\nAucune partie trouvée pour {pseudo}.");
            }
            else
            {

                int limite = parties.Count - 5;
                if (limite < 0)
                {
                    limite = 0;
                }

                for (int i = parties.Count - 1; i >= limite; i--)
                {
                    AfficherLigneHistorique(parties[i]);
                }
            }

            Console.WriteLine("\nAppuyer sur entrée pour revenir au menu principal ..");
            Console.ReadLine();
        }

        static void AfficherLigneHistorique(Partie p)
        {
            Joueur j1Inst = p.JoueurCourant;
            p.ChangerJoueurCourant();
            Joueur j2Inst = p.JoueurCourant;
            p.ChangerJoueurCourant();

            string j1 = (j1Inst.CouleurBilles == Couleur.Blanc) ? j1Inst.Pseudo : j2Inst.Pseudo;
            string j2 = (j1Inst.CouleurBilles == Couleur.Noir) ? j1Inst.Pseudo : j2Inst.Pseudo;

            string date = p.DateDeCreation.ToString("dd/MM");
            int nbCoups = p.CoupsJoues.Count;
            string mode = p.Mode.ToString();

            if (p.Etat == EtatPartie.Egalite)
            {
                Console.WriteLine($"{date} - Mode: {mode} - {nbCoups} coups - Égalité ({j1} et {j2})");
            }
            else if (p.JoueurGagnant != null)
            {
                string gagnant = p.JoueurGagnant.Pseudo;
                string perdant = (gagnant == j1) ? j2 : j1;

                Console.WriteLine($"{date} - Mode: {mode} - {nbCoups} coups - Gagnant: {gagnant} / Perdant: {perdant}");
            }
        }

        static void AfficherActionIA(string message)
        {
            Console.WriteLine($"\nIA : {message}");
            Thread.Sleep(1200);
        }
        static void LancerPartie(ModeJeu mode,DifficulteIA? difficulte)
        {
            bool modeHardcode = false;

            string pseudo1 = "";
            string pseudo2 = mode == ModeJeu.HumainVsHumain ? "" : "IA";

            if (!modeHardcode)
            {
                Console.WriteLine("=== BIENVENUE DANS ORBITO ===");
                if (mode != ModeJeu.IAVsIA)
                {
                    Console.Write("Nom du Joueur 1 (Blancs) : ");
                    pseudo1 = Console.ReadLine() ?? "Joueur 1";
                }
                if (mode == ModeJeu.HumainVsHumain)
                {
                    Console.Write("Nom du Joueur 2 (Noirs) : ");
                    pseudo2 = Console.ReadLine() ?? "Joueur 2";
                }
            }

            OrbitoManager manager = new OrbitoManager(pseudo1, "avatar1.png", pseudo2, "avatar2.png", mode, difficulte);
            manager.OnActionIADetaillee += AfficherActionIA;

            while (!manager.EstTerminee())
            {
                Console.Clear();
                Console.WriteLine("=== ORBITO ===");
                Console.WriteLine(manager.AffichagePlateau);
                Console.WriteLine(manager.MessagePhase);

                if (!string.IsNullOrEmpty(manager.MessageErreur))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERREUR] {manager.MessageErreur}");
                    Console.ResetColor();
                }

                if (manager.EstTourIA)
                {
                    manager.FaireJouerIA();
                    continue;
                }

                if (manager.PhaseActuelle == PhaseTour.AttenteBouton)
                {
                    Console.ReadLine();
                    manager.ActionnerBouton();
                }
                else
                {
                    Console.Write("> ");
                    string? input = Console.ReadLine();

                    try
                    {
                        string[] parts = input!.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length == 2)
                        {
                            int x = int.Parse(parts[0]);
                            int y = int.Parse(parts[1]);

                            manager.GererAction(x, y);
                        }
                        else
                        {
                            Console.WriteLine("Format invalide. Tapez X espace Y (ex: 1 2). [ENTRÉE] pour réessayer...");
                            Console.ReadLine();
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Saisie invalide. Vérifiez que vous entrez bien des chiffres. [ENTRÉE] pour réessayer...");
                        Console.ReadLine();
                    }
                }
            }

            Console.Clear();
            Console.WriteLine("=== FIN DE LA PARTIE ===");
            Console.WriteLine(manager.AffichagePlateau);
            historique.AjouterPartie(manager._partie);

            if (manager.MessageEtat == "Egalite")
            {
                Console.WriteLine("🤝 Match Nul ! Bravo !");
            }
            else
            {
                Console.WriteLine($"🏆 {manager.NomGagnant}, Partie {manager.MessageEtat} ! Bravo !");
            }

            Console.ReadLine();


        }
    }
}