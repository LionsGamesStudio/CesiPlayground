using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.Players;
using Assets.Scripts.Core.Events.UI;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Process.Events;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Core.Games
{
    public class Game : MonoBehaviour
    {
        private static int _numberOfInstance = 0;

        private bool _startedGame = false;
        private float _score = 0;
        private string _displayText = "Score : 0";

        private EventBinding<OnScoreEvent> _scoreEventBinding;

        private EventBinding<PlayerEnterGameZoneEvent> _playerEnterGameZoneBinding;
        private EventBinding<PlayerLeaveGameZoneEvent> _playerLeaveGameZoneBinding;

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

            Display = _displayText;

            // Events
            _scoreEventBinding = new EventBinding<OnScoreEvent>(OnScoreEvent);

            _playerEnterGameZoneBinding = new EventBinding<PlayerEnterGameZoneEvent>(OnPlayerEnter);
            _playerLeaveGameZoneBinding = new EventBinding<PlayerLeaveGameZoneEvent>(OnPlayerLeave);

            EventBus<OnScoreEvent>.Register(_scoreEventBinding);
            EventBus<PlayerEnterGameZoneEvent>.Register(_playerEnterGameZoneBinding);
            EventBus<PlayerLeaveGameZoneEvent>.Register(_playerLeaveGameZoneBinding);
        }

        #region Game Steps

        /// <summary>
        /// Start the logic of the game which is in the strategy
        /// </summary>
        public void StartGame()
        {
            if (!_startedGame && CurrentPlayer != null)
            {
                _startedGame = true;
                GameStrategy.StartGame();
            }  
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public void ResetGame()
        {
            if (!_startedGame)
            {
                GameStrategy.ResetGame();
            }
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

        #endregion


        #region Events

        /// <summary>
        /// Send final score on the game to the corresponding scoreboard
        /// </summary>
        /// <param name="scoreEvent"></param>
        private void OnScoreEvent(OnScoreEvent scoreEvent)
        {
            if (scoreEvent.GameId == Data.GameId && scoreEvent.PlayerData.PlayerId == CurrentPlayer.PlayerData.PlayerId)
            {
                _score += scoreEvent.Points;
                Display = "Score : " + _score;
            }
        }

        /// <summary>
        /// Add the player to the game
        /// </summary>
        /// <param name="e"></param>
        private void OnPlayerEnter(PlayerEnterGameZoneEvent e)
        {
            if (e.Game.Data.GameId == Data.GameId && CurrentPlayer == null)
                CurrentPlayer = e.Player;
        }

        /// <summary>
        /// Remove the player from the game
        /// </summary>
        /// <param name="e"></param>
        private void OnPlayerLeave(PlayerLeaveGameZoneEvent e)
        {
            if (e.Game.Data.GameId == Data.GameId && CurrentPlayer != null)
                CurrentPlayer = null;
        }

        #endregion

        #region Getters and Setters

        public bool StartedGame
        {
            get { return _startedGame; }
            set { _startedGame = value; }
        }

        public float Score
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

        #endregion
    }
}
