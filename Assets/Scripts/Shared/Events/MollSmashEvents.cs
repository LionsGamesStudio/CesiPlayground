using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Shared.Events.MollSmash
{
    public enum MollState { Hidden, Emerging, Visible, Hit, Hiding }

    /// <summary>
    /// Fired on the CLIENT BUS to update the visual state of a moll.
    /// </summary>
    public struct MollStateChangedEvent : IEvent
    {
        public int NetworkId;
        public MollState NewState;
    }

    /// <summary>
    /// Fired on the CLIENT BUS to update the game's remaining lives.
    /// </summary>
    public struct GameLivesUpdatedEvent : IEvent
    {
        public string GameInstanceId;
        public int RemainingLives;
    }
}