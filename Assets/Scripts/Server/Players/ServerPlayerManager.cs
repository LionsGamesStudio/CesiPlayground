using System.Collections.Generic;
using CesiPlayground.Core;

namespace CesiPlayground.Server.Players
{
    /// <summary>
    /// A central server service that manages the lifecycle and access
    /// to all connected ServerPlayerEntity instances.
    /// </summary>
    public class ServerPlayerManager
    {
        private readonly Dictionary<string, ServerPlayerEntity> _players = new Dictionary<string, ServerPlayerEntity>();

        public ServerPlayerManager()
        {
            ServiceLocator.Register(this);
        }

        public void AddPlayer(ServerPlayerEntity player)
        {
            if (player != null && !_players.ContainsKey(player.PlayerId))
            {
                _players.Add(player.PlayerId, player);
                UnityEngine.Debug.Log($"[ServerPlayerManager] Player registered: {player.PlayerId}");
            }
        }

        public void RemovePlayer(string playerId)
        {
            _players.Remove(playerId);
            UnityEngine.Debug.Log($"[ServerPlayerManager] Player unregistered: {playerId}");
        }

        public ServerPlayerEntity GetPlayer(string playerId)
        {
            _players.TryGetValue(playerId, out var player);
            return player;
        }
    }
}