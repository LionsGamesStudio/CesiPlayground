using UnityEngine;

namespace CesiPlayground.Shared.Games
{
    /// <summary>
    /// Factory to create the strategy of a game
    /// </summary>
    public abstract class GameStrategyFactory : ScriptableObject
    {
        // <summary>
        /// Creates the server-side game strategy logic.
        /// </summary>
        /// <returns>An instance of the game strategy.</returns>
        public abstract IGameStrategy CreateGameStrat();
    }
}
