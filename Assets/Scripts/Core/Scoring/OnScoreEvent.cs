using Assets.Scripts.Core.Players;
using Assets.Scripts.Core.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Scoring
{
    /// <summary>
    /// Event sent when scoring
    /// </summary>
    public class OnScoreEvent : IEvent
    {
        public string GameId;
        public PlayerData PlayerData = new PlayerData();
        public float Points;
    }
}
