using LibOrbito;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Game.Models
{
    class ReplayPartieModel : INotifyPropertyChanged
    {
        private PartieTerminee? _partie;
        private int _indexCoupCourant = -1;

        public ObservableCollection<CelluleModel> Grille { get; } = new ObservableCollection<CelluleModel>();
        public string NomJoueur1 => _partie?.Joueur1.Pseudo ?? "";
        public string NomJoueur2 => _partie?.Joueur2.Pseudo ?? "";
        public string AvatarJoueur1 => _partie?.Joueur1.Avatar ?? "";
        public string AvatarJoueur2 => _partie?.Joueur2.Avatar ?? "";

        public bool VarianteEstActivee => _partie?.VarianteInverse ?? false;
        public string InfoTourCourant
        {
            get
            {
                if (_partie == null || _partie.HistoriqueCoups.Count == 0) return "Aucun coup joué";
                if (_indexCoupCourant == -1) return "Début de la partie";
                if (_indexCoupCourant >= _partie.HistoriqueCoups.Count - 1) return "Fin de la partie";
                return $"Coup {_indexCoupCourant + 1} / {_partie.HistoriqueCoups.Count}";
            }
        }

        public ICommand CommandeSuivant { get; }
        public ICommand CommandePrecedent { get; }

        public ReplayPartieModel()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Grille.Add(new CelluleModel(x, y));
                }
            }

            CommandeSuivant = new Command(() =>
            {
                if (_partie != null && _indexCoupCourant < _partie.HistoriqueCoups.Count - 1)
                {
                    _indexCoupCourant++;
                    ReconstruirePlateau();
                }
            });

            CommandePrecedent = new Command(() =>
            {
                if (_partie != null && _indexCoupCourant > -1)
                {
                    _indexCoupCourant--;
                    ReconstruirePlateau();
                }
            });
        }

        public void InitialiserPartie(PartieTerminee p)
        {
            _partie = p;
            _indexCoupCourant = -1;

            OnPropertyChanged(nameof(NomJoueur1));
            OnPropertyChanged(nameof(NomJoueur2));
            OnPropertyChanged(nameof(AvatarJoueur1));
            OnPropertyChanged(nameof(AvatarJoueur2));
            OnPropertyChanged(nameof(VarianteEstActivee));
            ReconstruirePlateau();
        }

        public void ReconstruirePlateau()
        {
            LibOrbito.Plateau plateauVirtuel = new LibOrbito.Plateau();
            plateauVirtuel.Initialiser();

            for(int i=0; i<= _indexCoupCourant; i++)
            {
                var coup = _partie.HistoriqueCoups[i];
                if (coup.origineDeplacement != null && coup.destinationDeplacement != null)
                {
                    plateauVirtuel.DeplacerBille(coup.origineDeplacement, coup.destinationDeplacement);
                }
                if (coup.placement != null)
                {
                    plateauVirtuel.AjouterBille(new Bille(coup.Responsable.CouleurBilles), coup.placement);
                }
                plateauVirtuel.ActionnerOrbite(coup.SensRotationInverse);
            }

            foreach (var cellule in Grille)
            {
                Bille? bille = plateauVirtuel.At(new Position(cellule.X, cellule.Y));
                if (bille == null) cellule.CheminImage = "case_vide.png";
                else if (bille.CouleurB == Couleur.Blanc) cellule.CheminImage = "bille_blanche.png";
                else cellule.CheminImage = "bille_noire.png";
            }

            OnPropertyChanged(nameof(InfoTourCourant));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
