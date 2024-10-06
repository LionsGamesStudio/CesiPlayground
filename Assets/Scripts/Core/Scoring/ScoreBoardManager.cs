using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Games;
using Assets.Scripts.Core.Storing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Scoring
{

    public class ScoreBoardManager : MonoBehaviour
    {
        [Header("Scoreboards")]
        public GameObject ScoreboardsHolder;
        public GameObject RowBoardPrefab;

        /// <summary>
        /// The games we want to have a scoreboard
        /// </summary>
        public List<DataGame> ListOfGame = new List<DataGame>();

        [Header("Storage")]
        public StorageManager StorageManager;

        private Scoreboard scoreboardInUse;
        private string storageFolder = "";

        private bool setupDone = false;

        private EventBinding<OnGameFinished> gameFinishedEventBinding;


        public void Awake()
        {
            if(!setupDone)
                Setup();
        }

        public void Setup()
        {
            storageFolder = Application.persistentDataPath + "/Score/";
            if (!System.IO.Directory.Exists(storageFolder))
                System.IO.Directory.CreateDirectory(storageFolder);

            // Binding
            gameFinishedEventBinding = new EventBinding<OnGameFinished>(ReactScoring);
            EventBus<OnGameFinished>.Register(gameFinishedEventBinding);

            setupDone = true;
        }

        #region Scoreboard

        /// <summary>
        /// Add a score to the current scoreboard
        /// </summary>
        /// <param name="pos">Position of the score</param>
        /// <param name="name">Name of the player</param>
        /// <param name="score">Score of the player</param>
        public void AddScore(int pos, string name, int score)
        {
            if (name == "")
            {
                name = "Unknown";
            }
            RowBoard newScore = new RowBoard(pos, name, score);

            scoreboardInUse.AddScore(newScore);

            DisplayUI(scoreboardInUse);
        }

        /// <summary>
        /// Change the scoreboard to use
        /// </summary>
        /// <param name="index"></param>
        public void ChangeScoreBoard(int index)
        {
            scoreboardInUse = LoadScoreboard(ListOfGame[index]);
        }

        /// <summary>
        /// Load data of a scoreboard
        /// </summary>
        /// <param name="dataGame">The data of the game we want to load the scoreboard</param>
        /// <returns></returns>
        private Scoreboard LoadScoreboard(DataGame dataGame)
        {
            string boardFilename = dataGame.GameScoreboardFileName;
            string nameTmp = boardFilename.Split('.')[0] + ".json";
            Scoreboard scoreboard = new Scoreboard();
            scoreboard.ID = dataGame.GameId;
            scoreboard.Filepath = storageFolder + nameTmp;
            scoreboard.Rows = StorageManager.Get<List<RowBoard>>(scoreboard.Filepath);

            if(scoreboard.Rows == null) scoreboard.Rows = new List<RowBoard>();

            scoreboard.SortRows();

            DisplayUI(scoreboard);

            return scoreboard;
        }

        #endregion

        #region UI

        /// <summary>
        /// Display UI of the scoreboard
        /// </summary>
        /// <param name="scoreboard"></param>
        private void DisplayUI(Scoreboard scoreboard)
        {
            // Clear the old scoreboard
            int size = ScoreboardsHolder.transform.childCount;
            for (int i = 0; i < size; i++)
            {
                Destroy(ScoreboardsHolder.transform.GetChild(i).gameObject);
            }

            if (scoreboard == null) return;

            // Fill with new scoreboard
            foreach (RowBoard column in scoreboard.Rows)
            {
                GameObject newRow = Instantiate(RowBoardPrefab, ScoreboardsHolder.transform);
                newRow.GetComponent<RowBoardUI>().Position.text = column.Pos.ToString();
                newRow.GetComponent<RowBoardUI>().Name.text = column.PlayerName;
                newRow.GetComponent<RowBoardUI>().Score.text = column.Score.ToString();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// React to a score of a game
        /// </summary>
        /// <param name="e"></param>
        private void ReactScoring(OnGameFinished e)
        {
            int i = 0;
            foreach(DataGame data in ListOfGame)
            {
                Scoreboard scoreboard = LoadScoreboard(data);
                if (scoreboard != null && scoreboard.ID == e.GameId)
                {
                    RowBoard row = new RowBoard(0, e.Player.PlayerData.PlayerId, e.Player.PlayerData.PlayerName, e.Score);
                    scoreboard.AddScore(row);

                    StorageManager.Store<List<RowBoard>>(scoreboard.Filepath, scoreboard.Rows);

                    ChangeScoreBoard(i);
                }

                i++;
            }
        }

        #endregion

    }
}
