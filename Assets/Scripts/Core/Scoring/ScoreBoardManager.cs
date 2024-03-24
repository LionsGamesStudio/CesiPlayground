using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Game;
using Assets.Scripts.Core.Storing;
using Assets.Scripts.Process.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Scoring
{

    public class ScoreBoardManager : Singleton<ScoreBoardManager>
    {
        [Header("Scoreboards")]
        public GameObject ScoreboardsHolder;
        public GameObject RowBoardPrefab;

        public List<DataGame> ListOfGame = new List<DataGame>();

        [Header("Storage")]
        public StorageManager StorageManager;

        private Scoreboard scoreboardInUse;
        private string storageFolder = "";

        private EventBinding<OnGameFinished> gameFinishedEventBinding;


        public void Start()
        {
            storageFolder = Application.persistentDataPath + "/Score/";
            if(!System.IO.Directory.Exists(storageFolder))
                System.IO.Directory.CreateDirectory(storageFolder);

            // Binding
            gameFinishedEventBinding = new EventBinding<OnGameFinished>(ReactScoring);
            EventBus<OnGameFinished>.Register(gameFinishedEventBinding);
        }

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

        public void ChangeScoreBoard(int index)
        {
            scoreboardInUse = LoadScoreboard(ListOfGame[index]);
        }

        private Scoreboard LoadScoreboard(DataGame dataGame)
        {
            string boardFilename = dataGame.GameScoreboardFileName;
            string nameTmp = boardFilename.Split('.')[0] + ".json";
            Scoreboard scoreboard = new Scoreboard();
            scoreboard.ID = dataGame.GameId;
            scoreboard.Filepath = storageFolder + nameTmp;
            scoreboard.Columns = StorageManager.Get<List<RowBoard>>(scoreboard.Filepath);

            scoreboard.SortColumns();

            DisplayUI(scoreboard);

            return scoreboard;
        }

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
            foreach (RowBoard column in scoreboard.Columns)
            {
                GameObject newRow = Instantiate(RowBoardPrefab, ScoreboardsHolder.transform);
                newRow.GetComponent<RowBoardUI>().Position.text = column.Pos.ToString();
                newRow.GetComponent<RowBoardUI>().Name.text = column.PlayerName;
                newRow.GetComponent<RowBoardUI>().Score.text = column.Score.ToString();
            }
        }

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

                    StorageManager.Store<List<RowBoard>>(scoreboard.Filepath, scoreboard.Columns);

                    ChangeScoreBoard(i);
                }

                i++;
            }
        }

        

    }
}
