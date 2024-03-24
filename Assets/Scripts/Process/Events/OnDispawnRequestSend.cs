using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Spawn.Spawners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.Events
{
    public class OnDispawnRequestSend : IEvent
    {
        public GameObject ObjectToDispawn;
        public Spawner Spawner;
        public float TimeBeforeDispawn;
    }
}
