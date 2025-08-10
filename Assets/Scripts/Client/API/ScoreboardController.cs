using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Client.API;
using CesiPlayground.Shared.API;
using CesiPlayground.Shared.Data.Games;
using CesiPlayground.Shared.Data.Scoreboards;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A client-side controller that fetches scoreboard data from the remote API
    /// and passes it to a ScoreboardView to be displayed.
    /// </summary>
    public class ScoreboardController : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The list of games whose scoreboards can be displayed.")]
        [SerializeField] private List<DataGame> availableGames;
        
        [Header("References")]
        [Tooltip("The UI view that will display the scoreboard.")]
        [SerializeField] private ScoreboardView scoreboardView;

        private int _currentGameIndex = 0;
        private ClientAPIService _apiService;

        private void Start()
        {
            // Cache the reference to the API service at the start.
            _apiService = ServiceLocator.Get<ClientAPIService>();

            // Load the first scoreboard by default.
            if (availableGames != null && availableGames.Count > 0)
            {
                RequestScoreboardForGame(availableGames[0].GameId);
            }
        }

        /// <summary>
        /// Asynchronously fetches the scoreboard for a specific game from the server.
        /// </summary>
        /// <param name="gameId">The unique ID of the game.</param>
        public async void RequestScoreboardForGame(string gameId)
        {
            if (_apiService == null)
            {
                Debug.LogError("ClientAPIService not found. Cannot fetch scoreboard.");
                return;
            }
            
            if (scoreboardView == null)
            {
                Debug.LogError("ScoreboardView is not assigned in the inspector.");
                return;
            }

            Debug.Log($"[Client] Requesting scoreboard for game '{gameId}'...");
            
            // 1. Asynchronously call the API service and wait for the JSON response.
            string jsonData = await _apiService.GetScoreboardAsync(gameId);

            // 2. Check if the request was successful.
            if (!string.IsNullOrEmpty(jsonData))
            {
                // 3. Deserialize the JSON into our Scoreboard data object using the central helper.
                var scoreboard = JsonSerialization.FromJson<Scoreboard>(jsonData);
                
                // 4. Pass the data to the view to be displayed.
                scoreboardView.DisplayScoreboard(scoreboard);
            }
            else
            {
                Debug.LogWarning($"Could not retrieve scoreboard for game '{gameId}'. The server might not have data for it yet.");
                scoreboardView.DisplayScoreboard(new Scoreboard { GameId = gameId, Rows = new List<RowBoard>() });
            }
        }

        /// <summary>
        /// A public method to be called by UI buttons to cycle through scoreboards.
        /// </summary>
        public void ShowNextScoreboard()
        {
            if (availableGames == null || availableGames.Count == 0) return;
            
            _currentGameIndex = (_currentGameIndex + 1) % availableGames.Count;
            RequestScoreboardForGame(availableGames[_currentGameIndex].GameId);
        }
    }
}