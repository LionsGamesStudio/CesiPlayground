using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Games;
using CesiPlayground.Shared.Events.Game;
using CesiPlayground.Server.Network;
using CesiPlayground.Shared.Network;
using CesiPlayground.Shared.Data.Games;

namespace CesiPlayground.Server.Games
{
    public class GameSessionController : IGameSessionContext
    {
        public string GameInstanceId { get; }
        public DataGame GameData { get; }
        public List<Vector3> SpawnPoints { get; } // Now a public property

        private readonly IGameStrategy _gameStrategy;
        private readonly List<string> _playerIds = new List<string>();
        private readonly Dictionary<string, float> _playerScores = new Dictionary<string, float>();
        private bool _isGameActive = false;
        private ServerNetworkManager _networkManager;

        /// <summary>
        /// Constructor for games that don't require pre-defined spawn points.
        /// </summary>
        public GameSessionController(string instanceId, DataGame dataGame, GameStrategyFactory factory)
            : this(instanceId, dataGame, factory, new List<Vector3>())
        {
        }

        /// <summary>
        /// The main constructor that accepts a list of discovered spawn points.
        /// </summary>
        public GameSessionController(string instanceId, DataGame dataGame, GameStrategyFactory factory, List<Vector3> spawnPoints)
        {
            GameInstanceId = instanceId;
            GameData = dataGame;
            SpawnPoints = spawnPoints;

            _networkManager = ServiceLocator.Get<ServerNetworkManager>();

            _gameStrategy = factory.CreateGameStrat();
            _gameStrategy.Initialize(this);
            
            GameEventSystem.Server.Register<PlayerScoredEvent>(OnPlayerScored);
        }

        public void AddPlayer(string playerId)
        {
            if (!_playerIds.Contains(playerId))
            {
                _playerIds.Add(playerId);
                _playerScores[playerId] = 0;
            }
        }

        public void StartGame()
        {
            if (_isGameActive || _gameStrategy == null) return;
            _isGameActive = true;
            _gameStrategy.StartGame();
        }

        public void Update(float deltaTime)
        {
            if (_isGameActive)
            {
                _gameStrategy.Update(deltaTime);
            }
        }

        public void EndGame()
        {
            if (!_isGameActive) return;
            _isGameActive = false;
            _gameStrategy.EndGame();
            
            GameEventSystem.Server.Raise(new GameSessionFinishedEvent
            {
                GameId = this.GameInstanceId,
                FinalScores = new Dictionary<string, float>(_playerScores)
            });
            
            GameEventSystem.Server.Unregister<PlayerScoredEvent>(OnPlayerScored);
        }
        
        private void OnPlayerScored(PlayerScoredEvent e)
        {
            if (e.GameId == this.GameInstanceId)
            {
                AddScore(e.PlayerId, e.Points);
            }
        }

        // --- IGameSessionContext Implementation ---
        public void AddScore(string playerId, float points)
        {
            if (_playerScores.ContainsKey(playerId) && _playerIds.Count > 0)
            {
                _playerScores[playerId] += points;
                
                _networkManager?.Broadcast(new GameStateUpdateMessage
                {
                    gameInstanceId = this.GameInstanceId,
                    localPlayerScore = _playerScores[_playerIds[0]]
                });
            }
        }
    }
}