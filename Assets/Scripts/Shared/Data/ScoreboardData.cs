using System;
using System.Collections.Generic;

namespace CesiPlayground.Shared.Data.Scoreboards
{
    /// <summary>
    /// Represents a single entry in a scoreboard.
    /// This is a pure data container (DTO).
    /// </summary>
    [Serializable]
    public class RowBoard
    {
        public int Position;
        public string PlayerId;
        public string PlayerName;
        public float Score;
    }
    
    /// <summary>
    /// Represents a full scoreboard for a specific game.
    /// </summary>
    [Serializable]
    public class Scoreboard
    {
        public string GameId;
        public List<RowBoard> Rows = new List<RowBoard>();
    }
}