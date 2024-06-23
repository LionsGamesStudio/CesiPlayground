using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.AI.BehaviourTrees.Nodes
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;

            foreach (Node node in _children)
            {
                switch (node.Evaluate())
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
