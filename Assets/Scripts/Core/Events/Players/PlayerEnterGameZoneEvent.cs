using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Games;
using Assets.Scripts.Core.Players;

namespace Assets.Scripts.Core.Events.Players
{
    public class PlayerEnterGameZoneEvent : IEvent
    {
        public Player Player;
        public Game Game;
    }
}
