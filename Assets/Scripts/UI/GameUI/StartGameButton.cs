using Assets.Scripts.Core;
using Assets.Scripts.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameUI
{
    /// <summary>
    /// Button to start a game
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        public Player Player;
        public Game Game;

        public void SendOnClick()
        {
            Game.StartGame(Player);
        }
    }
}
