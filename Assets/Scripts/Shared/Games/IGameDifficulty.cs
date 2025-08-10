

namespace CesiPlayground.Shared.Games
{
    /// <summary>
    /// Defines a contract for game strategies that support difficulty levels.
    /// </summary>
    public interface IGameDifficulty
    {
        /// <summary>
        /// Sets the difficulty for the game logic.
        /// </summary>
        /// <param name="difficulty">The selected difficulty level.</param>
        void SetDifficulty(GameDifficulty difficulty);
    }
}