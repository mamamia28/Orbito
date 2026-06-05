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

        private async void OnQuitterClicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlertAsync(
                "Quitter le jeu",
                "Voulez-vous vraiment fermer l'application ?",
                "Oui",
                "Non"
            );
            if (confirmation)
            {
                Application.Current?.Quit();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PopUpReprendre.IsVisible = (App.PartieActive != null && !App.PartieActive.EstTerminee());
        }

        private async void OnReprendreClicked(object sender, EventArgs e)
        {
            App.RepriseEnCours = true;
            await Shell.Current.GoToAsync("Plateau");
        }
        private async void OnCloseReprendreClicked(object sender, EventArgs e)
        {
            PopUpReprendre.IsVisible = false;
            App.PartieActive = null;
            File.Delete(App.xmlSauvegarde);
        }

        private void OnCreditsClicked(object sender, EventArgs e)
        {
            PopupCredits.IsVisible = true;
            BtnCredits.BackgroundColor = Colors.Black;
            BtnCredits.TextColor = Colors.White;
        }

        private void OnCloseCreditsClicked(object sender, EventArgs e)
        {
            PopupCredits.IsVisible = false;
            BtnCredits.BackgroundColor = Colors.White;
            BtnCredits.TextColor = Colors.Black;
        }
    }
}
