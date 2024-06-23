using Assets.Scripts.Core.AI.BehaviourTrees;
using Assets.Scripts.Core.AI.BehaviourTrees.Nodes.SpecificNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.AI
{
    public class BaseMollBT : Tree
    {
        public UnityEngine.Transform[] Waypoints;
        public UnityEngine.Transform SpawnPosition;
        public float Speed;
        public float WaitTime;

        private UnityEngine.Transform[] _waypointsUsed;

        protected override Node SetupTree()
        {
            _waypointsUsed = new UnityEngine.Transform[2];
            _waypointsUsed[0] = new UnityEngine.GameObject().transform;
            _waypointsUsed[0].position = SpawnPosition.position;

            UnityEngine.Transform transformUp = new UnityEngine.GameObject().transform;
            transformUp.position = new UnityEngine.Vector3(SpawnPosition.position.x, SpawnPosition.position.y + 0.5f, SpawnPosition.position.z);
            _waypointsUsed[1] = transformUp;

            Node root = new TaskPatrol(gameObject.transform, _waypointsUsed, Speed, WaitTime, true);

            return root;
        }

        public UnityEngine.Transform[] WaypointsUsed { get { return _waypointsUsed; } }
    }
}
