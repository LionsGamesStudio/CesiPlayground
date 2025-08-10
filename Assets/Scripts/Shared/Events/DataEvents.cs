using CesiPlayground.Shared.Data.Players;
using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Shared.Events.Data
{
    /// <summary>
    /// Fired by the PersistenceService when player data has been successfully
    /// loaded from the remote server.
    /// </summary>
    public struct PlayerDataLoadedEvent : IEvent
    {
        public PlayerData Data;
    }

    /// <summary>
    /// Fired when a player enters a conceptual game zone.
    /// Carries the ID of the player, not a direct component reference.
    /// </summary>
    public struct PlayerEnteredZoneEvent : IEvent
    {
        public string PlayerId;
        public string ZoneId;
    }
}