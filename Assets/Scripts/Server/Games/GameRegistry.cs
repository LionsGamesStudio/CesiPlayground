using System.Collections.Generic;
using CesiPlayground.Shared.Games;
using UnityEngine;

namespace CesiPlayground.Server.Games
{
    /// <summary>
    /// A server-side ScriptableObject that acts as a central registry,
    /// mapping a string GameId to its corresponding GameStrategyFactory asset.
    /// This resolves the cross-assembly dependency issue.
    /// </summary>
    [CreateAssetMenu(fileName = "GameRegistry", menuName = "Server/Game Registry")]
    public class GameRegistry : ScriptableObject
    {
        [System.Serializable]
        public struct GameFactoryMapping
        {
            public string GameId;
            public GameStrategyFactory Factory;
        }

        [SerializeField]
        private List<GameFactoryMapping> gameMappings = new List<GameFactoryMapping>();

        private Dictionary<string, GameStrategyFactory> _lookup;

        /// <summary>
        /// Initializes the registry's internal dictionary for fast lookups.
        /// </summary>
        public void Initialize()
        {
            _lookup = new Dictionary<string, GameStrategyFactory>();
            foreach (var mapping in gameMappings)
            {
                if (mapping.Factory != null && !string.IsNullOrEmpty(mapping.GameId))
                {
                    _lookup[mapping.GameId] = mapping.Factory;
                }
            }
        }

        /// <summary>
        /// Gets the factory associated with a given Game ID.
        /// </summary>
        /// <returns>The factory, or null if not found.</returns>
        public GameStrategyFactory GetFactory(string gameId)
        {
            _lookup.TryGetValue(gameId, out var factory);
            return factory;
        }
    }
}