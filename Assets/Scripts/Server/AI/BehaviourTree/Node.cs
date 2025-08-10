using System.Collections.Generic;
using CesiPlayground.Core.Storing;

namespace CesiPlayground.Server.AI.BehaviourTree
{
    public enum NodeState { RUNNING, SUCCESS, FAILURE }

    public abstract class Node
    {
        protected NodeState _state;
        public Node Parent { get; private set; }
        protected List<Node> _children = new List<Node>();

        protected Blackboard _blackboard; 
        
        public Node()
        {
            Parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }

        private void Attach(Node node)
        {
            node.Parent = this;
            _children.Add(node);
        }

        /// <summary>
        /// The main evaluation method, now accepts a delta time.
        /// </summary>
        /// <param name="deltaTime">The time since the last update.</param>
        /// <returns>The state of the node.</returns>
        public abstract NodeState Evaluate(float deltaTime);

        /// <summary>
        /// Sets the blackboard instance for this node and all its children.
        /// This is called once by the root Tree.
        /// </summary>
        public virtual void SetBlackboard(Blackboard blackboard)
        {
            _blackboard = blackboard;
            foreach (var child in _children)
            {
                child.SetBlackboard(blackboard);
            }
        }
    }
}