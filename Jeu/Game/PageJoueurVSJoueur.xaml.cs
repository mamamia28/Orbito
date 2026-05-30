using LibOrbito;
using Game.Models;

namespace Game;

[QueryProperty(nameof(VarianteInverse), "VarianteInverse")]
public partial class PageJoueurVSJoueur : ContentPage
{
    private SetupVsHumainModel _viewModel;

    public bool VarianteInverse { get; set; }
    public PageJoueurVSJoueur()
    {
        InitializeComponent();
        _viewModel = new SetupVsHumainModel();
        this.BindingContext = _viewModel;
    }

    private async void OnPlateauClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Pseudo1) || string.IsNullOrWhiteSpace(_viewModel.Pseudo2))
        {
            await DisplayAlertAsync("Erreur", "Les deux joueurs doivent avoir un pseudo.", "OK");
            return;
        }

        var parametres = new Dictionary<string, object>
        {
            { "Pseudo1", _viewModel.Pseudo1 },
            { "Pseudo2", _viewModel.Pseudo2 },
            { "Avatar1", _viewModel.AvatarSelectionne.Fichier },
            { "Avatar2", _viewModel.AvatarSelectionne2.Fichier },
            { "Mode", ModeJeu.HumainVsHumain },
            { "VarianteInverse", VarianteInverse }
        };

        await Shell.Current.GoToAsync(nameof(Plateau), parametres);
    }

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}