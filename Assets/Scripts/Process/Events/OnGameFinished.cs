using Assets.Scripts.Core;
using Assets.Scripts.Core.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.Events
{
    public class OnGameFinished : IEvent
    {
        public string GameId;
        public Player Player;
        public int Score;
    }
}
