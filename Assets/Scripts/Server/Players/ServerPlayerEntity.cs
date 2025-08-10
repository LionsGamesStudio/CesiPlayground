using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Server.Network;
using CesiPlayground.Shared.Network;

namespace CesiPlayground.Server.Players
{
    public class ServerPlayerEntity
    {
        public string PlayerId { get; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public bool IsActive { get; set; } = true;
        
        private const float MaxSpeed = 10f;

        public ServerPlayerEntity(string playerId, Vector3 initialPosition)
        {
            PlayerId = playerId;
            Position = initialPosition;
            Rotation = Quaternion.identity;
        }

        /// <summary>
        /// Authoritatively sets the position and FORCES a correction message to the client.
        /// Used for things like anti-cheat or admin commands.
        /// </summary>
        public void SetPositionAndForceCorrection(Vector3 newPosition, Quaternion newRotation)
        {
            Position = newPosition;
            Rotation = newRotation;
            var networkManager = ServiceLocator.Get<ServerNetworkManager>();
            networkManager?.SendToPlayer(this.PlayerId, new PlayerTransformCorrectionMessage
            {
                Position = this.Position,
                Rotation = this.Rotation
            });
        }
        
        /// <summary>
        /// Authoritatively sets the position WITHOUT sending a correction.
        /// Used when the server is acknowledging a valid, large-scale teleport from the client.
        /// </summary>
        public void SetPositionWithoutCorrection(Vector3 newPosition, Quaternion newRotation)
        {
            Position = newPosition;
            Rotation = newRotation;
        }

        public void UpdateTransform(Vector3 position, Quaternion rotation, float deltaTime)
        {
            float distanceMoved = Vector3.Distance(this.Position, position);
            // We give a little buffer to the max speed check
            if (distanceMoved > (MaxSpeed * deltaTime) + 0.1f)
            {
                Debug.LogWarning($"[Server] Player {PlayerId} moved too fast! Forcing correction.");
                SetPositionAndForceCorrection(this.Position, this.Rotation);
                return;
            }
            
            Position = position;
            Rotation = rotation;
        }
    }
}