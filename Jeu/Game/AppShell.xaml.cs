namespace Game
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("PagePseudos", typeof(PagePseudos));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("ChoixMode", typeof(ChoixMode));
        }
    }
}
