using UnityEngine;
using CesiPlayground.Server.AI.BehaviourTree;

namespace CesiPlayground.Server.AI
{
    /// <summary>
    /// Represents a generic, server-authoritative AI entity.
    /// It owns a Behaviour Tree and manages its core state like position and rotation.
    /// This is the base class for all specific AI types.
    /// </summary>
    public class AIEntity
    {
        public int NetworkId { get; }
        public Vector3 Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        public bool IsActive { get; set; } = true;

        protected readonly BehaviourTree.Tree _behaviourTree;
        
        public AIEntity(int networkId, Vector3 initialPosition, BehaviourTree.Tree behaviourTree)
        {
            NetworkId = networkId;
            Position = initialPosition;
            Rotation = Quaternion.identity;
            
            _behaviourTree = behaviourTree;
            _behaviourTree.Initialize();
        }

        /// <summary>
        /// Called on every server update tick. This method is virtual
        /// so that specific AI types can add their own logic.
        /// </summary>
        public virtual void Update(float deltaTime)
        {
            if (!IsActive) return;

            // 1. Write the entity's current state to the blackboard for the tree to read.
            _behaviourTree.Blackboard.Set("Position", Position);
            _behaviourTree.Blackboard.Set("Rotation", Rotation);

            // 2. Run the tree's logic.
            _behaviourTree.Update(deltaTime);

            // 3. Read the resulting state from the blackboard and update the entity.
            Position = _behaviourTree.Blackboard.Get<Vector3>("Position");
            Rotation = _behaviourTree.Blackboard.Get<Quaternion>("Rotation");
        }
    }
}