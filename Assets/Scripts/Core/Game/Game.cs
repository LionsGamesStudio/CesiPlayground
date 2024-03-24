using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Process.Events;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Core.Game
{
    public class Game : MonoBehaviour
    {
        private static int _numberOfInstance = 0;

        private bool _startedGame = false;
        private int _score = 0;
        private string _displayText = "Score : 0";

        private EventBinding<OnScoreEvent> _scoreEventBinding;

        [NonSerialized]
        public string ID;

        [Header("Game Settings")]
        public DataGame Data;
        public IGameStrategy GameStrategy;
        public Spawner Spawner;
        public Player CurrentPlayer;

        [Header("UI")]
        public TMP_Text ScoreText;

        private void Awake()
        {
            ID = _numberOfInstance.ToString();
            _numberOfInstance++;
            DontDestroyOnLoad(this.gameObject);

            // Events
            _scoreEventBinding = new EventBinding<OnScoreEvent>(OnScoreEvent);
            EventBus<OnScoreEvent>.Register(_scoreEventBinding);
        }

        /// <summary>
        /// Start the logic of the game which is in the strategy
        /// </summary>
        public void StartGame(Player player)
        {
            if (!_startedGame)
            {
                CurrentPlayer = player;
                GameStrategy.StartGame();
                _startedGame = true;

                Debug.Log(CurrentPlayer.PlayerData.PlayerName);

                Display = "Score : 0";
            }  
        }

        public void ResetGame()
        {
            GameStrategy.ResetGame();
        }

        /// <summary>
        /// End the game
        /// </summary>
        public void EndGame()
        {
            if(_startedGame)
            {
                GameStrategy.EndGame();
                _startedGame = false;

                Display = "Score : 0";

                OnGameFinished onGameFinished = new OnGameFinished();
                onGameFinished.Player = CurrentPlayer;
                onGameFinished.GameId = Data.GameId;
                onGameFinished.Score = _score;

                EventBus<OnGameFinished>.Raise(onGameFinished);
            }
        }

        private void OnScoreEvent(OnScoreEvent scoreEvent)
        {
            if (scoreEvent.GameId == Data.GameId && scoreEvent.PlayerData.PlayerId == CurrentPlayer.PlayerData.PlayerId)
            {
                _score += scoreEvent.Points;
                Display = "Score : " + _score;
            }
        }

        // GETTERS AND SETTERS
        public bool StartedGame
        {
            get { return _startedGame; }
            set { _startedGame = value; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public string GameName { get; set; }

        public string Display
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
                ScoreText.text = _displayText;
            }
        }

    }
}
