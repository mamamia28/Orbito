using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using LibOrbito;

namespace Game.Models
{
    public class SetupIAVsIaModel : INotifyPropertyChanged
    {

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}