using Game.Models;
using LibOrbito;

namespace Game;

[QueryProperty(nameof(VarianteInverse), "VarianteInverse")]
public partial class PageJoueurVSIA : ContentPage
{
    private SetupVsIaModel _viewModel;
    public bool VarianteInverse { get; set; }
    public PageJoueurVSIA()
	{
		InitializeComponent();
        _viewModel = new SetupVsIaModel();
        this.BindingContext = _viewModel;
    }
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChoixMode");
    }

    private async void OnPlateauClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Pseudo1))
        {
            await DisplayAlertAsync("Erreur", "Tu dois entrer un pseudo.", "OK");
            return;
        }

        var parametres = new Dictionary<string, object>
        {
            { "Pseudo1", _viewModel.Pseudo1 },
            { "Pseudo2", "IA" },
            { "Avatar1", _viewModel.AvatarSelectionne.Fichier },
            { "Avatar2", "ia.png" },
            { "Mode", ModeJeu.HumainVsIA },
            { "Difficulte", _viewModel.DifficulteSelectionnee.ToString() },
            { "VarianteInverse", VarianteInverse }
        };

        await Shell.Current.GoToAsync(nameof(Plateau), parametres);
    }
}