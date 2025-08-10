using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Shared.Events.World
{
    /// <summary>
    /// Fired ONLY on the CLIENT BUS when the local player's trigger
    /// interacts with a gate. It signals a local intent.
    /// </summary>
    public struct ClientPlayerUsedGateEvent : IEvent
    {
        public string GateName;
    }

    /// <summary>
    /// Fired ONLY on the SERVER BUS after the network layer has received
    /// a message and identified the player. It signals an authenticated action.
    /// </summary>
    public struct ServerPlayerUsedGateEvent : IEvent
    {
        public string PlayerId;
        public string GateName;
    }
}