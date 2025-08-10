using System.Collections.Generic;


namespace CesiPlayground.Server.AI.BehaviourTree.Nodes
{
    public class Sequence : Node
    {
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate(float deltaTime)
        {
            bool anyChildRunning = false;
            foreach (Node node in _children)
            {
                switch (node.Evaluate(deltaTime))
                {
                    case NodeState.FAILURE:
                        _state = NodeState.FAILURE;
                        return _state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        _state = NodeState.SUCCESS;
                        return _state;
                }
            }
            _state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return _state;
        }
    }
}