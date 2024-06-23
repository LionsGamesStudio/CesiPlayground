using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.AI.BehaviourTrees.Nodes.SpecificNodes
{
    public class TaskPatrol : Node
    {
        // The transform of the agent
        private Transform _transform;

        // The waypoints to patrol
        private Transform[] _waypoints;

        // Patrol variables
        private int _currentWaypointIndex = 0;
        private float _speed = 1.0f;
        private bool _rotateToward = true;

        private float _waitTime = 1.0f;
        private float _waitCounter = 0.0f;
        private bool _waiting = false;


        public TaskPatrol(Transform transform, Transform[] waypoints, float speed, float waitTime, bool rotateToward) : base()
        {
            _transform = transform;
            _waypoints = waypoints;
            _speed = speed;
            _waitTime = waitTime;
            _rotateToward = rotateToward;
        }

        public override NodeState Evaluate()
        {
            Debug.Log(_waiting);
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if(_waitCounter >= _waitTime)
                    _waiting = false;
            }
            else
            {
                Transform wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.0001f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0.0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                }
                else
                {
                    _transform.position = Vector3.MoveTowards(_transform.position, wp.position, _speed * Time.deltaTime);
                    if (_rotateToward)
                        _transform.LookAt(wp.position);
                }
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}

