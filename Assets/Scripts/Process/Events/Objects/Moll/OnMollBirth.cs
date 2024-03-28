using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Game;
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
        public int OutsideRate;
    }
}
