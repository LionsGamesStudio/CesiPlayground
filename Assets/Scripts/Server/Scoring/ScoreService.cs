using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Server.API;
using CesiPlayground.Shared.Events.Game;
using UnityEngine;


namespace CesiPlayground.Server.Scoring
{
    /// <summary>
    /// Authoritative service that manages player scores during game sessions.
    /// It listens for game end events and reports the final scores to the
    /// backend web API for persistence.
    /// </summary>
    public class ScoreService
    {
        private readonly ServerAPIService _apiService;

        public ScoreService()
        {
            ServiceLocator.Register(this);
            _apiService = ServiceLocator.Get<ServerAPIService>();
            GameEventSystem.Server.Register<GameSessionFinishedEvent>(OnGameSessionFinished);
        }

        private async void OnGameSessionFinished(GameSessionFinishedEvent e)
        {
            if (e.FinalScores == null || e.FinalScores.Count == 0)
            {
                Debug.Log($"[ScoreService] Game {e.GameId} finished with no scores to report.");
                return;
            }

            Debug.Log($"[ScoreService] Game {e.GameId} finished. Reporting final scores to the API...");

            foreach (var scoreEntry in e.FinalScores)
            {
                Debug.Log($"... Player {scoreEntry.Key}: {scoreEntry.Value}");
            }

            if (_apiService == null)
            {
                Debug.LogError("ServerAPIService not found. Cannot save scores.");
                return;
            }

            bool success = await _apiService.SaveScoresAsync(e.GameId, e.FinalScores);

            if (success)
            {
                Debug.Log($"[ScoreService] Successfully saved scores for game {e.GameId}.");
            }
            else
            {
                Debug.LogError($"[ScoreService] Failed to save scores for game {e.GameId}.");
            }
        }
    }
}