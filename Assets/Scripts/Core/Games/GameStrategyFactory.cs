using UnityEngine;

namespace Assets.Scripts.Core.Games
{
    /// <summary>
    /// Factory to create the strategy of a game
    /// </summary>
    public abstract class GameStrategyFactory : ScriptableObject
    {
        /// <summary>
        /// Create the game strategy
        /// </summary>
        /// <param name="game">Game we need to create the strategy</param>
        /// <returns></returns>
        public abstract IGameStrategy CreateGameStrat(Game game);
    }
}
