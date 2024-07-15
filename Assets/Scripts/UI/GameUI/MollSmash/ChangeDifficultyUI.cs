using Assets.Scripts.Core.Games;
using Assets.Scripts.Process.MollSmash.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Process.MollSmash.Main.MollSmashStrategy;

namespace Assets.Scripts.UI.GameUI.MollSmash
{
    /// <summary>
    /// UI displaying button to change the difficulty of a game
    /// </summary>
    public class ChangeDifficultyUI : MonoBehaviour
    {
        public Game Game;

        #region Change Difficulty Functions

        /// <summary>
        /// Change Moll Smash Difficulty
        /// </summary>
        /// <param name="difficulty">String of the enum name of the difficulty</param>
        public void ChangeDifficultyMollSmash(string difficulty)
        {
            IGameDifficulty iDifficulty = Game.GameStrategy as IGameDifficulty;

            if (iDifficulty != null)
            {
                GameDifficulty eDifficulty = (GameDifficulty)Enum.Parse(typeof(GameDifficulty), difficulty);

                iDifficulty.SetDifficulty(eDifficulty);
            }
        }

        #endregion
    }
}
