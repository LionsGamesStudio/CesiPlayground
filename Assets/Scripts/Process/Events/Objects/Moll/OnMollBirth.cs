using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Game;
using UnityEngine;

namespace Assets.Scripts.Process.Events.Objects.MollEvent
{
    public class OnMollBirth : IEvent
    {
        public Game Game;
        public int IdMoll;
        public float Multiplier;
        public int OutsideRate;
    }
}
