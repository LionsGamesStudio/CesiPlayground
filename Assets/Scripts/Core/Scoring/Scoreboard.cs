using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Scoring
{
    [Serializable]
    public class Scoreboard
    {
        public string ID;
        public List<RowBoard> Columns = new List<RowBoard>();
        public string Filepath;

        public void AddScore(RowBoard rowBoard)
        {
            if (IsNewScore(rowBoard))
            {
                Columns.Add(rowBoard);
            }
            else if(IsSuperiorScore(rowBoard))
            {
                RowBoard oldRow = Columns.Find(obj => obj.HashID == rowBoard.HashID);
                Columns[Columns.IndexOf(oldRow)] = rowBoard;
            }

            if(Columns == null) Columns = new List<RowBoard>();

            SortColumns();
        }

        public void SortColumns()
        {
            if (Columns.Count <= 0) return;

            Columns = Columns.OrderByDescending(obj => obj.Score).ToList();
            for(int i = 0; i < Columns.Count; i++)
            {
                Columns[i].Pos = i;
            }
        }

        private bool IsNewScore(RowBoard rowBoard)
        {
            foreach (RowBoard col in Columns)
            {
                if(col.HashID == rowBoard.HashID)
                    return false;
            }

            return true;
        }

        private bool IsSuperiorScore(RowBoard rowBoard)
        {
            foreach (RowBoard col in Columns)
            {
                if (col.HashID == rowBoard.HashID && col.Score < rowBoard.Score)
                    return true;
            }

            return false;
        }


    }
}
