using CesiPlayground.Shared.Events.Interfaces;
using UnityEngine;

namespace CesiPlayground.Shared.Events.Spawn
{
    /// <summary>
    /// Fired on the CLIENT BUS when the server commands an object to be spawned.
    /// This is the primary event for creating visual objects on the client.
    /// </summary>
    public struct SpawnNetworkedObjectEvent : IEvent
    {
        public int NetworkId;       // A unique ID for this instance, assigned by the server.
        public string PrefabId;     // An identifier for the prefab to spawn (e.g., from an Addressables list).
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 InitialVelocity;
    }

    /// <summary>
    /// Fired on the CLIENT BUS when the server commands an object to be destroyed.
    /// </summary>
    public struct DestroyNetworkedObjectEvent : IEvent
    {
        public int NetworkId;       // The unique ID of the instance to destroy.
    }
}