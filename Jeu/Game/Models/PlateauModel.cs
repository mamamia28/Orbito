using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LibOrbito;

namespace Game.Models
{
    public class PlateauModel : INotifyPropertyChanged
    {
        private OrbitoManager _manager;

        public ObservableCollection<CelluleModel> Grille { get; } = new ObservableCollection<CelluleModel>();
        public ObservableCollection<Coup> LesCoups { get; } = new ObservableCollection<Coup>();
        public string MessageActuel => _manager?.MessagePhase ?? "Chargement...";
        public string MessageErreur => _manager?.MessageErreur ?? "";
        public string NomJoueurCourant => _manager?.NomJoueur ?? "";
        public string NomJoueur1 => _manager?._partie.Joueur1.Pseudo!;
        public string NomJoueur2 => _manager?._partie.Joueur2.Pseudo!;
        public int BillesRestantesJoueur1 => _manager?._partie.Joueur1.BillesRestantes ?? 8;
        public int BillesRestantesJoueur2 => _manager?._partie.Joueur2.BillesRestantes ?? 8;
        public string AvatarJoueur1 => _manager?._partie.Joueur1.Avatar!;
        public string AvatarJoueur2 => _manager?._partie.Joueur2.Avatar!;
        public string AvatarJoueurGagnant => (_manager?._partie.JoueurGagnant == null) ? "egalite.png" : _manager?._partie.JoueurGagnant!.Avatar;
        public bool PeutPasserDeplacement => _manager?.PhaseActuelle == PhaseTour.ChoixDeplacementAdverse && !_manager.EstTourIA;
        public bool VarianteEstActivee => _manager != null && _manager.varianteInverse;

        private bool _estPartieTerminee = false;
        public bool EstPartieTerminee
        {
            get => _estPartieTerminee;
            set { _estPartieTerminee = value; OnPropertyChanged(); }
        }
        private bool _estCoupsClicked = false;
        public bool EstCoupsClicked
        {
            get => _estCoupsClicked;
            set { _estCoupsClicked = value; OnPropertyChanged(); }
        }

        public string TexteVictoire => _manager?.NomGagnant == "Egalite" || _manager?.NomGagnant == "Aucun"
            ? "Match nul !"
            : $"{_manager?.NomGagnant} a gagné ! ";

        public string TexteCarte => _manager?.NomGagnant == "Egalite" || _manager?.NomGagnant == "Aucun"
            ? "Pas de vainqueur"
            : $"Bravo {_manager?.NomGagnant} !";

        private bool _billeEnMain = false;
        private bool _iaEnTrainDeJouer = false;
        private bool _estAideAffiche;
        public bool EstAideAffiche
        {
            get => _estAideAffiche;
            set
            {
                if (_estAideAffiche != value)
                {
                    _estAideAffiche = value;
                    OnPropertyChanged(nameof(EstAideAffiche));
                }
            }
        }

        public string TexteAide
        {
            get
            {
                if (_manager == null) return "";

                return _manager.PhaseActuelle switch
                {
                    PhaseTour.PlacementBille => "Phase de placement :\n\nGlissez-déposez une de vos billes depuis votre réserve vers une case vide du plateau.",
                    PhaseTour.ChoixDeplacementAdverse => "Phase de déplacement (Optionnelle) :\n\nVous pouvez glisser-déposer une bille de votre adversaire vers une case vide adjacente.\n\nSi vous ne le souhaitez pas ou ne le pouvez pas, cliquez sur le bouton 'Ne rien déplacer'.",
                    PhaseTour.DestinationDeplacement => "Phase de destination :\n\nRelâchez la bille adverse sélectionnée sur l'une des cases en surbrillance pour valider le déplacement.",
                    PhaseTour.AttenteBouton => "Fin du tour :\n\nAppuyez sur le bouton central Orbito (rouge) pour faire pivoter le plateau et terminer votre tour.",
                    _ => "Veuillez patienter..."
                };
            }
        }

