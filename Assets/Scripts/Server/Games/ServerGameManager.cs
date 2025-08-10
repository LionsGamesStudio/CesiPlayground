using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Server.Network;
using CesiPlayground.Shared.Network;
using CesiPlayground.Shared.Data.Games;

namespace CesiPlayground.Server.Games
{
    /// <summary>
    /// Authoritative manager for all active game sessions on the server.
    /// It is responsible for creating, tracking, and destroying GameSessionControllers,
    /// and for notifying clients of these lifecycle events.
    /// </summary>
    public class ServerGameManager
    {
        private readonly Dictionary<string, GameSessionController> _activeGames = new Dictionary<string, GameSessionController>();
        private readonly GameRegistry _gameRegistry;
        private int _nextGameInstanceId = 0;
        
        private ServerNetworkManager _networkManager;

        public ServerGameManager(GameRegistry registry)
        {
            _gameRegistry = registry;
            _gameRegistry.Initialize();
            ServiceLocator.Register(this);
        }
        
        // Helper property for lazy initialization to avoid race conditions.
        private ServerNetworkManager NetworkManager
        {
            get
            {
                if (_networkManager == null)
                    _networkManager = ServiceLocator.Get<ServerNetworkManager>();
                return _networkManager;
            }
        }
        
        public void Update(float deltaTime)
        {
            var activeGameKeys = new List<string>(_activeGames.Keys);
            foreach (var key in activeGameKeys)
            {
                if(_activeGames.TryGetValue(key, out var session))
                {
                    session.Update(deltaTime);
                }
            }
        }

        // This version is a simple entry point for games without specific spawn points.
        public GameSessionController CreateGameSession(DataGame gameData, Vector3 position)
        {
            return CreateGameSession(gameData, position, new List<Vector3>());
        }
        
        /// <summary>
        /// Creates a new authoritative game session and broadcasts the command to clients.
        /// </summary>
        public GameSessionController CreateGameSession(DataGame gameData, Vector3 position, List<Vector3> spawnPoints)
        {
            var factory = _gameRegistry.GetFactory(gameData.GameId);
            if (factory == null)
            {
                Debug.LogError($"[ServerGameManager] No factory found for GameId: {gameData.GameId}");
                return null;
            }

            string instanceId = $"game_{_nextGameInstanceId++}";
            var newGame = new GameSessionController(instanceId, gameData, factory, spawnPoints);
            _activeGames.Add(instanceId, newGame);
            
            Debug.Log($"[ServerGameManager] Created new game session: {gameData.GameName} (ID: {instanceId}) with {spawnPoints.Count} spawn points.");

            NetworkManager?.Broadcast(new InstantiateGameMessage { gameInstanceId = instanceId, gameDataId = gameData.GameId, position = position });
            
            return newGame;
        }

        /// <summary>
        /// Destroys a game session and commands clients to return to the hub.
        /// </summary>
        public void DestroyGameSession(string instanceId)
        {
            if (_activeGames.Remove(instanceId))
            {
                Debug.Log($"[Server] Destroyed game session: {instanceId}");
                
                if (NetworkManager != null)
                {
                    var message = new ReturnToHubMessage 
                    { 
                        gameInstanceId = instanceId,
                        // The server defines where players return in the hub. (Maybe change this later)
                        hubReturnPosition = Vector3.zero 
                    };
                    NetworkManager.Broadcast(message);
                }
            }
        }
        
        public GameSessionController GetGameSession(string instanceId)
        {
            _activeGames.TryGetValue(instanceId, out var game);
            return game;
        }
    }
}