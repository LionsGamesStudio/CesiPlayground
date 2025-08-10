using UnityEngine;
using CesiPlayground.Core.Storing;
using CesiPlayground.Shared.Games;

namespace CesiPlayground.Client.UI.GameUI
{
    /// <summary>
    /// A reusable client-side UI component that allows a player to select a difficulty
    /// and writes that choice to the client's shared Blackboard.
    /// </summary>
    public class ChangeDifficultyUI : MonoBehaviour
    {
        [Tooltip("A unique key for this game's difficulty setting in the Blackboard (e.g., 'MollSmash_Difficulty').")]
        [SerializeField] private string difficultyBlackboardKey;

        private void Start()
        {
            SetDifficulty("Normal");
        }

        /// <summary>
        /// To be called from UI Buttons. The 'difficulty' parameter should be
        /// the string name of the enum value (e.g., "Easy", "Normal", "Hard").
        /// </summary>
        public void SetDifficulty(string difficultyString)
        {
            if (string.IsNullOrEmpty(difficultyBlackboardKey))
            {
                Debug.LogError("Difficulty Blackboard Key is not set in the inspector!", this);
                return;
            }

            if (System.Enum.TryParse<GameDifficulty>(difficultyString, out GameDifficulty newDifficulty))
            {
                // Write the selected difficulty to the client's global blackboard.
                BlackboardHub.Client.Set(difficultyBlackboardKey, newDifficulty);
                Debug.Log($"[ChangeDifficultyUI] Set '{difficultyBlackboardKey}' to '{newDifficulty}' in Client Blackboard.");
            }
        }
    }
}