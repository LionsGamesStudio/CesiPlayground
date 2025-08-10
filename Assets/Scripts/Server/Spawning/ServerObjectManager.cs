using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Server.Network;
using CesiPlayground.Shared.Network;

namespace CesiPlayground.Server.Spawning
{
    /// <summary>
    /// Authoritative manager on the server that controls the lifecycle of networked entities.
    /// It decides WHEN and WHERE objects should exist, but does not deal with GameObjects.
    /// </summary>
    public class ServerObjectManager
    {
        private int _nextNetworkId = 1;

        /// <summary>
        /// Registers itself with the Service Locator.
        /// </summary>
        public ServerObjectManager()
        {
            ServiceLocator.Register(this);
        }

        /// <summary>
        /// The authoritative method to create a new networked entity.
        /// </summary>
        /// <param name="prefabId">The identifier of the object to spawn.</param>
        /// <param name="position">The world position.</param>
        /// <param name="rotation">The world rotation.</param>
        public int SpawnObject(string prefabId, Vector3 position, Quaternion rotation, Vector3 initialVelocity = default(Vector3))
        {
            int networkId = _nextNetworkId++;
            
            UnityEngine.Debug.Log($"[Server] Spawning object. PrefabID: {prefabId}, NetworkID: {networkId}");

            // The network layer broadcasts the creation message to all clients.
            var networkManager = ServiceLocator.Get<ServerNetworkManager>();
            networkManager?.Broadcast(new SpawnObjectMessage
            {
                networkId = networkId,
                prefabId = prefabId,
                position = position,
                rotation = rotation,
                initialVelocity = initialVelocity
            });

            return networkId;
        }

        /// <summary>
        /// The authoritative method to destroy a networked entity.
        /// </summary>
        /// <param name="networkId">The unique network ID of the entity to destroy.</param>
        public void DestroyObject(int networkId)
        {
            UnityEngine.Debug.Log($"[Server] Destroying object with NetworkID: {networkId}");
            
            var networkManager = ServiceLocator.Get<ServerNetworkManager>();
            networkManager?.Broadcast(new DestroyObjectMessage { networkId = networkId });
        }
    }
}