        public ICommand CommandeClicReserve { get; }
        public ICommand CommandeClicOrbito { get; }
        public ICommand CommandeDragCase { get; }
        public ICommand CommandeDropCase { get; }
        public ICommand CommandePasserDeplacement { get; }
        public ICommand CommandeMenuPrincipal { get; }
        public ICommand CommandeRejouer { get; }     

        public PlateauModel()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Grille.Add(new CelluleModel(x, y));
                }
            }

            CommandePasserDeplacement = new Command(() =>
            {
                if (_manager != null && !_manager.EstTourIA) 
                { 
                    _manager.GererAction(-1, -1); 
                    EffacerSurbrillance();
                }
            });

            CommandeClicReserve = new Command(() =>
            {
                if (_manager != null && !_manager.EstTourIA && _manager.PhaseActuelle == PhaseTour.PlacementBille)
                {
                    _billeEnMain = true;
                }
            });

            CommandeDragCase = new Command<CelluleModel>((cellule) =>
            {
                if (_manager == null || _manager.EstTourIA || EstPartieTerminee) return;

                if (_manager.PhaseActuelle == PhaseTour.ChoixDeplacementAdverse)
                {
                    _manager.GererAction(cellule.X, cellule.Y);
                    if (_manager.PhaseActuelle == PhaseTour.DestinationDeplacement)
                    {
                        AfficherSurbrillance(cellule.X, cellule.Y);
                    }
                }
            });

            CommandeDropCase = new Command<CelluleModel>((cellule) =>
            {
                if (_manager == null || _manager.EstTourIA || EstPartieTerminee) return;

                if (_manager.PhaseActuelle == PhaseTour.PlacementBille && _billeEnMain)
                {
                    _manager.GererAction(cellule.X, cellule.Y);
                    if (_manager.PhaseActuelle != PhaseTour.PlacementBille)
                        _billeEnMain = false;
                }
                else if (_manager.PhaseActuelle == PhaseTour.DestinationDeplacement)
                {
                    _manager.GererAction(cellule.X, cellule.Y);
                    EffacerSurbrillance();
                }
            });

            CommandeClicOrbito = new Command(() =>
            {
                if (_manager != null && !_manager.EstTourIA && !EstPartieTerminee) _manager.ActionnerBouton();
            });
            CommandeMenuPrincipal = new Command(async () =>
            {
                await Shell.Current.GoToAsync("///MainPage");
            });

            CommandeRejouer = new Command(async () =>
            {
                await Shell.Current.GoToAsync("ChoixMode");
            });
        }

        public void InitialiserPartie(string pseudo1,string avatar1, string pseudo2,string avatar2, ModeJeu mode, DifficulteIA? difficulte, bool varianteInverse=false)
        {
            DifficulteIA diff = difficulte ?? DifficulteIA.Facile;
            _manager = new OrbitoManager(pseudo1, avatar1, pseudo2, avatar2, mode, diff);
            _manager.varianteInverse = varianteInverse;
            _manager.OnMiseAJourUI += SynchroniserGrille;
            _manager.OnActionIADetailleeAsync += AfficherActionIA;
            OnPropertyChanged(nameof(NomJoueur1));
            OnPropertyChanged(nameof(NomJoueur2));
            OnPropertyChanged(nameof(AvatarJoueur1));
            OnPropertyChanged(nameof(AvatarJoueur2));
            OnPropertyChanged(nameof(VarianteEstActivee));
            SynchroniserGrille();
        }

        private async void SynchroniserGrille()
        {
            foreach (var cellule in Grille)
            {
                Bille? bille = _manager._partie.LePlateau.At(new Position(cellule.X, cellule.Y));
                if (bille == null) cellule.CheminImage = "case_vide.png";
                else if (bille.CouleurB == Couleur.Blanc) cellule.CheminImage = "bille_blanche.png";
                else cellule.CheminImage = "bille_noire.png";
            }
            LesCoups.Clear();
            foreach (var coup in _manager._partie.CoupsJoues)
            {
                LesCoups.Add(coup);
            }
            if (_manager.EstTerminee())
            {
                if (!EstPartieTerminee)
                {
                    OnPropertyChanged(nameof(AvatarJoueurGagnant));
                    OnPropertyChanged(nameof(TexteVictoire));
                    OnPropertyChanged(nameof(TexteCarte));
                    await Task.Delay(1200);
                    EstCoupsClicked = false;
                    EstPartieTerminee = true;
                    App.Hist.AjouterPartie(_manager._partie);
                    App.PartieActive = null;
                    File.Delete(App.xmlSauvegarde);
                }
                return;
            }

            OnPropertyChanged(nameof(MessageActuel));
            OnPropertyChanged(nameof(MessageErreur));
            OnPropertyChanged(nameof(TexteAide));
            OnPropertyChanged(nameof(NomJoueurCourant));
            OnPropertyChanged(nameof(PeutPasserDeplacement));
            OnPropertyChanged(nameof(BillesRestantesJoueur1));
            OnPropertyChanged(nameof(BillesRestantesJoueur2));
            App.PartieActive = _manager._partie;
            App.SauvegarderPartieEnCours();

            if (_manager.EstTourIA)
            {
                DeclencherIA();
            }
        }
        public void InitialiserReprise()
        {
            if (App.PartieActive != null)
            {
                _manager = new OrbitoManager(App.PartieActive);
                _manager.OnMiseAJourUI += SynchroniserGrille;
                _manager.OnActionIADetailleeAsync += AfficherActionIA;
                OnPropertyChanged(nameof(NomJoueur1));
                OnPropertyChanged(nameof(NomJoueur2));
                OnPropertyChanged(nameof(AvatarJoueur1));
                OnPropertyChanged(nameof(AvatarJoueur2));
                OnPropertyChanged(nameof(NomJoueurCourant));
                OnPropertyChanged(nameof(BillesRestantesJoueur1));
                OnPropertyChanged(nameof(BillesRestantesJoueur2));
                OnPropertyChanged(nameof(VarianteEstActivee));

                SynchroniserGrille();
            }
        }


        private async Task AfficherActionIA(string m)
        {
            foreach (var cellule in Grille)
            {
                Bille? bille = _manager._partie.LePlateau.At(new Position(cellule.X, cellule.Y));
                if (bille == null) cellule.CheminImage = "case_vide.png";
                else if (bille.CouleurB == Couleur.Blanc) cellule.CheminImage = "bille_blanche.png";
                else cellule.CheminImage = "bille_noire.png";
            }
            await Task.Delay(1200);
        }

        private async void DeclencherIA()
        {
            if (_iaEnTrainDeJouer || EstPartieTerminee) return;
            _iaEnTrainDeJouer = true;

            await Task.Delay(1000);

            if (_manager != null && !_manager.EstTerminee())
            {
                await _manager.FaireJouerIAAsync();
            }

            _iaEnTrainDeJouer = false;

            if (_manager != null && _manager.EstTourIA && !EstPartieTerminee)
            {
                DeclencherIA();
            }
        }

        private void AfficherSurbrillance(int sourceX, int sourceY)
        {
            Position posSource = new Position(sourceX, sourceY);
            foreach (var cellule in Grille)
            {
                Position posCellule = new Position(cellule.X, cellule.Y);
                if (posSource.EstAdjacente(posCellule))
                {
                    if (_manager._partie.LePlateau.At(posCellule) == null)
                    {
                        cellule.Highlighted = true;
                    }
                }
                else
                {
                    cellule.Highlighted = false;
                }
            }
        }

        private void EffacerSurbrillance()
        {
            foreach (var cellule in Grille)
            {
                cellule.Highlighted = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}