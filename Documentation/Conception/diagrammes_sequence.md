## 1. Création et initialisation d'une partie

```plantuml
@startuml
participant "ClientUI"      as C order 1
participant "OrbitoManager" as M order 2
participant "Partie"        as P order 3
participant "Joueur 1 & 2"  as J order 4
participant "IA"            as IA order 5
participant "Plateau"       as Pl order 6

C -> M : new OrbitoManager(nomJ1, avatarJ1, nomJ2, avatarJ2, mode, difficulte)
activate M

M -> P : new Partie(nomJ1, avatarJ1, nomJ2, avatarJ2, mode, difficulte)
activate P

opt mode inclut une IA (HumainVsIA ou IAVsIA)
    P -> P : GenererIA(difficulte)
    activate P
    alt difficulte == Facile
        P -> IA : new IAFacile()
        activate IA
        IA --> P : instance IComportementIA
        deactivate IA
    else difficulte == Moyen
        P -> IA : new IAMoyenne()
        activate IA
        IA --> P : instance IComportementIA
        deactivate IA
    end
    P --> P : IComportementIA?
    deactivate P
end

P -> J : new Joueur(nomJ1, ..., Couleur.Blanc, iaJ1)
activate J
J --> P : instance Joueur (J1)
deactivate J

P -> J : new Joueur(nomJ2, ..., Couleur.Noir, iaJ2)
activate J
J --> P : instance Joueur (J2)
deactivate J

P -> Pl : new Plateau(rayon: 2)
activate Pl
Pl -> Pl : Initialiser()
Pl --> P : instance Plateau
deactivate Pl

P -> Pl : Initialiser()
activate Pl
Pl --> P : void
deactivate Pl

P --> M : instance Partie
deactivate P

M --> C : instance OrbitoManager
deactivate M
@enduml
```

## 2. Déroulement d'un tour de jeu
```plantuml
@startuml
participant "ClientUI"        as C  order 1
participant "OrbitoManager"   as M  order 2
participant "Partie"          as P  order 3
participant "JoueurCourant"   as J  order 4
participant "Plateau"         as Pl order 5

== 1. Gestion de l'Action (Sélection, Déplacement ou Placement) ==

C -> M : GererAction(X, Y)
activate M

alt PhaseActuelle == ChoixDeplacementAdverse
    M -> M : TraiterSelectionAdverse(Position(X, Y))
    activate M
    M -> P : EstBilleAdverse(Position)
    activate P
    P -> Pl : At(Position)
    activate Pl
    Pl --> P : Bille
    deactivate Pl
    P --> M : bool
    deactivate P
    deactivate M

else PhaseActuelle == DestinationDeplacement
    M -> M : TraiterDestinationAdverse(Position(X, Y))
    activate M
    M -> P : DeplacerBilleAdverse(source, dest)
    activate P
    P -> Pl : DeplacerBille(source, dest)
    activate Pl
    Pl --> P : bool
    deactivate Pl
    P --> M : bool
    deactivate P
    deactivate M

else PhaseActuelle == PlacementBille
    M -> M : TraiterPlacementBille(Position(X, Y))
    activate M
    
    M -> P : PoserBille(Position)
    activate P
    P -> J : placerBille()
    activate J
    J --> P : void (ou Exception)
    deactivate J
    
    P -> Pl : AjouterBille(Bille, Position)
    activate Pl
    Pl --> P : bool
    deactivate Pl
    
    P --> M : bool
    deactivate P
    
    opt Pose échouée
        M -> M : Mettre à jour MessageErreur
    end
    deactivate M
end

M --> C : void
deactivate M

== 2. Fin du Tour (Pression sur le Bouton Orbito) ==

C -> M : ActionnerBouton()
activate M

M -> P : AppuyerBoutonOrbito()
activate P

break placement manquant avant le tour 16
    P --> M : InvalidOperationException
end

P -> Pl : ActionnerOrbite()
activate Pl
Pl --> P : bool
deactivate Pl

P -> P : VerifierVictoire()
activate P
P -> Pl : DetecterAlignement()
activate Pl
Pl --> P : (blancGagne, noirGagne)
deactivate Pl
P --> P : JoueurGagnant
deactivate P

alt Partie terminée
    P --> M : true
else Partie en cours
    P -> P : ChangerJoueurCourant()
    P --> M : true
end
deactivate P

M -> M : Mettre à jour PhaseActuelle
M --> C : void
deactivate M
@enduml
```

## 3. Sauvegarde et chargement de l'historique

