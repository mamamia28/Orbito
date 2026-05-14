namespace Game;

public partial class PageJoueurVSJoueur : ContentPage
{
    public PageJoueurVSJoueur()
    {
        InitializeComponent();
    }
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChoixMode");
    }

    private async void OnPlateauClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Plateau");
    }
}
