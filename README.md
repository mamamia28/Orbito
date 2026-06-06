# Projet Orbito

Bienvenue sur le dépôt du projet Orbito, une adaptation numérique du célèbre jeu de plateau au tour par tour. Ce projet a pour but d'offrir une expérience de jeu simple, intergénérationnelle et conviviale, jouable en 1 contre 1 (Humain contre Humain, ou Humain contre IA).

## Téléchargement

Lien vers le site de téléchargement du jeu : 
[Download Orbito](https://codefirst.iut.uca.fr/kubernetes/iut-inf63-maui-2026-g3-g4/orbito-site/)

## Description du Projet

L'application permet d'affronter d'autres joueurs ou une intelligence artificielle, que ce soit le temps d'une partie rapide ou lors de tournois. Elle assure un suivi complet de la progression grâce à la sauvegarde des scores et des statistiques détaillées par joueur (nombre de victoires, coups joués, etc.).

L'interface de l'application s'articule autour d'un menu principal donnant accès au jeu, aux statistiques, et à un écran de création de profil.

## Aperçu des Règles du Jeu

**But du jeu :** Être le premier à aligner 4 billes de sa couleur horizontalement, verticalement ou en diagonale.

**Déroulement d'un tour :**
Chaque joueur joue à tour de rôle. Un tour se compose de 3 étapes :

1. **Déplacer une bille adverse (Facultatif)** : Vous pouvez déplacer une bille de votre adversaire vers une case adjacente libre (horizontalement ou verticalement).
2. **Placer sa propre bille (Obligatoire)** : Placez une de vos billes sur n'importe quelle case libre du plateau. Une fois posée, elle ne peut être déplacée que par l'adversaire.
3. **Actionner le bouton Orbito (Obligatoire)** : Pour terminer votre tour, vous devez appuyer sur le bouton Orbito. Toutes les billes se déplacent alors d'une position selon les orbites du plateau (rotation antihoraire). Un alignement de 4 billes n'est valide qu'après avoir appuyé sur ce bouton.

La partie s'arrête lorsqu'un joueur aligne 4 billes de sa couleur après la rotation du plateau.

## Documentation et Conception

Retrouvez ci-dessous l'ensemble des documents ayant servi à l'analyse, la conception et l'élaboration de ce projet, répartis par catégories.

### Contexte, Personas et UI

- [Contexte et Règles du Jeu](Documentation/ContexteSketchsPersonnas/ContexteEtReglesDuJeu.md)
  Présentation détaillée du projet, des règles du jeu et des variantes possibles.

- [Diagramme de Cas d'Utilisation](Documentation/ContexteSketchsPersonnas/DiagrammeDeCasD'utilisationEtDescription.md)
  Description des interactions possibles entre les acteurs (joueurs, IA) et le système.

- [Personas et User Stories](Documentation/ContexteSketchsPersonnas/PersonaetUserStory.md)
  Définition des profils types des utilisateurs ciblés et de leurs attentes.

- [Sketchs](Documentation/ContexteSketchsPersonnas/Sketch.md)
  Premières ébauches et maquettes conceptuelles de l'interface du jeu.

- [StoryBoards](Documentation/ContexteSketchsPersonnas/StoryBoards.md)
  Illustration du déroulement d'une partie et de la navigation entre les différents écrans de l'application.

### Conception Technique

- [Diagramme de Classes](Documentation/Conception/diagramme_classes.md)
  Représentation de la structure logicielle, modélisant les différentes classes du jeu et leurs relations.

- [Diagramme de Séquence](Documentation/Conception/diagrammes_sequence.md)
  Visualisation de la dynamique du système, montrant les échanges de messages au cours du temps entre les objets.

- [Diagramme de Paquetage](Documentation/Conception/diagramme_paquetage.md)
  Organisation globale de l'architecture du code sous forme de différents modules (packages).
