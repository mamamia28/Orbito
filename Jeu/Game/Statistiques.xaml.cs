using System.Collections.ObjectModel;

namespace Game;

public partial class Statistiques : ContentPage
{
    public List<JoueurStatsDisplay> ListeJoueurStats { get; set; } = new List<JoueurStatsDisplay>();
    public Statistiques()
    {
        InitializeComponent();
        ChargerStatistiques();
        BindingContext = this;
    }

    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void ChargerStatistiques()
    {
        LibOrbito.Historique histo = App.Hist;
        foreach (var joueur in histo.lesJoueurs)
        {
            var stats = histo.RecupererStats(joueur.Pseudo);

            ListeJoueurStats.Add(new JoueurStatsDisplay { Joueur = joueur, Stats = stats });
        }
    }

}

public class JoueurStatsDisplay
{
    public LibOrbito.Joueur Joueur { get; set; }
    public LibOrbito.Statistiques Stats { get; set; }
}
