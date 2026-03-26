## Contexte

Nous devons créer une application simple d’utilisation d’un jeu de société se jouant en 1 contre 1.
Les combinaisons possibles sont : 
- Humain VS Humain
- Humain VS IA
- IA VS IA

Le jeu choisi est Orbito, un jeu de plateau au tour par tour dont le but est d'aligner 4 billes. Les mouvements sont simples. Au début de la partie se déroule le tour d’initialisation. Le premier joueur doit poser une bille et appuyer sur le bouton central qui va déclencher un mécanisme d'orbite (qui représente la spécificité principale du jeu). Ensuite le prochain joueur pourra aussi placer une de ses billes.
Puis les joueurs peuvent déplacer une bille adverse sur ses cases adjacentes et poser une de leurs billes sur le plateau.
Il s'agit donc d'un jeu de stratégie complexe.

Cette version numérique offrira une expérience de jeu simple et intergénérationnelle, conçue pour plaire aussi bien aux enfants qu'aux adultes. Elle sera particulièrement adaptée aux joueurs amateurs, qui pourront affronter d'autres joueurs à domicile ou une IA, que ce soit le temps d'une partie rapide ou lors de tournois.

L'application permettra des affrontements en 1 contre 1 (humain ou IA) et assurera un suivi complet de la progression grâce à la sauvegarde des scores et des statistiques détaillées par joueur (nombre de victoires, coups joués, etc.).

Côté navigation, l'interface s'articulera autour d'un menu principal (donnant accès au jeu et aux statistiques), d'un écran de création de profil, et de l'interface de jeu elle-même, qui intégrera le plateau et les données de la partie en cours.

---

## Règles du Jeu

**But du jeu :** Être le premier à aligner 4 billes de sa couleur horizontalement, verticalement ou en diagonale.

### Mise en place de la partie
Le joueur le plus jeune prend les billes blanches et commence.

### Déroulement d'un tour
Chaque joueur joue à tour de rôle. Un tour se compose de 3 étapes :

1. **Déplacer une bille adverse (Facultatif)**
   Vous pouvez déplacer une bille de votre adversaire vers une case adjacente libre.
   Le déplacement se fait uniquement à l'horizontale ou à la verticale (pas en diagonale).
   *Exceptions :* Cette étape est impossible au premier tour ou si toutes les cases adjacentes aux billes adverses sont occupées.

2. **Placer sa propre bille (Obligatoire)**
   Placez une de vos billes sur n'importe quelle case libre du plateau.
   *Note :* Une fois posée, vous ne pouvez plus déplacer vos propres billes ; seul votre adversaire peut le faire durant son tour.

3. **Actionner le bouton Orbito (Obligatoire)**
   Pour terminer votre tour, vous devez appuyer sur le bouton Orbito.
   Toutes les billes sur le plateau se déplacent alors d'une position.
   Le mouvement suit deux orbites (intérieure et extérieure) qui tournent dans le sens antihoraire.
   *Important :* Un alignement de 4 billes n'est valide qu'après avoir appuyé sur le bouton Orbito. Il n'est jamais comptabilisé durant le tour d'un joueur, avant la rotation.

### Fin de partie
La partie s'arrête dans les situations suivantes :

- **Victoire immédiate :** Un joueur aligne 4 billes de sa couleur après la rotation du plateau.
  *Attention :* Si la rotation crée un alignement de 4 billes pour votre adversaire, c'est lui qui gagne.
- **Égalité :** Si les deux joueurs obtiennent simultanément un alignement de 4 billes après la rotation, la partie est nulle.
- **Prolongations :** Si toutes les billes sont placées sans qu'il y ait de gagnant, appuyez 5 fois de plus sur le bouton Orbito. Le premier à obtenir un alignement gagne. Si aucun alignement n'apparaît après ces 5 pressions, c'est un match nul.

---

## Idées de Variante

- **Mode de jeu speed :** lancer un timer à chaque tour d’un joueur et si ce timer est dépassé alors le jeu déplacera une bille au hasard du joueur concerné (donc c’est un malus ou un avantage si le joueur est chanceux).
- Le sens des billes peuvent changer à chaque tour (avant / arrière) aléatoirement.

---

## Fonctionnalités Supplémentaires

- Ajouter un dé avec 3 faces blanches et 3 noires qui décidera de quelle couleur va commencer en premier.
- Ajout d’une partie de tutoriel fait entre IA à visionner si voulu pour comprendre les règles ainsi que des bulles d'explication sous la forme de pop-up.