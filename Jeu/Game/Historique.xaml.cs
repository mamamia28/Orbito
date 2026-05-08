namespace Game;

public partial class Historique : ContentPage
{
	public Historique()
	{
		InitializeComponent();
	}

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}