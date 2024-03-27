using Assets.Scripts.Core;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Game;
using Assets.Scripts.Core.Scoring;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Core.Wave;
using Assets.Scripts.Process.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Process.GunClub.Game
{
    public class GunClubStrategy : IGameStrategy
    {
        private Assets.Scripts.Core.Game.Game _game;
        private List<DataGunClubWave> _waves;

        private IWave _currentWave;
        private int _currentLevel = 0;

        private EventBinding<OnWaveFinished> _waveFinished;

        public GunClubStrategy(Assets.Scripts.Core.Game.Game game, List<DataGunClubWave> waves)
        {
            _game = game;
            _waves = waves;

            if (_waves.Count == 0)
            {
                Debug.LogError("No data level provide.");
                return;
            }

            _waveFinished = new EventBinding<OnWaveFinished>(NextWave);
            EventBus<OnWaveFinished>.Register(_waveFinished);
        }

        #region Game Loop

        /// <summary>
        /// Start the game
        /// </summary>
        public void StartGame()
        {
            if(_game.Spawner == null)
            {
                Debug.LogError("No spawner found.");
                return;
            }

            _game.Score= 0;
            _currentLevel = 0;

            _currentWave = new GunClubWave(_game, _waves[_currentLevel]);

            _game.StartCoroutine(LaunchGame());
        }


        public void ResetGame()
        {
            _game.StartedGame = false;
            _currentLevel = 0;
            _currentWave.ResetLevel();
        }

        /// <summary>
        /// End the game
        /// </summary>
        public void EndGame()
        {
            _currentLevel = 0;
            _currentWave.ResetLevel();
        }

        #endregion

        /// <summary>
        /// Go to the next wave
        /// </summary>
        public void NextWave(OnWaveFinished e)
        {
            if (e.Game.ID != _game.ID) return;

            _currentLevel++;

            _game.StartCoroutine(DisplayNextWave());
        }

        private IEnumerator LaunchGame()
        {
            _game.Display = "Start Game";
            yield return new WaitForSeconds(2);

            _game.Display = "Wave 1";
            yield return new WaitForSeconds(2);

            _game.Display = "Score : 0";
            _currentWave.InitializeLevel();
        }

        private IEnumerator DisplayNextWave()
        {
            yield return new WaitForSeconds(2);

            if (_waves.Count - 1 <= _currentLevel)
            {
                Debug.Log("End of the game");

                _game.Display = "End of the game";

                yield return new WaitForSeconds(_waves[_currentLevel].CooldownAlive);

                _game.EndGame();
            }
            else
            {
                _game.Display = "Next Wave";
                yield return new WaitForSeconds(2);

                int waveLevel = _currentLevel;

                _game.Display = "Wave " + (waveLevel + 1);
                yield return new WaitForSeconds(2);

                _game.Display = "Score : " + _game.Score;

                _currentWave = new GunClubWave(_game, _waves[_currentLevel]);
                _currentWave.InitializeLevel();
            }

        }
    }
}
