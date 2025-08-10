using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Network;
using CesiPlayground.Client.Players;
using CesiPlayground.Shared.Data.Games;
using CesiPlayground.Shared.Events.Client;
using CesiPlayground.Client.LevelDesign;

namespace CesiPlayground.Client.Games
{
    public class ClientGameManager
    {
        // This dictionary now stores the root GameObject of the instantiated game environment.
        private readonly Dictionary<string, GameObject> _activeGameInstances = new Dictionary<string, GameObject>();
        private readonly Dictionary<string, DataGame> _gameDataRegistry = new Dictionary<string, DataGame>();

        public ClientGameManager()
        {
            ServiceLocator.Register(this);
            LoadGameData();

            // Listen for the new, clearer server commands.
            GameEventSystem.Client.Register<InstantiateGameEvent>(OnInstantiateGame);
            GameEventSystem.Client.Register<ReturnToHubEvent>(OnReturnToHub);
        }

        private void LoadGameData()
        {
            var allGameData = Resources.LoadAll<DataGame>("GameData");
            foreach (var data in allGameData)
            {
                if (!string.IsNullOrEmpty(data.GameId))
                {
                    _gameDataRegistry[data.GameId] = data;
                }
            }
        }

        /// <summary>
        /// Called when the server commands this client to create a game instance.
        /// </summary>
        private void OnInstantiateGame(InstantiateGameEvent e)
        {
#if UNITY_EDITOR || UNITY_CLIENT
            if (_activeGameInstances.ContainsKey(e.InstanceId) || !_gameDataRegistry.TryGetValue(e.GameId, out var gameData) || gameData.GamePrefab == null) return;

            GameObject instance = Object.Instantiate(gameData.GamePrefab, e.Position, Quaternion.identity);
            _activeGameInstances.Add(e.InstanceId, instance);
            
            // Find the spawn point component within the new instance.
            var spawnPoint = instance.GetComponentInChildren<PlayerSpawnPoint>();
            if (PlayerRigController.HasInstance && spawnPoint != null)
            {
                PlayerRigController.TryGetInstance().TeleportTo(spawnPoint.transform);
            }
            else { Debug.LogError($"Prefab for game '{gameData.GameId}' is missing a PlayerSpawnPoint component!"); }

            if (instance.TryGetComponent<GameView>(out var view)) { view.Initialize(e.InstanceId); }
#endif
        }

        /// <summary>
        /// Called when the server commands this client to return to the hub.
        /// </summary>
        private void OnReturnToHub(ReturnToHubEvent e)
        {
#if UNITY_EDITOR || UNITY_CLIENT
            if (_activeGameInstances.TryGetValue(e.InstanceId, out GameObject instance))
            {
                if (instance != null) Object.Destroy(instance);
                _activeGameInstances.Remove(e.InstanceId);
            }
            
            var playerRig = PlayerRigController.TryGetInstance();
            var hubReturnPoint = Object.FindFirstObjectByType<HubReturnPoint>();
            if (playerRig != null && hubReturnPoint != null)
            {
                playerRig.TeleportTo(hubReturnPoint.transform);
            }
#endif
        }
    }
}