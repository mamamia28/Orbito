using LibOrbito;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Game.Models
{
    public class AvatarModel : INotifyPropertyChanged
    {
        public ObservableCollection<AvatarOption> ListeAvatars { get; } = new ObservableCollection<AvatarOption>
        {
            new AvatarOption("Chien","chien.png"),
            new AvatarOption("Chat", "chat.png"),
            new AvatarOption("Astronaute","dotnet.png"),
            new AvatarOption("Renard","avatar_renard.png"),
            new AvatarOption("Hibou","avatar_hibou.png"),
            new AvatarOption("Ours","avatar_ours.png"),
            new AvatarOption("Lapin","avatar_lapin.png")

        };

        private AvatarOption _avatarSelectionne = new AvatarOption("", "");
        public AvatarOption AvatarSelectionne
        {
            get => _avatarSelectionne;
            set
            {
                _avatarSelectionne = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CheminImage));
            }
        }
        public string CheminImage => AvatarSelectionne?.Fichier ?? "chien.png";

        private AvatarOption _avatarSelectionne2 = new AvatarOption("", ""); 
        public AvatarOption AvatarSelectionne2
        {
            get => _avatarSelectionne2;
            set
            {
                _avatarSelectionne2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CheminImage2));
            }
        }
        public string CheminImage2 => AvatarSelectionne2?.Fichier ?? "chien.png";

        public AvatarModel()
        {
            AvatarSelectionne = ListeAvatars[0];
            AvatarSelectionne2 = ListeAvatars[0];
        }

        



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public class AvatarOption
        {
            public string Nom { get; set; } = "Chien";
            public string Fichier { get; set; } = "chien.png";

            public AvatarOption(string Nom,string Fichier)
            {
                this.Nom = Nom;
                this.Fichier = Fichier;
            }
        }
    }
}
