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

        private IEnumerator LaunchGame()
        {
            _game.Display = "Start Game";
            yield return new WaitForSeconds(2);

            _game.Display = "Difficulty : " + _difficulty.Name;
            yield return new WaitForSeconds(2);

            _game.Display = "Score : 0";
            _game.StartCoroutine(Update());
        }

        private IEnumerator Update()
        {
            float difficulty = _difficulty.DifficultyMultiplier;

            while(_life > 0)
            {
                int numberOfTargetToSpawn = (int)_difficulty.NumberOfEnnemies.Evaluate(Time.deltaTime);

                for (int i = 0; i < numberOfTargetToSpawn; i++)
                {
                    if(_game.Spawner.PositionsAvailable.Count <= 0) break;

                    int randomTargetIndex = UnityEngine.Random.Range(0, _difficulty.PrefabsMolls.Count);
                    int randomPositionIndex = UnityEngine.Random.Range(0, _game.Spawner.PositionsAvailable.Count);
                    GameObject _target = _game.Spawner.Spawn<GameObject>(randomPositionIndex, _difficulty.PrefabsMolls[randomTargetIndex]);

                    if (_target != null)
                    {
                        OnMollBirth onMollBirth = new OnMollBirth();
                        onMollBirth.Multiplier = difficulty;
                        onMollBirth.IdMoll = _target.GetComponent<Target>().ID;
                        onMollBirth.OutsideRate = _difficulty.GoOutsideRate;
                        onMollBirth.Game = _game;

                        EventBus<OnMollBirth>.Raise(onMollBirth);
                    }
                }

                yield return new WaitUntil(() => _game.Spawner.SpawnPosibilitiesAlreadyUsed.Count <= 0);
                float nextDiff = difficulty - (_difficulty.MultiplierReducer * UnityEngine.Random.Range(1, 10));
                difficulty = Mathf.Max(nextDiff, 0.01f, nextDiff);
            }

            _game.Display = "End of the game";

            yield return new WaitForSeconds(2);

            _game.EndGame();
        }

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
    }
}
