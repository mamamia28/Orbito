using LibOrbito;
using Game.Models;


namespace Game;

[QueryProperty(nameof(Pseudo1), "Pseudo1")]
[QueryProperty(nameof(Pseudo2), "Pseudo2")]
[QueryProperty(nameof(Avatar1), "Avatar1")]
[QueryProperty(nameof(Avatar2), "Avatar2")]
[QueryProperty(nameof(Mode), "Mode")]
[QueryProperty(nameof(Difficulte), "Difficulte")]
[QueryProperty(nameof(VarianteInverse), "VarianteInverse")]
public partial class Plateau : ContentPage
{
    private PlateauModel _viewModel;
    public string Pseudo1 { get; set; }
    public string Pseudo2 { get; set; }
    public string Avatar1 { get; set; }
    public string Avatar2 { get; set; }
    public ModeJeu Mode { get; set; }
    private DifficulteIA? _difficulteIa;
    public bool VarianteInverse { get; set; }
    public string Difficulte
    {
        set
        {
            if (Enum.TryParse<DifficulteIA>(value, out var diff))
            {
                _difficulteIa = diff;
            }
            else
            {
                _difficulteIa = null;
            }
        }
    }
    public Plateau()
	{
		InitializeComponent();
        _viewModel = new PlateauModel();
        _viewModel.OnOrbiteActive += OnBoutonOrbitoClicked;
        this.BindingContext = _viewModel;
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (!App.RepriseEnCours && string.IsNullOrEmpty(_viewModel.NomJoueurCourant))
        {
            _viewModel.InitialiserPartie(Pseudo1, Avatar1, Pseudo2, Avatar2, Mode, _difficulteIa, VarianteInverse);
        }
        else
        {
            App.RepriseEnCours = false;
            App.EstEnCoursDeChargement = true;
            _viewModel.InitialiserReprise();
            App.EstEnCoursDeChargement = false;
        }
    }

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        if (await DisplayAlertAsync("Quitter la partie", "Voulez-vous vraiment retourner au menu principal ? \nLa progression sera perdue.", "Confirmer", "Annuler"))
        {
            App.PartieActive = null;
            File.Delete(App.xmlSauvegarde);
            await Shell.Current.GoToAsync("///MainPage");
        }
    }

    private async void OnCoupsClicked(object sender, EventArgs e)
    {
        _viewModel.EstCoupsClicked = true;
    }
    private async void OnCloseCoupsClicked(object sender, EventArgs e)
    {
        _viewModel.EstCoupsClicked = false;
    }
    private async void BoutonPasser_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "IsVisible")
        {
            if (BoutonPasser.IsVisible)
            {
                await LancerClignotement();
            }
            else
            {
                Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(BoutonPasser);
                BoutonPasser.Opacity = 1;
            }
        }
    }

    private async Task LancerClignotement()
    {

        while (BoutonPasser.IsVisible)
        {
            await BoutonPasser.FadeToAsync(0.6, 1200);
            if (!BoutonPasser.IsVisible) break;
            await BoutonPasser.FadeToAsync(1.0, 1200);
        }
        BoutonPasser.Opacity = 1;
    }

    private void OnAideClicked(object sender, EventArgs e)
    {
        _viewModel.EstAideAffiche = true;
    }

    private void OnCloseAideClicked(object sender, EventArgs e)
    {
        _viewModel.EstAideAffiche = false;
    }

    public async Task TremblerPlateauAsync(int durationMs = 400, double intensity = 8)
    {
        Random random = new Random();
        int stepDelay = 15; 
        int steps = durationMs / stepDelay;

        for (int i = 0; i < steps; i++)
        {
            double offsetX = (random.NextDouble() * 2 - 1) * intensity;
            double offsetY = (random.NextDouble() * 2 - 1) * intensity;

            VuePlateau.TranslationX = offsetX;
            VuePlateau.TranslationY = offsetY;

            await Task.Delay(stepDelay);
        }

        VuePlateau.TranslationX = 0;
        VuePlateau.TranslationY = 0;
    }

    private async void OnBoutonOrbitoClicked()
    {
        await TremblerPlateauAsync(durationMs: 200, intensity: 5);
    }
}