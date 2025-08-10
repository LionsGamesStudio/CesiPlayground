using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CesiPlayground.Shared.API;
using CesiPlayground.Shared.Data.Players;

namespace CesiPlayground.Server.API
{
    /// <summary>
    /// The single, unified, server-side service for communicating with the backend web API.
    /// It is used for all authoritative actions like verifying credentials and saving scores.
    /// </summary>
    [CreateAssetMenu(fileName = "ServerAPIService", menuName = "Services/Server API Service")]
    public class ServerAPIService : APIRequester
    {
        /// <summary>
        /// Verifies player credentials against the web API's authentication endpoint.
        /// This is used by the ServerNetworkManager during the login process.
        /// </summary>
        /// <param name="username">The player's username.</param>
        /// <param name="password">The player's plain-text password.</param>
        /// <returns>The PlayerData if credentials are valid, otherwise null.</returns>
        public async Task<PlayerData> VerifyCredentialsAsync(string username, string password)
        {
            var loginData = new { username, password };
            string jsonPayload = JsonSerialization.ToJson(loginData);
            
            // This POSTs to an endpoint like http://your_api_address/api/players/verify
            string responseJson = await PostAsync("players/verify", jsonPayload);

            if (string.IsNullOrEmpty(responseJson))
            {
                return null; // Login failed (e.g., 401 Unauthorized, 404 Not Found, or other error)
            }

            // The API should return the full PlayerData object on success.
            return JsonSerialization.FromJson<PlayerData>(responseJson);
        }

        /// <summary>
        /// Sends a request to the web API to create a new player account.
        /// </summary>
        /// <returns>The server's response string on success, null on failure.</returns>
        public async Task<string> CreateAccountAsync(string username, string password)
        {
            var accountData = new { username, password };
            string jsonPayload = JsonSerialization.ToJson(accountData);
            // This POSTs to an endpoint like http://your_api_address/api/players/create
            return await PostAsync("players/create", jsonPayload);
        }
        
        /// <summary>
        /// Sends the final scores of a game session to the web API to be saved in the database.
        /// This is used by the ScoreService at the end of a game.
        /// </summary>
        /// <param name="gameId">The ID of the game type (e.g., "MollSmash").</param>
        /// <param name="scores">A dictionary mapping PlayerIds to their final scores.</param>
        /// <returns>True if the request was successful.</returns>
        public async Task<bool> SaveScoresAsync(string gameId, Dictionary<string, float> scores)
        {
            var payload = new { gameId, scores };
            string jsonPayload = JsonSerialization.ToJson(payload);

            // This POSTs to an endpoint like http://your_api_address/api/scores/submit
            string response = await PostAsync("scores/submit", jsonPayload);

            // A non-null, non-empty response usually indicates success.
            return !string.IsNullOrEmpty(response);
        }
    }
}