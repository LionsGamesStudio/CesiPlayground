using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Shared.Games;
using CesiPlayground.Client.Network;
using CesiPlayground.Shared.Network;
using CesiPlayground.Shared.Data.Games;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A unified component for a game stand.
    /// In the editor, it acts as a marker for the server layout baking process.
    /// In a client build, it handles the UI logic for joining a game.
    /// </summary>
    public class GameStand : MonoBehaviour
    {
        [Tooltip("The DataGame asset that defines the game this stand represents.")]
        [SerializeField] private DataGame gameData;

        // Public accessor for the baker script to read the game data.
        public DataGame GameData => gameData;
        
        private GameDifficulty _selectedDifficulty = GameDifficulty.Normal;

        /// <summary>
        /// Public method to be called by UI Buttons to set the difficulty.
        /// </summary>
        public void SetDifficulty(string difficultyString)
        {
            if (System.Enum.TryParse<GameDifficulty>(difficultyString, out GameDifficulty newDifficulty))
            {
                _selectedDifficulty = newDifficulty;
                Debug.Log($"[GameStand] Difficulty for '{gameData.GameId}' set to: {_selectedDifficulty}");
            }
        }

        /// <summary>
        /// Called by the main "Play" button's OnClick() event.
        /// </summary>
        public void RequestJoinGame()
        {
            if (gameData == null) return;
            var networkManager = ServiceLocator.Get<ClientNetworkManager>();
            if (networkManager == null) return;
            
            networkManager.Send(new QueueForGameMessage 
            { 
                GameId = gameData.GameId,
                Difficulty = _selectedDifficulty
            });
        }
    }
}