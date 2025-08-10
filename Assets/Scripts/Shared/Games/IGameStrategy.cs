
namespace CesiPlayground.Shared.Games
{
    /// <summary>
    /// Defines the contract for the core logic of any mini-game.
    /// Implementations of this interface will live in the Server assembly.
    /// </summary>
    public interface IGameStrategy
    {
        /// <summary>
        /// Initializes the strategy with its context. Called once after creation.
        /// </summary>
        /// <param name="context">The game session that owns this strategy.</param>
        void Initialize(IGameSessionContext context);
        
        /// <summary>
        /// Contains the logic to run when the game starts.
        /// </summary>
        void StartGame();
        
        /// <summary>
        /// The main update loop for the strategy, called by the GameSessionController.
        /// </summary>
        /// <param name="deltaTime">The time since the last server tick.</param>
        void Update(float deltaTime);

        /// <summary>
        /// Contains the logic to run to reset the game to its initial state.
        /// </summary>
        void ResetGame();

        /// <summary>
        /// Contains the logic to run to clean up and end the game.
        /// </summary>
        void EndGame();
    }
}