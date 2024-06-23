using Newtonsoft.Json;
using System;

namespace Assets.Scripts.Core.Scoring
{
    /// <summary>
    /// Row of a scoreboard
    /// </summary>
    [Serializable]
    public class RowBoard
    {
        public int Pos;
        public string HashID;
        public string PlayerName;
        public float Score;
        
        public RowBoard(int pos, string playerName, float score)
        {
            HashID = Guid.NewGuid().ToString();
            Pos = pos;
            PlayerName = playerName;
            Score = score;
        }

        [JsonConstructor]
        public RowBoard(int pos, string id, string playerName, float score)
        {
            HashID = id;
            Pos = pos;
            PlayerName = playerName;
            Score = score;
        }
    }
}
