using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using LibOrbito;

namespace Game.Models
{
    public class SetupVsIaModel : AvatarModel ,INotifyPropertyChanged
    {
        private string _pseudo1 = "";
        public string Pseudo1
        {
            get => _pseudo1;
            set { _pseudo1 = value; OnPropertyChanged(); }
        }

        private DifficulteIA _difficulteSelectionnee = DifficulteIA.Facile;
        public DifficulteIA DifficulteSelectionnee
        {
            get => _difficulteSelectionnee;
            set { _difficulteSelectionnee = value; OnPropertyChanged(); }
        }

        public ObservableCollection<DifficulteIA> ListeDifficultes { get; } = new ObservableCollection<DifficulteIA>
        {
            DifficulteIA.Facile,
            DifficulteIA.Moyen
        };
    }
}