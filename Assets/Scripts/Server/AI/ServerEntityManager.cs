using System.Collections.Generic;
using CesiPlayground.Core;

namespace CesiPlayground.Server.AI
{
    /// <summary>
    /// A central server service that manages the lifecycle and updates of all AI entities.
    /// </summary>
    public class ServerEntityManager
    {
        private readonly Dictionary<int, AIEntity> _activeEntities = new Dictionary<int, AIEntity>();

        public ServerEntityManager()
        {
            ServiceLocator.Register(this);
        }

        public void RegisterEntity(AIEntity entity)
        {
            if (entity != null && !_activeEntities.ContainsKey(entity.NetworkId))
            {
                _activeEntities.Add(entity.NetworkId, entity);
            }
        }

        public void UnregisterEntity(int networkId)
        {
            _activeEntities.Remove(networkId);
        }

        public AIEntity GetEntity(int networkId)
        {
            _activeEntities.TryGetValue(networkId, out var entity);
            return entity;
        }

        /// <summary>
        /// The main update loop for all AI entities, called by the ServerGameLoop.
        /// </summary>
        public void Update(float deltaTime)
        {
            // Iterate over a copy of the keys in case the collection is modified during the loop.
            var keys = new List<int>(_activeEntities.Keys);
            foreach (var key in keys)
            {
                if (_activeEntities.TryGetValue(key, out var entity))
                {
                    entity.Update(deltaTime);
                }
            }
        }

        /// <summary>
        /// Returns a collection of all currently active AI entities.
        /// </summary>
        public IEnumerable<AIEntity> GetAllEntities()
        {
            return _activeEntities.Values;
        }
    }
}