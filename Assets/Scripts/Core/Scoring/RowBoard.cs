using Newtonsoft.Json;
using System;

namespace Assets.Scripts.Core.Scoring
{
    [Serializable]
    public class RowBoard
    {
        public int Pos;
        public string HashID;
        public string PlayerName;
        public int Score;
        
        public RowBoard(int pos, string playerName, int score)
        {
            HashID = Guid.NewGuid().ToString();
            Pos = pos;
            PlayerName = playerName;
            Score = score;
        }

        [JsonConstructor]
        public RowBoard(int pos, string id, string playerName, int score)
        {
            HashID = id;
            Pos = pos;
            PlayerName = playerName;
            Score = score;
        }
    }
}
