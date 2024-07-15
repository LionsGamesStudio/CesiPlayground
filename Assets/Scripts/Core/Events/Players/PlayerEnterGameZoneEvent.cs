using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Events.Players
{
    public class PlayerEnterGameZoneEvent : IEvent
    {
        public Player Player;
        public Game Game;
    }
}
