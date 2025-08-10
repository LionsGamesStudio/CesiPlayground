namespace CesiPlayground.Shared.Games
{
    /// <summary>
    /// Defines a contract for a wave-based game logic.
    /// Implementations will live on the server.
    /// </summary>
    public interface IWave
    {
        /// <summary>
        /// Gets a value indicating whether the wave has completed its objectives.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Initialize the wave.
        /// </summary>
        void InitializeLevel();

        /// <summary>
        /// The main update loop for the wave, called by its parent strategy.
        /// </summary>
        /// <param name="deltaTime">The time since the last server tick.</param>
        void Update(float deltaTime);

        /// <summary>
        /// Reset the wave to its initial state.
        /// </summary>
        void ResetLevel();
    }
}