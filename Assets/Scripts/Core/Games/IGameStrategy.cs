
namespace Assets.Scripts.Core.Games
{
    public interface IGameStrategy
    {

        /// <summary>
        /// Start the logic of the game
        /// </summary>
        void StartGame();

        /// <summary>
        /// Reset the logic
        /// </summary>
        void ResetGame();

        /// <summary>
        /// End the logic
        /// </summary>
        void EndGame();
    }
}
