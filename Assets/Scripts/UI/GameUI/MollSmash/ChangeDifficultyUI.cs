using Assets.Scripts.Core.Game;
using Assets.Scripts.Process.MollSmash.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Process.MollSmash.Game.MollSmashStrategy;

namespace Assets.Scripts.UI.GameUI.MollSmash
{
    public class ChangeDifficultyUI : MonoBehaviour
    {
        public Game Game;

        public void ChangeDifficulty(string difficulty)
        {
            MollSmashStrategy strat = (MollSmashStrategy)Game.GameStrategy;

            if(strat != null)
            {
                MollSmashDifficulty dif = (MollSmashDifficulty)Enum.Parse(typeof(MollSmashDifficulty), difficulty);

                strat.SetDifficulty(dif);
            }
        }
    }
}
