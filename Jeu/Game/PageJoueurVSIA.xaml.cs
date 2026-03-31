namespace Game;

public partial class PageJoueurVSIA : ContentPage
{
	public PageJoueurVSIA()
	{
		InitializeComponent();
	}
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChoixMode");
    }
}