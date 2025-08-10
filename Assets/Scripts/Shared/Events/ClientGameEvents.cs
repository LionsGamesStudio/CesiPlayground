using UnityEngine;
using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Shared.Events.Client
{
    /// <summary>
    /// Fired on the CLIENT BUS to command the creation of a game instance.
    /// </summary>
    public struct InstantiateGameEvent : IEvent
    {
        public string InstanceId;
        public string GameId;
        public Vector3 Position;
    }

    /// <summary>
    /// Fired on the CLIENT BUS to command the destruction of a game instance and return to the hub.
    /// </summary>
    public struct ReturnToHubEvent : IEvent
    {
        public string InstanceId;
        public Vector3 HubReturnPosition;
    }

    /// <summary>
    /// Fired on the CLIENT BUS when a player interacts with a matchmaking terminal.
    /// </summary>
    public struct PlayerQueueForGameEvent : IEvent
    {
        public string GameId;
    }
}