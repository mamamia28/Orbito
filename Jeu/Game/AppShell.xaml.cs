namespace Game
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("PageJoueurVSJoueur", typeof(PageJoueurVSJoueur));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("ChoixMode", typeof(ChoixMode));
            Routing.RegisterRoute("PageJoueurVSIA", typeof(PageJoueurVSIA));
            Routing.RegisterRoute("Historique", typeof(Historique));

        }
    }
}
