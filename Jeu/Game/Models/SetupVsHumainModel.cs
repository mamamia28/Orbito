using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Game.Models;

namespace Game.Models
{
    public class SetupVsHumainModel : AvatarModel
    {
        private string _pseudo1 = "";
        public string Pseudo1
        {
            get => _pseudo1;
            set { _pseudo1 = value; OnPropertyChanged(); }
        }

        private string _pseudo2 = "";
        public string Pseudo2
        {
            get => _pseudo2;
            set { _pseudo2 = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
