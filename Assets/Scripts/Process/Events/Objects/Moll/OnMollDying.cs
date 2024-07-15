using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.Events.Objects.MollEvent
{
    /// <summary>
    /// Event sent when a moll die
    /// </summary>
    public class OnMollDying : IEvent
    {
        public Game Game;
        public GameObject Moll;
        public float Damage;
    }
}
