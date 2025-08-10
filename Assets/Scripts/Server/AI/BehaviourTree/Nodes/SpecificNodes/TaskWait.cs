

namespace CesiPlayground.Server.AI.BehaviourTree.Nodes.SpecificNodes
{
    public class TaskWait : Node
    {
        private float _waitTime;
        private float _waitCounter = 0f;

        public TaskWait(float waitTime)
        {
            _waitTime = waitTime;
        }

        public override NodeState Evaluate(float deltaTime)
        {
            _waitCounter += deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waitCounter = 0; // Reset for next time
                _state = NodeState.SUCCESS;
                return _state;
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}