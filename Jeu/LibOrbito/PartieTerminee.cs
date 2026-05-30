using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LibOrbito
{
    [DataContract]
    public class PartieTerminee
    {
        [DataMember]
        public Joueur Joueur1 { get; set; }

        [DataMember]
        public Joueur Joueur2 { get; set; }

        [DataMember]
        public Joueur? JoueurGagnant { get; set; }
        [DataMember]
        public Joueur? JoueurPerdant { get; set; }

        [DataMember]
        public ModeJeu Mode { get; set; }

        [DataMember]
        public DateTime DateDeFin { get; set; }

        [DataMember]
        public int NombreDeTours { get; set; }

        
        [DataMember]
        public List<Coup> HistoriqueCoups { get; set; } = new List<Coup>();
        [DataMember]
        public EtatPartie Etat { get; set; }

        [DataMember]
        public bool VarianteInverse { get; set; } = false;
        public PartieTerminee() { }

        public PartieTerminee(Partie partie)
        {
            Joueur1 = partie.Joueur1;
            Joueur2 = partie.Joueur2;
            JoueurGagnant = partie.JoueurGagnant;
            JoueurPerdant = partie.JoueurPerdant;
            Mode = partie.Mode;
            Etat = partie.Etat;
            DateDeFin = partie.DateDeCreation;
            NombreDeTours = partie.CompteurTours;
            HistoriqueCoups = new List<Coup>(partie.CoupsJoues);
            VarianteInverse = partie.VarianteInverse;
        }
    }
}