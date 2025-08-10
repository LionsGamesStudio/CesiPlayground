using UnityEngine;
using System.Collections.Generic;
using CesiPlayground.Server.AI.BehaviourTree;
using CesiPlayground.Server.AI.BehaviourTree.Nodes;
using CesiPlayground.Server.AI.BehaviourTree.Nodes.SpecificNodes;

namespace CesiPlayground.Server.AI.MollSmash
{
    public class BaseMollBT : BehaviourTree.Tree
    {
        private readonly Vector3 _positionUp;
        private readonly Vector3 _positionDown;
        private readonly float _speed;
        private readonly float _waitTime;

        /// <summary>
        /// Initializes a new instance of the BaseMollBT class.
        /// </summary>
        public BaseMollBT(Vector3 positionDown, Vector3 positionUp, float speed, float waitTime)
        {
            _positionDown = positionDown;
            _positionUp = positionUp;
            _speed = speed;
            _waitTime = waitTime;
        }

        protected override Node SetupTree()
        {
            // The tree describes the behavior: go up, wait, go down.
            // It uses the data it was given during construction.
            Node root = new Sequence(new List<Node>
            {
                new TaskPatrol(new[] { _positionUp }, _speed),
                new TaskWait(_waitTime),
                new TaskPatrol(new[] { _positionDown }, _speed)
            });
            
            return root;
        }
    }
}