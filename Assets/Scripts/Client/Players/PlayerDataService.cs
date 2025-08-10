using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Data.Players;
using CesiPlayground.Shared.Events.Data;
using UnityEngine;

namespace CesiPlayground.Client.Players
{
    /// <summary>
    /// A client-side service that holds the data for the currently logged-in player.
    /// It acts as a cache and a single source of truth for player data on the client.
    /// </summary>
    public class PlayerDataService
    {
        public PlayerData CurrentPlayerData { get; private set; }
        public bool IsLoggedIn => CurrentPlayerData != null;

        /// <summary>
        /// Registers itself with the Service Locator and subscribes to data events.
        /// </summary>
        public PlayerDataService()
        {
            ServiceLocator.Register(this);
            GameEventSystem.Client.Register<PlayerDataLoadedEvent>(OnPlayerDataLoaded);
        }
        
        /// <summary>
        /// Called when the PersistenceService successfully loads data from the server.
        /// It updates the current player data and notifies other client systems.
        /// </summary>
        private void OnPlayerDataLoaded(PlayerDataLoadedEvent e)
        {
            CurrentPlayerData = e.Data;
            Debug.Log($"Player data loaded for: {CurrentPlayerData.PlayerName}");
        }

        /// <summary>
        /// Log out the current player.
        /// This clears the current player data and unregisters from events.
        /// /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            CurrentPlayerData = null;
            GameEventSystem.Client.Unregister<PlayerDataLoadedEvent>(OnPlayerDataLoaded);
            ServiceLocator.Unregister<PlayerDataService>();
        }
    }
}