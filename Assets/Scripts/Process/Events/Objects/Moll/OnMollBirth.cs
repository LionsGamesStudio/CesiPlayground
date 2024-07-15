using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Games;
using UnityEngine;

namespace Assets.Scripts.Process.Events.Objects.MollEvent
{
    /// <summary>
    /// Event sent when a moll is instantiated
    /// </summary>
    public class OnMollBirth : IEvent
    {
        public Game Game;
        public int IdMoll;
        public float Multiplier;
        public Transform[] Waypoints;
        public Transform SpawnPosition;
    }
}
