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

    private void OnVariantesClicked(object sender, EventArgs e)
    {
        PopupVariantes.IsVisible = !PopupVariantes.IsVisible;

        if (PopupVariantes.IsVisible)
        {
            BtnVariantes.BackgroundColor = Colors.Purple;
            BtnVariantes.TextColor = Colors.White;
        } else
        {
            BtnVariantes.BackgroundColor = Colors.White;
            BtnVariantes.TextColor = Colors.Purple;
        }
    }

    private void OnCloseVariantesClicked(object sender, EventArgs e)
    {
        PopupVariantes.IsVisible = false;
        BtnVariantes.BackgroundColor = Colors.White;
        BtnVariantes.TextColor = Colors.Purple;
    }

    private void OnReglesClicked(object sender, EventArgs e)
    {
        PopupRegles.IsVisible = true;
        BtnRegles.BackgroundColor = Colors.Black;
        BtnRegles.TextColor = Colors.White;
    }

    private void OnCloseReglesClicked(object sender, EventArgs e)
    {
        PopupRegles.IsVisible = false;
        BtnRegles.BackgroundColor = Colors.White;
        BtnRegles.TextColor = Colors.Black;
    }
}