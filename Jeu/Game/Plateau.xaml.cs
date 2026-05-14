namespace Game;

public partial class Plateau : ContentPage
{
	public Plateau()
	{
		InitializeComponent();
	}

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("FinDePartie");
    }
}