using Assets.Scripts.Core.AI.BehaviourTrees;
using Assets.Scripts.Core.AI.BehaviourTrees.Nodes.SpecificNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.AI
{
    public class BaseTargetBT : Tree
    {
        public UnityEngine.Transform[] Waypoints;
        public float Speed;
        public float WaitTime;
        public float DistanceToMove;

        private UnityEngine.Transform[] _waypointsUsed;

        protected override Node SetupTree()
        {
            _waypointsUsed = new UnityEngine.Transform[2];
            _waypointsUsed[0] = new UnityEngine.GameObject().transform;
            _waypointsUsed[0].position = gameObject.transform.position;

            int randomDirection = UnityEngine.Random.Range(0, 2);
            UnityEngine.Vector3 direction = new UnityEngine.Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
            direction.Normalize();
            UnityEngine.Vector3 position = gameObject.transform.position + direction * DistanceToMove;
            UnityEngine.Transform transformNext = new UnityEngine.GameObject().transform;

            transformNext.position = position;
            _waypointsUsed[1] = transformNext;

            UnityEngine.Debug.Log("Waypoint 1: " + _waypointsUsed[0].position);
            UnityEngine.Debug.Log("Waypoint 2: " + _waypointsUsed[1].position);


            Node root = new TaskPatrol(gameObject.transform, _waypointsUsed, Speed, WaitTime, false);

            return root;
        }

        public UnityEngine.Transform[] WaypointsUsed { get { return _waypointsUsed; } }

        public void OnDestroy()
        {
            foreach (UnityEngine.Transform waypoint in _waypointsUsed)
            {
                UnityEngine.GameObject.Destroy(waypoint.gameObject);
            }
        }
    }
}
