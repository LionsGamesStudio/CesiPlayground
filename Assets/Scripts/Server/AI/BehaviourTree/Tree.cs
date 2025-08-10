using CesiPlayground.Core.Storing;

namespace CesiPlayground.Server.AI.BehaviourTree
{
    /// <summary>
    /// A non-MonoBehaviour class that represents and runs a Behaviour Tree.
    /// It is owned by a server-side AI entity.
    /// </summary>
    public abstract class Tree
    {
        private Node _root = null;
        public Blackboard Blackboard { get; private set; } = new Blackboard();
        
        /// <summary>
        /// Initializes the tree and sets up the blackboard for all nodes.
        /// </summary>
        public void Initialize()
        {
            _root = SetupTree();
            _root.SetBlackboard(Blackboard);
        }

        /// <summary>
        /// Called by the owning AI entity on every server tick.
        /// </summary>
        /// <param name="deltaTime">The time since the last server tick.</param>
        public void Update(float deltaTime)
        {
            _root?.Evaluate(deltaTime);
        }

        /// <summary>
        /// Must be implemented by a concrete tree to define its structure.
        /// </summary>
        /// <returns>The root node of the tree.</returns>
        protected abstract Node SetupTree();
    }
}