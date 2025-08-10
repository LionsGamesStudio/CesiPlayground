using CesiPlayground.Shared.Data.Games;

namespace CesiPlayground.Shared.Games
{
    /// <summary>
    /// Defines a contract for a game session, providing the strategy with all the necessary context
    /// without creating a direct dependency on the concrete server-side implementation.
    /// </summary>
    public interface IGameSessionContext
    {
        string GameInstanceId { get; }
        DataGame GameData { get; }

        /// <summary>
        /// Allows the strategy to request the end of the game.
        /// </summary>
        void EndGame();

        /// <summary>
        /// Allows the strategy to report that a player has scored points.
        /// </summary>
        /// <param name="playerId">The ID of the player who scored.</param>
        /// <param name="points">The number of points to add.</param>
        void AddScore(string playerId, float points);
    }
}