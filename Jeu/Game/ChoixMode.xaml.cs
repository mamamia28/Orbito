namespace Game;

public partial class ChoixMode : ContentPage
{
	public ChoixMode()
	{
		InitializeComponent();
	}

    private async void OnJoueurVSJoueurClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PageJoueurVSJoueur");
    }
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    private async void OnJoueurVSIAClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PageJoueurVSIA");
    }
}