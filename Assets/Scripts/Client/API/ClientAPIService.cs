using UnityEngine;
using System.Threading.Tasks;
using CesiPlayground.Shared.API;
using CesiPlayground.Shared.Data.Players;

namespace CesiPlayground.Client.API
{
    /// <summary>
    /// A client-side service for communicating with the game's backend API.
    /// This is the single point of contact for all client-to-server HTTP requests.
    /// It is a ScriptableObject, so an asset instance must be created in the project.
    /// </summary>
    [CreateAssetMenu(fileName = "ClientAPIService", menuName = "Services/Client API Service")]
    public class ClientAPIService : APIRequester
    {
        /// <summary>
        /// Sends the complete player data object to the server to be saved/updated.
        /// </summary>
        /// <param name="playerData">The player data object to save.</param>
        /// <returns>The server's confirmation response.</returns>
        public async Task<string> SavePlayerDataAsync(PlayerData playerData)
        {
            string jsonPayload = JsonSerialization.ToJson(playerData);
            return await PostAsync("players/save", jsonPayload);
        }

        /// <summary>
        /// Requests the full player data for the currently authenticated player.
        /// This should be a secure endpoint that uses an authentication token.
        /// </summary>
        /// <returns>The JSON string of the player data.</returns>
        public async Task<string> LoadPlayerDataAsync()
        {
            return await GetAsync("players/me");
        }

        /// <summary>
        /// Fetches public player data for any player, usually for display purposes.
        /// This endpoint should only return non-sensitive information.
        /// </summary>
        /// <param name="playerId">The ID of the player to fetch.</param>
        /// <returns>The JSON string of the public player data.</returns>
        public async Task<string> GetPublicPlayerDataAsync(string playerId)
        {
            return await GetAsync($"players/{playerId}");
        }

        /// <summary>
        /// Fetches a specific game's scoreboard from the backend.
        /// </summary>
        /// <param name="gameId">The unique ID of the game's scoreboard to fetch.</param>
        /// <returns>The JSON string of the scoreboard data.</returns>
        public async Task<string> GetScoreboardAsync(string gameId)
        {
            return await GetAsync($"scores/{gameId}");
        }
    }
}