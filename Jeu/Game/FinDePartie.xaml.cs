namespace Game;

public partial class FinDePartie : ContentPage
{
	public FinDePartie()
	{
		InitializeComponent();
	}

    private async void OnMainClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    private async void OnAgainClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChoixMode");
    }
}