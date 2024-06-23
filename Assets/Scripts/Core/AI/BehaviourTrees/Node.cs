using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.AI.BehaviourTrees
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState _state;

        public Node parent;
        protected List<Node> _children = new List<Node>();

        private Dictionary<string, object> _blackboard = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                _Attach(child);
            }
        }

        /// <summary>
        /// Add a child to the node
        /// </summary>
        /// <param name="node">Child to add</param>
        private void _Attach(Node node)
        {
            node.parent = this;
            _children.Add(node);
        }

        /// <summary>
        /// Evaluate the state of the node
        /// </summary>
        /// <returns></returns>
        public virtual NodeState Evaluate() => NodeState.FAILURE;


        #region Blackboard

        /// <summary>
        /// Add or update a value in the blackboard
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetData(string key, object value)
        {
            if (_blackboard.ContainsKey(key))
            {
                _blackboard[key] = value;
            }
            else
            {
                _blackboard.Add(key, value);
            }
        }

        /// <summary>
        /// Get a value from the blackboard
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            object value = null;
            if(_blackboard.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        /// <summary>
        /// Clear a value from the blackboard
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ClearData(string key)
        {
            if(_blackboard.ContainsKey(key))
            {
                _blackboard.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }

        #endregion
    }
}
