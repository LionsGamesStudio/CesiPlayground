using Assets.Scripts.Core.Addressables;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Game;
using Assets.Scripts.Process.Events;
using Assets.Scripts.Process.Events.Objects.MollEvent;
using Assets.Scripts.Process.Object;
using Assets.Scripts.Process.Objects.Targets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Process.MollSmash.Game
{
    public class MollSmashStrategy : IGameStrategy
    {
        private Assets.Scripts.Core.Game.Game _game;
        private DataMollSmashDifficulty _difficulty;
        private float _life;

        private EventBinding<OnMollDying> _onMollDying;

        [Serializable]
        public enum MollSmashDifficulty
        {
            Easy,
            Normal,
            Hard
        }

        public MollSmashStrategy(Core.Game.Game game)
        {
            _game = game;

            // Event
            _onMollDying = new EventBinding<OnMollDying>(OnMollDying);
            EventBus<OnMollDying>.Register(_onMollDying);
        }

        #region Strategy

        public void EndGame()
        {
            _difficulty = null;
            _game.StopCoroutine(Update());
        }

        public void ResetGame()
        {
            _game.StartedGame = false;
            _game.StopCoroutine(Update());
            _game.Score = 0;
        }

        public void StartGame()
        {
            if(_difficulty == null)
            {
                _game.Display = "Choose a difficulty";
                _game.StartedGame = false;
                return;
            }

            _game.Score = 0;
            _life = _difficulty.LifePoint;

            _game.StartCoroutine(LaunchGame());

        }

        #endregion

        /// <summary>
        /// Set the difficulty
        /// </summary>
        /// <param name="difficulty"></param>
        public void SetDifficulty(MollSmashDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MollSmashDifficulty.Easy:
                    _difficulty = AddressablesManager.Instance.LoadAddressableAsync<DataMollSmashDifficulty>("MollSmashEasy");
                    break;
                case MollSmashDifficulty.Normal:
                    _difficulty = AddressablesManager.Instance.LoadAddressableAsync<DataMollSmashDifficulty>("MollSmashNormal");
                    break;
                case MollSmashDifficulty.Hard:
                    _difficulty = AddressablesManager.Instance.LoadAddressableAsync<DataMollSmashDifficulty>("MollSmashHard");
                    break;
                default:
                    _difficulty = AddressablesManager.Instance.LoadAddressableAsync<DataMollSmashDifficulty>("MollSmashEasy");
                    break;
            }
        }

        #region Logic

        /// <summary>
        /// Setup UI and start loop
        /// </summary>
        /// <returns></returns>
        private IEnumerator LaunchGame()
        {
            _game.Display = "Start Game";
            yield return new WaitForSeconds(2);

            _game.Display = "Difficulty : " + _difficulty.Name;
            yield return new WaitForSeconds(2);

            _game.Display = "Score : 0";
            _game.StartCoroutine(Update());
        }

        /// <summary>
        /// Loop to summon moll until life is down
        /// </summary>
        /// <returns></returns>
        private IEnumerator Update()
        {
            float difficulty = _difficulty.DifficultyMultiplier;
            float spawnRate = _difficulty.SpawnRate;

            while (_life > 0)
            {
                bool spawn = UnityEngine.Random.Range(0, 100) < _difficulty.SpawnRate;
                if (spawn)
                {
                    SpawnAMoll(difficulty, _difficulty.PrefabsMolls, _game.Spawner.PositionsAvailable);
                }

                // The more the game is difficult, the more molls are spawned
                yield return new WaitForSeconds(1/difficulty);
            }

            _game.Display = "End of the game";

            yield return new WaitForSeconds(2);

            _game.EndGame();
        }

        /// <summary>
        /// Spawn a moll
        /// </summary>
        /// <param name="difficultyMultiplier"></param>
        /// <param name="prefabsMolls"></param>
        /// <param name="positionsAvailable"></param>
        private void SpawnAMoll(float difficultyMultiplier, List<GameObject> prefabsMolls, List<Transform> positionsAvailable)
        {
            if (_game.Spawner.PositionsAvailable.Count <= 0) return;

            int randomTargetIndex = UnityEngine.Random.Range(0, prefabsMolls.Count);
            int randomPositionIndex = UnityEngine.Random.Range(0, positionsAvailable.Count);
            Transform randomPosition = positionsAvailable[randomPositionIndex];
            GameObject _target = _game.Spawner.Spawn<GameObject>(randomPositionIndex, prefabsMolls[randomTargetIndex]);

            if (_target != null)
            {
                OnMollBirth onMollBirth = new OnMollBirth();
                onMollBirth.Multiplier = difficultyMultiplier;
                onMollBirth.IdMoll = _target.GetComponent<Target>().ID;
                onMollBirth.Game = _game;
                onMollBirth.Waypoints = positionsAvailable.ToArray();
                onMollBirth.SpawnPosition = randomPosition;


                EventBus<OnMollBirth>.Raise(onMollBirth);
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Dispawn moll when dying
        /// </summary>
        /// <param name="e"></param>
        private void OnMollDying(OnMollDying e)
        {
            _life -= e.Damage;

            Moll target = e.Moll.GetComponent<Moll>();
            GameObject.Instantiate(target.EffectOnDying, target.transform);

            OnDispawnRequestSend request = new OnDispawnRequestSend();
            request.ObjectToDispawn = e.Moll;
            request.Spawner = _game.Spawner;

            EventBus<OnDispawnRequestSend>.Raise(request);
        }

        #endregion
    }
}
