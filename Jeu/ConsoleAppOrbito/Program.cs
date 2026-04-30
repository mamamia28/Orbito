using System;
using System.Text;
using ConsoleAppOrbito;
using LibOrbito;

namespace ConsoleAppOrbito
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = Encoding.UTF8;

            
            bool modeHardcode = true; 

            string pseudo1 = "Alice";
            string pseudo2 = "Bob";

            if (!modeHardcode)
            {
                Console.WriteLine("=== BIENVENUE DANS ORBITO ===");
                Console.Write("Nom du Joueur 1 (Blancs) : ");
                pseudo1 = Console.ReadLine() ?? "Joueur 1";
                Console.Write("Nom du Joueur 2 (Noirs) : ");
                pseudo2 = Console.ReadLine() ?? "Joueur 2";
            }

            OrbitoManager manager = new OrbitoManager(pseudo1, "avatar1.png", pseudo2, "avatar2.png", ModeJeu.HumainVsHumain);

            
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
                        Console.WriteLine("Test rentrée");
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
