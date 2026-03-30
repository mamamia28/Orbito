namespace Game;

public partial class PagePseudos : ContentPage
{
    public PagePseudos()
    {
        InitializeComponent();
    }
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ChoixMode");
    }
}
