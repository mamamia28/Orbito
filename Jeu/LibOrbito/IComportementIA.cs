using System;
using System.Collections.Generic;
using System.Text;

namespace LibOrbito
{
    /// <summary>
    /// Définit le contrat pour les comportements de l'intelligence artificielle.
    /// Toute classe implémentant cette interface (comme IAFacile, IAMoyenne) 
    /// doit fournir sa propre logique stratégique pour déterminer son prochain coup.
    /// </summary>
    public interface IComportementIA
    {
        /// <summary>
        /// Évalue l'état actuel de la partie et détermine le coup à jouer selon la stratégie spécifique de l'IA.
        /// </summary>
        /// <param name="partie">L'état actuel de la partie en cours, incluant le plateau, le compteur de tours et le joueur courant.</param>
        /// <returns>Une instance de <see cref="Action"/> contenant la décision de l'IA (déplacement éventuel d'une bille adverse et placement de sa propre bille).</returns>
        Action CalculerCoup(Partie partie);
    }
}
