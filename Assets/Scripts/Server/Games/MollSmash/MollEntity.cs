using UnityEngine;
using CesiPlayground.Server.AI;
using CesiPlayground.Server.AI.MollSmash;

namespace CesiPlayground.Server.Games.MollSmash
{
    public class MollEntity : AIEntity
    {
        private float _lifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MollEntity"/> class.
        /// <param name="networkId">The unique identifier for the network.</param>
        /// <param name="spawnPosition">The initial position where the moll will spawn.</param
        /// param name="upPosition">The position above the spawn point, used for orientation.</param>
        /// <param name="lifetime">The duration in seconds for which the moll will remain active.</param>
        /// <param name="speed">The speed at which the moll moves.</param>
        /// <param name="waitTime">The time in seconds the moll waits at each waypoint.</param>
        /// </summary>
        public MollEntity(int networkId, Vector3 spawnPosition, Vector3 upPosition, float lifetime, float speed, float waitTime)
            : base(networkId, spawnPosition, new BaseMollBT(spawnPosition, upPosition, speed, waitTime))
        {
            _lifetime = lifetime;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive) return;

            _lifetime -= deltaTime;
            if (_lifetime <= 0)
            {
                IsActive = false;
                return;
            }

            base.Update(deltaTime);
        }
    }
}