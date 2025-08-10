using UnityEngine;
using TMPro;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Game;

namespace CesiPlayground.Client.Games
{
    /// <summary>
    /// The client-side, visual representation of a game instance.
    /// Its only job is to display data received from the server (e.g., score)
    /// and manage visual/audio feedback. It contains no game logic.
    /// </summary>
    public class GameView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text scoreText;
        
        private string _gameInstanceId;
        
        /// <summary>
        /// Initializes the view with the ID provided by the server.
        /// </summary>
        public void Initialize(string gameInstanceId)
        {
            _gameInstanceId = gameInstanceId;
            gameObject.name = $"GameView (ID: {_gameInstanceId})";
            
            GameEventSystem.Client.Register<GameStateUpdatedEvent>(OnGameStateUpdated);
        }
        
        private void OnDestroy()
        {
            GameEventSystem.Client.Unregister<GameStateUpdatedEvent>(OnGameStateUpdated);
        }

        /// <summary>
        /// Listens to state updates on the CLIENT bus and updates the UI.
        /// </summary>
        private void OnGameStateUpdated(GameStateUpdatedEvent e)
        {
            if (e.GameId == _gameInstanceId)
            {
                scoreText.text = $"Score: {e.LocalPlayerScore}";
            }
        }
    }
}