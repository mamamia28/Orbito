using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Définit les différents niveaux de difficulté disponibles pour l'intelligence artificielle du jeu Orbito.
    /// Permet de sélectionner la stratégie de calcul utilisée lors de la configuration de la partie.
    /// </summary>
    public enum DifficulteIA
    {
        /// <summary>
        /// Niveau facile : l'IA joue de manière principalement aléatoire et n'anticipe pas les coups de l'adversaire.
        /// Lié à l'implémentation de la classe <see cref="IAFacile"/>.
        /// </summary>
        Facile,
        /// <summary>
        /// Niveau moyen : l'IA évalue les coups d'un tour sur l'autre pour chercher une victoire immédiate 
        /// ou bloquer les opportunités de victoire de l'adversaire.
        /// Lié à l'implémentation de la classe <see cref="IAMoyenne"/>.
        /// </summary>
        Moyen
    }
}