```plantuml
@startuml
skinparam maxMessageSize 150

participant "Client"     as C order 1
participant "Historique"   as H order 2
participant "Partie"       as P order 3
participant "Statistiques" as S order 4

== 1. Ajout d'une Partie à l'Historique ==

C -> H : AjouterPartie(partie)
activate H

H -> H : _lesSessions.Add(partie)

H -> H : ActualiserListeJoueurs(partie)
activate H
H -> P : get JoueurCourant
P --> H : Joueur (J1)
H -> P : ChangerJoueurCourant()
H -> P : get JoueurCourant
P --> H : Joueur (J2)
H -> P : ChangerJoueurCourant()

H -> H : AjouterSiExistant(J1)
H -> H : AjouterSiExistant(J2)
deactivate H

H --> C : void
deactivate H

== 2. Génération des Statistiques d'un Joueur ==

C -> H : RecupererStats(pseudo)
activate H

loop pour chaque partie dans _lesSessions
    H -> H : EstPresentDansPartie(partie, pseudo)
    activate H
    H -> P : get JoueurCourant
    P --> H : Joueur
    H -> P : ChangerJoueurCourant()
    H -> P : get JoueurCourant
    P --> H : Joueur
    H -> P : ChangerJoueurCourant()
    H --> H : bool
    deactivate H
    
    opt EstPrésent == true
        H -> P : get JoueurGagnant
        P --> H : Joueur?
        H -> H : incrémente (victoires ou defaites ou total)
    end
end

H -> S : new Statistiques(total, victoires, defaites)
activate S
S -> S : calcul interne du TauxVictoire
S --> H : instance Statistiques
deactivate S

H --> C : Statistiques
deactivate H
@enduml
```

## 4. IA Facile : Calculer un coup
```plantuml
@startuml
skinparam maxMessageSize 150
skinparam ParticipantPadding 20

participant "Partie"    as P order 1
participant "IAFacile"  as IA order 2
participant "Plateau"   as Pl order 3

== IA Facile : Calculer un Coup ==

P -> IA : CalculerCoup(partie)
activate IA

IA -> P : get LePlateau
P --> IA : Plateau
IA -> P : get JoueurCourant
P --> IA : Joueur
IA -> P : get CompteurTours
P --> IA : int

opt CompteurTours > 2 et <= 16 et Hasard == 0
    IA -> IA : ObtenirDeplacementsValides(plateau, couleurAdverse)
    activate IA
    IA -> IA : ObtenirCasesVides()
    IA -> Pl : EstCaseLibre(Position) / At(Position)
    Pl --> IA : Bille / bool
    IA --> IA : deplacementsPossibles
    deactivate IA
    
    opt deplacementsPossibles.Count > 0
        IA -> IA : Sélection aléatoire d'un déplacement (origine, destination)
    end
end

IA -> IA : ObtenirCasesVides(plateau)
activate IA
IA -> Pl : EstCaseLibre(Position)
Pl --> IA : bool
IA --> IA : casesVides
deactivate IA

IA -> IA : Retirer destination et ajouter origine dans casesVides
IA -> IA : Sélection aléatoire d'un placement parmi casesVides

IA --> P : new Action(origine, destination, placement)
deactivate IA
@enduml
```
## 5. IA Moyenne : Calculer un coup
```plantuml
@startuml
skinparam maxMessageSize 150
skinparam ParticipantPadding 20

participant "Partie"        as P order 1
participant "IAMoyenne"     as IA order 2
participant "PlateauSim"    as Sim order 3

== IA Moyenne : Calculer un Coup ==

P -> IA : CalculerCoup(partie)
activate IA

IA -> IA : GenererTousLesCoupsLegaux(plateau, maCouleur, advCouleur, tour)
activate IA
IA --> IA : List<Action>
deactivate IA

loop pour chaque coup possible (Recherche Victoire Immédiate)
    IA -> IA : CopierPlateau(plateau)
    activate IA
    create Sim
    IA -> Sim : new Plateau()
    IA --> IA : instance PlateauSim
    deactivate IA
    
    IA -> IA : AppliquerCoup(Sim, coup, maCouleur)
    activate IA
    opt déplacement ou placement
        IA -> Sim : DeplacerBille() / AjouterBille()
    end
    IA -> Sim : ActionnerOrbite()
    deactivate IA
    
    IA -> Sim : DetecterAlignement()
    activate Sim
    Sim --> IA : (blancGagne, noirGagne)
    deactivate Sim
    
    opt Si le coup rend l'IA gagnante
        IA --> P : coup (Victoire immédiate)
    end
end

IA -> IA : Trier les coups avec et sans déplacement

loop pour chaque coup (Vérification Survie)
    IA -> IA : CopierPlateau(plateau) => Sim
    IA -> IA : AppliquerCoup(Sim, coup, maCouleur)
    
    IA -> IA : AdversairePeutGagner(Sim, advCouleur, maCouleur, tour+1)
    activate IA
    IA -> IA : GenererTousLesCoupsLegaux(Sim, advCouleur, maCouleur, tour+1)
    
    loop pour chaque coup adverse
        IA -> IA : CopierPlateau(Sim) => SimAdverse
        IA -> IA : AppliquerCoup(SimAdverse, coupAdverse, advCouleur)
        IA -> Sim : DetecterAlignement()
        opt Si le coup adverse rend l'adversaire gagnant
            IA --> IA : true
        end
    end
    IA --> IA : false
    deactivate IA
    
    opt Si l'adversaire ne gagne pas (false)
        IA -> IA : Ajouter aux coups sans danger
    end
end

alt coups sans danger et sans déplacement existent
    IA --> P : coup sans danger et sans déplacement
else coups sans danger avec déplacement existent
    IA --> P : coup sans danger avec déplacement
else aucun coup sans danger (Défaite inévitable)
    IA --> P : coup par défaut au hasard
end

deactivate IA
@enduml
```

