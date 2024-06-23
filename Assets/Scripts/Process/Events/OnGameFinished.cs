using Assets.Scripts.Core;
using Assets.Scripts.Core.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.Events
{
    /// <summary>
    /// Event sent when a game is finished
    /// </summary>
    public class OnGameFinished : IEvent
    {
        public string GameId;
        public Player Player;
        public float Score;
    }
}
