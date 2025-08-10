using CesiPlayground.Shared.Data.Scoreboards;
using UnityEngine;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A client-side view component responsible for displaying a scoreboard.
    /// It receives a Scoreboard data object and populates the UI accordingly.
    /// </summary>
    public class ScoreboardView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject scoreboardsHolder;
        [SerializeField] private GameObject rowBoardPrefab;

        /// <summary>
        /// Clears and populates the scoreboard UI with new data.
        /// </summary>
        /// <param name="scoreboard">The scoreboard data to display.</param>
        public void DisplayScoreboard(Scoreboard scoreboard)
        {
            // 1. Clear the old scoreboard
            foreach (Transform child in scoreboardsHolder.transform)
            {
                Destroy(child.gameObject);
            }

            if (scoreboard?.Rows == null) return;

            // 2. Fill with the new scoreboard data
            foreach (RowBoard rowData in scoreboard.Rows)
            {
                GameObject newRow = Instantiate(rowBoardPrefab, scoreboardsHolder.transform);
                RowBoardUI rowUI = newRow.GetComponent<RowBoardUI>();
                if (rowUI != null)
                {
                    rowUI.Position.text = (rowData.Position + 1).ToString(); // Display 1-based index
                    rowUI.Name.text = rowData.PlayerName;
                    rowUI.Score.text = rowData.Score.ToString("F0");
                }
            }
        }
    }
}