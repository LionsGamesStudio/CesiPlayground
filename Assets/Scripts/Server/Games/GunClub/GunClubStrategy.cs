using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Shared.Games;
using CesiPlayground.Shared.Games.GunClub;

namespace CesiPlayground.Server.Games.GunClub
{
    public class GunClubStrategy : IGameStrategy
    {
        private IGameSessionContext _sessionContext;
        private List<DataGunClubWave> _waves;
        
        private GunClubWave _currentWave;
        private int _currentWaveIndex = -1;
        private float _intermissionTimer = 0f;
        private bool _isGameActive = false;

        public GunClubStrategy() { }

        public void Initialize(IGameSessionContext context)
        {
            _sessionContext = context;
        }
        
        public void StartGame()
        {
            if (_waves == null || _waves.Count == 0)
            {
                Debug.LogError("No waves configured for this Gun Club game. Ending game.");
                _sessionContext.EndGame();
                return;
            }

            _currentWaveIndex = -1;
            _isGameActive = true;
            StartNextWave();
        }

        public void ResetGame()
        {
            if (_currentWave != null)
            {
                _currentWave.ResetLevel();
            }
            _isGameActive = false;
            _currentWaveIndex = -1;
            _currentWave = null;
        }

        public void EndGame()
        {
            _currentWave.ResetLevel();
            _isGameActive = false;
        }
        
        
        public void Update(float deltaTime)
        {
            if (!_isGameActive) return;

            // If a wave is active, update it.
            if (_currentWave != null)
            {
                _currentWave.Update(deltaTime);
                if (_currentWave.IsFinished)
                {
                    _currentWave = null; // Mark the wave as completed.

                    // Check if that was the last wave.
                    if (_currentWaveIndex >= _waves.Count - 1)
                    {
                        // Game over!
                        _sessionContext.EndGame();
                    }
                    else
                    {
                        // Start a countdown to the next wave.
                        _intermissionTimer = 5f;
                    }
                }
            }
            // If we are in the intermission between waves, tick down the timer.
            else if (_intermissionTimer > 0)
            {
                _intermissionTimer -= deltaTime;
                if (_intermissionTimer <= 0)
                {
                    StartNextWave();
                }
            }
        }

        public void SetConfiguration(List<DataGunClubWave> waves)
        {
            _waves = waves;
        }
        
        private void StartNextWave()
        {
            _currentWaveIndex++;
            Debug.Log($"[Server] Starting wave {_currentWaveIndex + 1}");
            _currentWave = new GunClubWave(_sessionContext, _waves[_currentWaveIndex]);
            _currentWave.InitializeLevel();
        }
    }
}