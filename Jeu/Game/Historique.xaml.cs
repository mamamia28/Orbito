namespace Game;
using LibOrbito;

public partial class Historique : ContentPage
{
    public LibOrbito.Historique? histo { get; set; } = App.Hist;
	public Historique()
	{
		InitializeComponent();
        BindingContext = App.Hist;
	}

    private async void OnRetourClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnRevoirPartieClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var partie = button?.CommandParameter as LibOrbito.PartieTerminee;

        if (partie != null)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                {"PartieARevoir", partie}
            };
            await Shell.Current.GoToAsync("ReplayPartie", navigationParameter);
        }
    }
}