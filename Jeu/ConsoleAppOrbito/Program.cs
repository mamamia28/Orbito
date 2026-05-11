using System;
using System.Text;
using System.Threading;
using LibOrbito;

namespace ConsoleAppOrbito
{
    class Program
    {
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
            Console.WriteLine("2. Stats Joueur");
            Console.WriteLine("3. Retour");
            Console.Write("\nChoix : ");

            string choix = Console.ReadLine() ?? "";
            if (choix == "3") return;
        }

        static void MenuHistorique()
        {
            Console.Clear();
            Console.WriteLine("    HISTORIQUE    ");
            Console.WriteLine("1. Dernières parties");
            Console.WriteLine("2. Parties joueur");
            Console.WriteLine("3. Retour");
            Console.Write("\nChoix : ");

            string choix = Console.ReadLine() ?? "";
            if (choix == "3") return;
        }

        static void AfficherActionIA(string message)
        {
            Console.WriteLine($"\nIA : {message}");
            Thread.Sleep(1200);
        }
        static void LancerPartie(ModeJeu mode,DifficulteIA? difficulte)
        {
            bool modeHardcode = true;

            string pseudo1 = "Alice";
            string pseudo2 = mode == ModeJeu.HumainVsHumain ? "Bob" : "OrbitoBot (IA)";

            if (!modeHardcode)
            {
                Console.WriteLine("=== BIENVENUE DANS ORBITO ===");
                Console.Write("Nom du Joueur 1 (Blancs) : ");
                pseudo1 = Console.ReadLine() ?? "Joueur 1";
                Console.Write("Nom du Joueur 2 (Noirs) : ");
                pseudo2 = Console.ReadLine() ?? "Joueur 2";
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