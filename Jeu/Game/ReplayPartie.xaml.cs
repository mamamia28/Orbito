using Game.Models;
using LibOrbito;

namespace Game;

[QueryProperty(nameof(PartieARevoir),"PartieARevoir")]
public partial class ReplayPartie : ContentPage
{
	private ReplayPartieModel _viewModel;
	private PartieTerminee _partieARevoir;
	public PartieTerminee PartieARevoir
	{
		get => _partieARevoir;
		set
		{
			_partieARevoir = value;
			_viewModel.InitialiserPartie(value);
		}
	}

	public ReplayPartie()
	{
		InitializeComponent();
		_viewModel = new ReplayPartieModel();
		BindingContext = _viewModel;
	}

	private async void OnRetourClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}