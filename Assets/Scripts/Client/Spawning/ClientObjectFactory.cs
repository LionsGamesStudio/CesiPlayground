using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Client.Addressables;
using CesiPlayground.Client.Gameplay.Views;
using CesiPlayground.Client.Network;
using CesiPlayground.Shared.Events.Spawn;

namespace CesiPlayground.Client.Spawning
{
    /// <summary>
    /// A client-side factory responsible for instantiating and destroying
    /// visual representations of networked objects based on server commands.
    /// It uses the AddressablesManager to load prefabs dynamically.
    /// </summary>
    public class ClientObjectFactory : MonoBehaviour
    {
        private readonly Dictionary<int, GameObject> _spawnedObjects = new Dictionary<int, GameObject>();
        private AddressablesManager _addressablesManager;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            // Cache the reference to the AddressablesManager at the start.
            _addressablesManager = ServiceLocator.Get<AddressablesManager>();
            if (_addressablesManager == null)
            {
                Debug.LogError("AddressablesManager not found in ServiceLocator. Ensure it is initialized first.");
            }
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<SpawnNetworkedObjectEvent>(OnSpawnObject);
            GameEventSystem.Client.Register<DestroyNetworkedObjectEvent>(OnDestroyObject);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<SpawnNetworkedObjectEvent>(OnSpawnObject);
            GameEventSystem.Client.Unregister<DestroyNetworkedObjectEvent>(OnDestroyObject);
        }
        
        /// <summary>
        /// Called when a SpawnNetworkedObjectEvent is raised on the client bus.
        /// It now loads the correct prefab and initializes the spawned instance.
        /// </summary>
        private void OnSpawnObject(SpawnNetworkedObjectEvent e)
        {
            if (_spawnedObjects.ContainsKey(e.NetworkId))
            {
                Debug.LogWarning($"[Client] Tried to spawn an object with an already existing NetworkID: {e.NetworkId}");
                return;
            }

            // --- REAL LOADING LOGIC ---
            GameObject prefabToSpawn = LoadPrefabById(e.PrefabId); 

            if (prefabToSpawn == null)
            {
                Debug.LogError($"[Client] Prefab with Addressable ID '{e.PrefabId}' could not be loaded!");
                return;
            }

            // --- INSTANTIATION AND INITIALIZATION ---
            GameObject newInstance = Instantiate(prefabToSpawn, e.Position, e.Rotation);
            
            // Add and initialize the NetworkedObjectView component, which is crucial.
            newInstance.AddComponent<NetworkedObjectView>().Initialize(e.NetworkId);
            
            // After spawning, check for specific view components to initialize them.
            if (newInstance.TryGetComponent<BulletView>(out var bulletView))
            {
                bulletView.Initialize(e.InitialVelocity);
            }
            // We can add more 'if' blocks here for other special components, e.g.:
            // if (newInstance.TryGetComponent<MollView>(out var mollView)) { ... }

            _spawnedObjects.Add(e.NetworkId, newInstance);
            
            Debug.Log($"[Client] Spawned object '{e.PrefabId}' with NetworkID: {e.NetworkId}");
        }

        /// <summary>
        /// Called when a DestroyNetworkedObjectEvent is raised on the client bus.
        /// </summary>
        private void OnDestroyObject(DestroyNetworkedObjectEvent e)
        {
            if (_spawnedObjects.TryGetValue(e.NetworkId, out GameObject objectToDestroy))
            {
                if (objectToDestroy == null)
                {
                    _spawnedObjects.Remove(e.NetworkId);
                    return;
                }

                if (objectToDestroy.TryGetComponent<DeathEffectHandler>(out var deathHandler))
                {
                    deathHandler.PlayEffectsAndDestroy();
                }
                else
                {
                    Destroy(objectToDestroy);
                }

                _spawnedObjects.Remove(e.NetworkId);
            }
        }
        
        /// <summary>
        /// Loads a prefab using the AddressablesManager service.
        /// </summary>
        private GameObject LoadPrefabById(string prefabId)
        {
            if (_addressablesManager == null)
            {
                Debug.LogError("AddressablesManager is not available.");
                return null;
            }
            
            // The PrefabId string is used directly as the Addressable address.
            return _addressablesManager.LoadAsset<GameObject>(prefabId);
        }
    }
}