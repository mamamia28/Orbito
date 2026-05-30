using LibOrbito;

namespace Game;

public partial class ChoixMode : ContentPage
{
    private OrbitoManager _manager;
    private bool _varianteAleatoire = false;
	public ChoixMode()
	{
		InitializeComponent();
        _manager = new OrbitoManager("Joueur 1", "chat.png", "Joueur 2", "chat.png", ModeJeu.HumainVsHumain, null);
	}

    private async void OnJoueurVSJoueurClicked(object sender, EventArgs e)
    {
        var variante = new Dictionary<string, object> { { "VarianteInverse", _varianteAleatoire } };
        await Shell.Current.GoToAsync("PageJoueurVSJoueur",variante);
    }
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    private async void OnJoueurVSIAClicked(object sender, EventArgs e)
    {
        var variante = new Dictionary<string, object> { { "VarianteInverse", _varianteAleatoire } };
        await Shell.Current.GoToAsync("PageJoueurVSIA",variante);
    }
    private async void OnIAVSIAClicked(object sender, EventArgs e)
    {
        var variante = new Dictionary<string, object> { { "VarianteInverse", _varianteAleatoire } };
        await Shell.Current.GoToAsync("PageIAVSIA",variante);
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

    private void OnVarianteTapped(object sender, TappedEventArgs e)
    {
        _varianteAleatoire = !_varianteAleatoire;
        Color couleurViolet = Color.FromArgb("#7A1CAC");
        Color couleurBlanc = Colors.White;

        if(_manager != null)
        {
            _manager.varianteInverse = _varianteAleatoire;
        }

        if (_varianteAleatoire)
        {
            VarianteInverse.BackgroundColor = couleurBlanc;
            VarianteInverse.Stroke = couleurViolet;

            TitreVariante.TextColor = couleurViolet;
            DescVariante.TextColor = couleurViolet;
        }
        else
        {
            VarianteInverse.BackgroundColor = couleurViolet;
            VarianteInverse.Stroke = couleurBlanc;

            TitreVariante.TextColor = couleurBlanc;
            DescVariante.TextColor = couleurBlanc;
        }
    }
}