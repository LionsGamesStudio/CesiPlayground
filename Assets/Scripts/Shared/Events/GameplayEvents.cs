using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Shared.Events.Gameplay
{
    /// <summary>
    /// Fired on the SERVER BUS when an authoritative collision occurs.
    /// Example: A bullet entity hits a target entity.
    /// </summary>
    public struct ServerHitEvent : IEvent
    {
        public int InstigatorNetworkId; // Who caused the hit (e.g., the player or their bullet)
        public int TargetNetworkId;     // What was hit
    }
}