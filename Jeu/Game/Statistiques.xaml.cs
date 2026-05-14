namespace Game;

public partial class Statistiques : ContentPage
{
	public Statistiques()
	{
		InitializeComponent();
	}

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}