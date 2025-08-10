using UnityEngine;
using System.Collections.Generic;
using CesiPlayground.Core;
using CesiPlayground.Shared.Games;
using CesiPlayground.Shared.Games.MollSmash;
using CesiPlayground.Server.Spawning;
using CesiPlayground.Server.AI;
using CesiPlayground.Server.Network;
using CesiPlayground.Shared.Network;
using CesiPlayground.Server.Games.MollSmash;

namespace CesiPlayground.Server.Games
{
    public class MollSmashStrategy : IGameStrategy, IGameDifficulty
    {
        private enum GameState { Intro, Wave, Finished }
        
        private IGameSessionContext _sessionContext;
        private DataMollSmashDifficulty _difficulty;
        private ServerNetworkManager _networkManager;
        
        private int _life;
        private float _spawnTimer;
        private float _stateTimer;
        private GameState _currentState;

        public MollSmashStrategy() { }

        public void Initialize(IGameSessionContext context)
        {
            _sessionContext = context;
            _networkManager = ServiceLocator.Get<ServerNetworkManager>();
        }

        public void SetDifficulty(GameDifficulty difficulty)
        {
            var assetManager = ServiceLocator.Get<Assets.ServerAssetManager>();
            string assetName = $"MollSmash{difficulty}";
            _difficulty = assetManager?.GetAsset<DataMollSmashDifficulty>(assetName);
        }

        public void StartGame()
        {
            if (_difficulty == null) SetDifficulty(GameDifficulty.Normal);
            if (_difficulty == null)
            {
                _sessionContext.EndGame();
                return;
            }
            
            _life = _difficulty.LifePoint;
            _currentState = GameState.Intro;
            _stateTimer = 3f;
            
            _networkManager?.Broadcast(new GameStatusMessage { gameInstanceId = _sessionContext.GameInstanceId, statusText = "Game Starts in 3..." });
        }

        public void Update(float deltaTime)
        {
            if (_currentState == GameState.Finished) return;
            
            if (_stateTimer > 0)
            {
                _stateTimer -= deltaTime;
                if (_stateTimer <= 0)
                {
                    if (_currentState == GameState.Intro)
                    {
                        _currentState = GameState.Wave;
                        _networkManager?.Broadcast(new GameStatusMessage { gameInstanceId = _sessionContext.GameInstanceId, statusText = "Wave 1 - GO!" });
                    }
                }
                return;
            }
            
            if (_currentState == GameState.Wave)
            {
                UpdateWave(deltaTime);
            }
        }

        private void UpdateWave(float deltaTime)
        {
            if (_life <= 0)
            {
                _currentState = GameState.Finished;
                _sessionContext.EndGame();
                return;
            }
            
            var entityManager = ServiceLocator.Get<ServerEntityManager>();
            if (entityManager != null)
            {
                var activeEntities = new List<AIEntity>(entityManager.GetAllEntities());
                foreach (var entity in activeEntities)
                {
                    if (entity is MollEntity moll && !moll.IsActive)
                    {
                        OnMollEscaped(moll.NetworkId);
                    }
                }
            }
            
            _spawnTimer -= deltaTime;
            if (_spawnTimer <= 0)
            {
                _spawnTimer = _difficulty.SpawnRate / _difficulty.DifficultyMultiplier;
                SpawnMoll();
            }
        }

        private void SpawnMoll()
        {
            var objectManager = ServiceLocator.Get<ServerObjectManager>();
            var entityManager = ServiceLocator.Get<ServerEntityManager>();
            if (objectManager == null || entityManager == null || _difficulty.MollPrefabIds == null || _difficulty.MollPrefabIds.Count == 0) return;

            string mollPrefabId = _difficulty.MollPrefabIds[Random.Range(0, _difficulty.MollPrefabIds.Count)];
            Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            Vector3 upPosition = spawnPosition + Vector3.up * 0.5f;

            int networkId = objectManager.SpawnObject(mollPrefabId, spawnPosition, Quaternion.identity);
            var newMollEntity = new MollEntity(networkId, spawnPosition, upPosition, _difficulty.MollLifetime, 1f * _difficulty.DifficultyMultiplier, 2f);
            entityManager.RegisterEntity(newMollEntity);
        }

        public void OnMollHit(int mollNetworkId, string playerId)
        {
            var entityManager = ServiceLocator.Get<ServerEntityManager>();
            AIEntity hitMoll = entityManager.GetEntity(mollNetworkId);
            
            if (hitMoll != null && hitMoll.IsActive)
            {
                hitMoll.IsActive = false;
                
                _sessionContext.AddScore(playerId, 100 * _difficulty.DifficultyMultiplier);
                
                ServiceLocator.Get<ServerObjectManager>().DestroyObject(mollNetworkId);
                entityManager.UnregisterEntity(mollNetworkId);
            }
        }

        public void OnMollEscaped(int mollNetworkId)
        {
            var entityManager = ServiceLocator.Get<ServerEntityManager>();
            if (entityManager.GetEntity(mollNetworkId) != null)
            {
                _life--;
                _networkManager?.Broadcast(new GameLivesUpdatedMessage { gameInstanceId = _sessionContext.GameInstanceId, remainingLives = _life });
                
                ServiceLocator.Get<ServerObjectManager>().DestroyObject(mollNetworkId);
                entityManager.UnregisterEntity(mollNetworkId);
            }
        }

        public void ResetGame()
        {
            var entityManager = ServiceLocator.Get<ServerEntityManager>();
            if (entityManager == null) return;

            var allEntities = new List<AIEntity>(entityManager.GetAllEntities());
            foreach(var entity in allEntities)
            {
                if (entity is MollEntity)
                {
                    ServiceLocator.Get<ServerObjectManager>().DestroyObject(entity.NetworkId);
                    entityManager.UnregisterEntity(entity.NetworkId);
                }
            }
            _life = 0;
            _currentState = GameState.Finished;
        }

        public void EndGame()
        {
            ResetGame();
            _networkManager?.Broadcast(new GameStatusMessage { gameInstanceId = _sessionContext.GameInstanceId, statusText = "Game Over!" });
            Debug.Log($"[Server] MollSmash Game Over for game {_sessionContext.GameInstanceId}.");
        }
    }
}