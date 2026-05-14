namespace Game
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnPlayClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ChoixMode");
        }

        private async void OnHistoriqueClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Historique");
        }

        private async void OnStatistiquesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Statistiques");
        }
    }
}
