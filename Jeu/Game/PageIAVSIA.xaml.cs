using Game.Models;
using LibOrbito;

namespace Game;

[QueryProperty(nameof(VarianteInverse), "VarianteInverse")]
public partial class PageIAVSIA : ContentPage
{
    private SetupIAVsIaModel _viewModel;

    public bool VarianteInverse { get; set; }
    public PageIAVSIA()
	{
        _viewModel = new SetupIAVsIaModel();
        BindingContext = _viewModel;
		InitializeComponent();
	}


    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChoixMode");
    }

    private async void OnPlateauClicked(object sender, EventArgs e)
    {
        var parametres = new Dictionary<string, object>
        {
            { "Pseudo1", "IA"},
            { "Pseudo2", "IA" },
            { "Avatar1", "ia.png" },
            { "Avatar2", "ia.png" },
            { "Mode", ModeJeu.IAVsIA },
            { "Difficulte", _viewModel.DifficulteSelectionnee.ToString() },
            { "VarianteInverse", VarianteInverse }
        };

        await Shell.Current.GoToAsync(nameof(Plateau), parametres);
    }
}