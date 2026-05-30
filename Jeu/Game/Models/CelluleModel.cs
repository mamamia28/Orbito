using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Game.Models
{
    public class CelluleModel : INotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }

        private string _cheminImage = "case_vide.png";
        public string CheminImage
        {
            get => _cheminImage;
            set
            {
                if (_cheminImage != value)
                {
                    _cheminImage = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _highlighted = false;
        public bool Highlighted 
        {
            get => _highlighted;
            set { _highlighted = value; OnPropertyChanged(); }
        }

        public CelluleModel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}