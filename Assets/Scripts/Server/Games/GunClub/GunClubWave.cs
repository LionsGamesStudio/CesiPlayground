using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Shared.Games;
using CesiPlayground.Server.Spawning;
using CesiPlayground.Shared.Games.GunClub;

namespace CesiPlayground.Server.Games.GunClub
{
    public class GunClubWave : IWave
    {
        private readonly IGameSessionContext _sessionContext;
        private readonly DataGunClubWave _waveData;
        private readonly ServerObjectManager _objectManager;

        private int _targetsSpawned = 0;
        private float _spawnTimer = 0f;

        // This list tracks all targets that this wave has spawned and are still active.
        private readonly List<int> _activeTargetIds = new List<int>();

        public bool IsFinished { get; private set; } = false;

        public GunClubWave(IGameSessionContext sessionContext, DataGunClubWave waveData)
        {
            _sessionContext = sessionContext;
            _waveData = waveData;
            _objectManager = ServiceLocator.Get<ServerObjectManager>();
        }


        public void InitializeLevel()
        {
            _targetsSpawned = 0;
            _spawnTimer = 0f;
            IsFinished = false;
        }


        public void Update(float deltaTime)
        {
            // The wave is considered finished only when all targets have been spawned
            // AND all spawned targets have been destroyed.
            if (_targetsSpawned >= _waveData.NumberOfTargets && _activeTargetIds.Count == 0)
            {
                IsFinished = true;
                return;
            }

            // Do not spawn more targets than required.
            if (_targetsSpawned >= _waveData.NumberOfTargets)
            {
                return;
            }

            _spawnTimer -= deltaTime;
            if (_spawnTimer <= 0)
            {
                _spawnTimer = _waveData.SpawnCooldown;
                SpawnTarget();
            }
        }

        /// <summary>
        /// Spawns a new target for this wave.
        /// </summary>
        private void SpawnTarget()
        {
            if (_waveData.PrefabIds.Count == 0) return;

            int randTargetIndex = Random.Range(0, _waveData.PrefabIds.Count);
            string prefabId = _waveData.PrefabIds[randTargetIndex];

            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 1, 15);

            // Spawn the object and get its unique network ID to track it.
            int newTargetId = _objectManager.SpawnObject(prefabId, spawnPosition, Quaternion.identity);
            _activeTargetIds.Add(newTargetId);

            _targetsSpawned++;
        }


        /// <summary>
        /// This method must be called by the game logic when a target is hit or despawns.
        /// </summary>
        public void OnTargetDestroyed(int networkId)
        {
            _activeTargetIds.Remove(networkId);
        }
        
        
        public void ResetLevel()
        {
            // Create a copy of the list to iterate over, as DestroyObject might modify the original.
            var targetsToDestroy = new List<int>(_activeTargetIds);
            foreach (var targetId in targetsToDestroy)
            {
                _objectManager.DestroyObject(targetId);
            }
            _activeTargetIds.Clear();
            _targetsSpawned = 0;
            IsFinished = true; // Mark as finished to stop any further logic.
        }
    }
}