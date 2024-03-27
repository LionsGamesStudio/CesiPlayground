using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.Events.Objects.MollEvent
{
    public class OnMollDying : IEvent
    {
        public Game Game;
        public GameObject Moll;
        public float Damage;
    }
}
