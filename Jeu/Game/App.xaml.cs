using LibOrbito;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using System.Runtime.Serialization;
using System.Xml;
using OrbitoPersistance;

namespace Game
{
    public partial class App : Application
    {
        public static LibOrbito.Historique Hist { get; set; } = new LibOrbito.Historique();
        private static string xmlFile = Path.Combine(FileSystem.AppDataDirectory, "parties.xml");
        public static string xmlSauvegarde = Path.Combine(FileSystem.AppDataDirectory, "partie_en_cours.xml");
        public static LibOrbito.Partie? PartieActive { get; set; } = null;
        public static bool EstEnCoursDeChargement { get; set; } = false;
        public static bool RepriseEnCours { get; set; } = false;

        public App()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"\n\nChemin maui (xml) : {xmlFile}\n\n");
            Hist = Persistance.ChargerHistorique(xmlFile, Hist);
            ChargerPartieEnCours();
        }
        

        public static void SauvegarderPartieEnCours()
        {
            if (EstEnCoursDeChargement) return;
            Persistance.SauvegarderPartieEnCours(PartieActive, xmlSauvegarde);
        }

        public static void ChargerPartieEnCours()
        {
            PartieActive = Persistance.ChargerPartieEnCours(xmlSauvegarde);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            //return new Window(new AppShell());
            Window window = new Window(new AppShell());

            window.Destroying += (s, e) =>
            {
                SauvegarderHistorique();
            };

            window.Backgrounding += (s, e) =>
            {
                SauvegarderHistorique();
            };

            return window;
        }

        static void SauvegarderHistorique()
        {
            Persistance.SauvegarderHistorique(Hist, xmlFile);
        }

        
    }
}