using UnityEngine;

namespace CesiPlayground.Server.AI.BehaviourTree.Nodes.SpecificNodes
{
    public class TaskPatrol : Node
    {
        private Vector3[] _waypoints;
        private float _speed;
        private int _currentWaypointIndex = 0;

        public TaskPatrol(Vector3[] waypoints, float speed)
        {
            _waypoints = waypoints;
            _speed = speed;
        }

        public override NodeState Evaluate(float deltaTime)
        {
            // If there are no waypoints, the task succeeds immediately.
            if (_waypoints == null || _waypoints.Length == 0)
            {
                _state = NodeState.SUCCESS;
                return _state;
            }

            // Get the current state from the blackboard.
            Vector3 currentPosition = _blackboard.Get<Vector3>("Position");
            Vector3 targetPosition = _waypoints[_currentWaypointIndex];

            // Check if the agent has reached the target waypoint.
            if (Vector3.Distance(currentPosition, targetPosition) < 0.01f)
            {
                _currentWaypointIndex++;
                // If all waypoints have been visited, the patrol is successful.
                if (_currentWaypointIndex >= _waypoints.Length)
                {
                    _currentWaypointIndex = 0; // Or reset, depending on desired behavior
                    _state = NodeState.SUCCESS;
                    return _state;
                }
                // Update the target for the next frame.
                targetPosition = _waypoints[_currentWaypointIndex];
            }
            
            // Move towards the target.
            Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, _speed * deltaTime);
            
            // Write the new desired state back to the blackboard.
            _blackboard.Set("Position", newPosition);
            _blackboard.Set("Rotation", Quaternion.LookRotation(targetPosition - currentPosition));

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}