namespace Game
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("PageJoueurVSJoueur", typeof(PageJoueurVSJoueur));
            Routing.RegisterRoute("ChoixMode", typeof(ChoixMode));
            Routing.RegisterRoute("PageJoueurVSIA", typeof(PageJoueurVSIA));
            Routing.RegisterRoute("Historique", typeof(Historique));
            Routing.RegisterRoute("Statistiques", typeof(Statistiques));
            Routing.RegisterRoute("Plateau", typeof(Plateau));
            Routing.RegisterRoute("FinDePartie", typeof(FinDePartie));
            Routing.RegisterRoute("PageIAVSIA", typeof(PageIAVSIA));
            Routing.RegisterRoute("ReplayPartie", typeof(ReplayPartie));
        }
    }
}