### Description des diagrammes de séquence

**1. Gestion de l'Historique et des Statistiques**
Ce diagramme montre comment on sauvegarde une partie terminée et comment on génère les scores. Pour calculer les statistiques d'un joueur, la classe `Historique` parcourt toutes les anciennes parties pour compter ses victoires et ses défaites, puis crée un objet `Statistiques` avec ces calculs.

**2. Initialisation d'une nouvelle partie**
Ce diagramme explique ce qui se passe au lancement du jeu. Le `OrbitoManager` (qui fait le lien avec l'interface) crée la `Partie`. Celle-ci va ensuite préparer le `Plateau`, créer les `Joueur`s et préparer l'Intelligence Artificielle si on joue contre l'ordinateur.

**3. Déroulement d'un tour de jeu**
Ici, on voit comment le code réagit aux clics du joueur. Selon le moment du tour (choisir une bille, la déplacer ou poser une bille), le `OrbitoManager` demande à la `Partie` d'effectuer l'action. À la toute fin, quand on clique sur le bouton Orbito, le plateau effectue sa rotation et le jeu vérifie si quelqu'un a aligné 4 billes pour gagner.

- **Pour l'IA Moyenne** : Elle simule tous ses coups possibles sur une copie du plateau (un faux plateau). Elle regarde d'abord si un coup la fait gagner immédiatement. Si ce n'est pas le cas, elle va tester les futurs coups du joueur adverse pour essayer de ne choisir que des actions "sans danger" (pour ne pas perdre bêtement au tour suivant).

---

## Mise à jour (Jalon 3) - Nouveaux diagrammes de séquence

Suite aux dernières évolutions du code, voici les versions mises à jour des diagrammes de séquence les plus importants.

### 1. Déroulement d'un tour de jeu (avec la Variante de rotation)

Ce diagramme intègre la possibilité de faire tourner le plateau dans le sens inverse (variante).

```plantuml
@startuml
participant "ClientUI"        as C  order 1
participant "OrbitoManager"   as M  order 2
participant "Partie"          as P  order 3
participant "JoueurCourant"   as J  order 4
participant "Plateau"         as Pl order 5

== 1. Gestion de l'Action (Sélection, Déplacement ou Placement) ==

C -> M : GererAction(X, Y)
activate M

alt PhaseActuelle == ChoixDeplacementAdverse
    M -> M : TraiterSelectionAdverse(Position(X, Y))
    M -> P : EstBilleAdverse(Position)
    P -> Pl : At(Position)
    Pl --> P : Bille
    P --> M : bool

else PhaseActuelle == DestinationDeplacement
    M -> M : TraiterDestinationAdverse(Position(X, Y))
    M -> P : DeplacerBilleAdverse(source, dest)
    P -> Pl : DeplacerBille(source, dest)
    Pl --> P : bool
    P --> M : bool

else PhaseActuelle == PlacementBille
    M -> M : TraiterPlacementBille(Position(X, Y))
    
    M -> P : PoserBille(Position)
    P -> J : placerBille()
    J --> P : void
    
    P -> Pl : AjouterBille(Bille, Position)
    Pl --> P : bool
    P --> M : bool
end

M --> C : void
deactivate M

== 2. Fin du Tour (Bouton Orbito) ==

C -> M : ActionnerBouton()
activate M

M -> P : AppuyerBoutonOrbito()
activate P

P -> Pl : ActionnerOrbite(VarianteInverse)
activate Pl
Pl --> P : bool
deactivate Pl

P -> P : VerifierVictoire()
P -> Pl : DetecterAlignement()
Pl --> P : (blancGagne, noirGagne)
P --> P : JoueurGagnant

alt Partie terminée
    P --> M : true
else Partie en cours
    P -> P : ChangerJoueurCourant()
    P --> M : true
end
deactivate P

M -> M : Mettre à jour PhaseActuelle
M --> C : void
deactivate M
@enduml
```

### 2. Gestion de l'Historique

Le diagramme d'historique a été fortement simplifié. Avant, on sauvegardait une classe `Partie` entière, ce qui nous obligeait à simuler des tours de jeu fictifs (`ChangerJoueurCourant`) juste pour lire qui avait joué. Désormais, l'utilisation de `PartieTerminee` rend le code beaucoup plus direct.

```plantuml
@startuml
skinparam maxMessageSize 150

participant "Client"         as C order 1
participant "Historique"     as H order 2
participant "PartieTerminee" as PT order 3
participant "Statistiques"   as S order 4

== 1. Ajout d'une Partie à l'Historique ==

C -> H : AjouterPartie(partie)
activate H

create PT
H -> PT : new PartieTerminee(partie)
PT --> H : instance PartieTerminee

H -> H : _lesSessions.Insert(0, PartieTerminee)

H -> H : ActualiserListeJoueurs(PartieTerminee)
H -> H : AjouterSiExistant(Joueur1)
H -> H : AjouterSiExistant(Joueur2)

H --> C : void
deactivate H

== 2. Génération des Statistiques d'un Joueur ==

C -> H : RecupererStats(pseudo)
activate H

loop pour chaque PartieTerminee dans _lesSessions
    H -> H : EstPresentDansPartie(PartieTerminee, pseudo)
    H --> H : bool
    
    opt EstPrésent == true
        H -> H : incrémente (total)
        alt pseudo == JoueurGagnant.Pseudo
            H -> H : incrémente (victoires)
        else
            H -> H : incrémente (defaites)
        end
    end
end

H -> S : new Statistiques(total, victoires, defaites)
activate S
S -> S : calcul interne du TauxVictoire
S --> H : instance Statistiques
deactivate S

H --> C : Statistiques
deactivate H
@enduml
```

### 3. Tour de jeu de l'Intelligence Artificielle (Asynchrone)

Ce nouveau diagramme montre le flux d'exécution d'un tour de l'ordinateur. Le processus est désormais "Asynchrone" (`Task`), ce qui signifie que l'IA réfléchit en arrière-plan sans bloquer l'écran du joueur.

```plantuml
@startuml
participant "ClientUI"        as C  order 1
participant "OrbitoManager"   as M  order 2
participant "IComportementIA" as IA order 3
participant "Partie"          as P  order 4

== Tour de l'IA (Exécution Asynchrone) ==

C -> M : FaireJouerIAAsync()
activate M

M -> IA : CalculerCoup(partie)
activate IA
IA -> IA : Analyse (Aléatoire ou Simulations)
IA --> M : Action (Coup choisi : origine, dest, placement)
deactivate IA

M -> M : GererAction(origine)
M -> M : GererAction(destination)
M -> M : GererAction(placement)

M -> P : AppuyerBoutonOrbito()
activate P
P --> M : bool
deactivate P

M -> M : OnMiseAJourUI.Invoke()
M --> C : Task terminée
deactivate M
@enduml
```

### Explications des changements récents

1. **La Variante Inverse** : Le premier diagramme montre comment le bouton Orbito déclenche désormais la méthode `ActionnerOrbite(VarianteInverse)` du plateau pour faire tourner les billes dans le sens inverse si on l'a activé.
2. **Le grand nettoyage de l'historique** : Grâce à `PartieTerminee`, l'extraction des statistiques est devenue très propre.
3. **Une IA fluide** : L'IA ne bloque plus le jeu pendant qu'elle cherche son meilleur coup (IA Moyenne) car elle s'exécute avec la méthode asynchrone `FaireJouerIAAsync()`.
