using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Scoring
{
    [Serializable]
    public class Scoreboard
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        public string ID;

        /// <summary>
        /// Rows of the scoreboard
        /// </summary>
        public List<RowBoard> Rows = new List<RowBoard>();

        /// <summary>
        /// Path to the file containing the data
        /// </summary>
        public string Filepath;

        /// <summary>
        /// Add a score in the scoreboard
        /// </summary>
        /// <param name="rowBoard">Row with the score to add</param>
        public void AddScore(RowBoard rowBoard)
        {
            if (IsNewScore(rowBoard))
            {
                Rows.Add(rowBoard);
            }
            else if(IsSuperiorScore(rowBoard))
            {
                RowBoard oldRow = Rows.Find(obj => obj.HashID == rowBoard.HashID);
                Rows[Rows.IndexOf(oldRow)] = rowBoard;
            }

            if(Rows == null) Rows = new List<RowBoard>();

            SortRows();
        }

        #region Actions on rows list

        /// <summary>
        /// Sort the rows by score in descending order
        /// </summary>
        public void SortRows()
        {
            if (Rows.Count <= 0) return;

            Rows = Rows.OrderByDescending(obj => obj.Score).ToList();
            for(int i = 0; i < Rows.Count; i++)
            {
                Rows[i].Pos = i;
            }
        }

        /// <summary>
        /// Check if it's new score
        /// </summary>
        /// <param name="rowBoard"></param>
        /// <returns></returns>
        private bool IsNewScore(RowBoard rowBoard)
        {
            foreach (RowBoard col in Rows)
            {
                if(col.HashID == rowBoard.HashID)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if score is better than last one
        /// </summary>
        /// <param name="rowBoard">Row of the actual score</param>
        /// <returns></returns>
        private bool IsSuperiorScore(RowBoard rowBoard)
        {
            foreach (RowBoard col in Rows)
            {
                if (col.HashID == rowBoard.HashID && col.Score < rowBoard.Score)
                    return true;
            }

            return false;
        }

        #endregion

    }
}
