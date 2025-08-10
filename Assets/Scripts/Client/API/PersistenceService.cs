using UnityEngine;
using System.Threading.Tasks;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.API;
using CesiPlayground.Shared.Events.Data;
using CesiPlayground.Shared.Data.Players;

namespace CesiPlayground.Client.API
{
    /// <summary>
    /// A client-side service that orchestrates the saving and loading of persistent player data
    /// by communicating with the ClientAPIService.
    /// </summary>
    public class PersistenceService : Singleton<PersistenceService>
    {
        private ClientAPIService _apiService;

        protected override void Awake()
        {
            base.Awake();
            if (this.enabled == false) return; // Destroyed as duplicate
            
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Get a reference to the API service once.
            _apiService = ServiceLocator.Get<ClientAPIService>();
        }

        /// <summary>
        /// Asynchronously sends a request to the server to save the player's data.
        /// </summary>
        /// <param name="playerData">The player data object to save.</param>
        public async Task<bool> RequestSavePlayerData(PlayerData playerData)
        {
            if (_apiService == null)
            {
                Debug.LogError("ClientAPIService not available.");
                return false;
            }

            Debug.Log("Requesting to save player data to the server...");
            string response = await _apiService.SavePlayerDataAsync(playerData);

            // A non-null response usually indicates success.
            if (response != null)
            {
                Debug.Log("Server confirmed data saved.");
                // Optionally, raise a "Save Successful" event for the UI
                return true;
            }
            
            Debug.LogError("Failed to save player data.");
            return false;
        }

        /// <summary>
        /// Asynchronously sends a request to the server to load the player's data.
        /// On success, it raises a PlayerDataLoadedEvent.
        /// </summary>
        public async Task<bool> RequestLoadPlayerData()
        {
            if (_apiService == null)
            {
                Debug.LogError("ClientAPIService not available.");
                return false;
            }

            Debug.Log("Requesting to load player data from the server...");
            string jsonData = await _apiService.LoadPlayerDataAsync();

            if (!string.IsNullOrEmpty(jsonData))
            {
                Debug.Log("Player data received from server.");
                
                // 1. Deserialize the JSON into a player data object
                var playerData = JsonSerialization.FromJson<PlayerData>(jsonData);

                // 2. Raise a client event with the loaded data
                GameEventSystem.Client.Raise(new PlayerDataLoadedEvent { Data = playerData });
                return true;
            }
            
            Debug.LogError("Failed to load player data.");
            return false;
        }
    }
}