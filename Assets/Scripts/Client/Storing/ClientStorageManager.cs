using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Storing;

namespace CesiPlayground.Client.Storing
{
    /// <summary>
    /// Manages LOCAL and NON-AUTHORITATIVE data persistence for the client.
    /// This system inherits from Singleton<T> to ensure it's unique, and registers
    /// itself with the ServiceLocator to be globally accessible in a safe manner.
    /// Use this for graphics settings, sound volume, input bindings, etc.
    /// DO NOT use this for player progression or sensitive game data.
    /// </summary>
    public class ClientStorageManager : Singleton<ClientStorageManager>
    {
        [Tooltip("The factory to create the local storage implementation (e.g., JsonStorageFactory)")]
        [SerializeField] private IStorageFactory storageFactory;
        
        private IStorage _storage;

        /// <summary>
        /// Awake is now responsible for initializing the singleton AND registering it as a service.
        /// </summary>
        protected override void Awake()
        {
            // First, call the base method from Singleton<T>.
            // This will check if another instance already exists and destroy this one if so.
            base.Awake();

            // The 'enabled' property is automatically set to false by Unity on the same frame
            // if Destroy() is called. This prevents the rest of the method from executing
            // on a component that is scheduled for destruction.
            if (this.enabled == false) return; 

            ServiceLocator.Register<ClientStorageManager>(this);
            
            DontDestroyOnLoad(gameObject);

            // Initialize the storage implementation from the factory.
            if (storageFactory != null)
            {
                _storage = storageFactory.CreateStorage();
            }
            else
            {
                Debug.LogError($"ClientStorageManager on GameObject '{gameObject.name}' requires a StorageFactory to be assigned in the inspector.");
            }
        }
        
        /// <summary>
        /// When this singleton is destroyed (e.g., on application quit),
        /// it unregisters itself from the Service Locator to prevent memory leaks
        /// and invalid references.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            ServiceLocator.Unregister<ClientStorageManager>();
        }

        #region Public Storage Functions

        /// <summary>
        /// Gets a content from a local file using the assigned storage implementation.
        /// </summary>
        /// <typeparam name="T">Type of the content to get.</typeparam>
        /// <param name="filepath">Filepath where content is stored.</param>
        /// <returns>The deserialized content, or null if not found or if storage is not initialized.</returns>
        public T Get<T>(string filepath) where T : class
        {
            if (_storage == null)
            {
                Debug.LogError("Storage is not initialized. Cannot Get data.");
                return null;
            }
            return _storage.Get<T>(filepath);
        }

        /// <summary>
        /// Stores a content in a local file using the assigned storage implementation.
        /// </summary>
        /// <typeparam name="T">Type of the content to store.</typeparam>
        /// <param name="filepath">Filepath where to store the content.</param>
        /// <param name="value">The content to store.</param>
        public void Store<T>(string filepath, T value) where T : class
        {
            if (_storage == null)
            {
                Debug.LogError("Storage is not initialized. Cannot Store data.");
                return;
            }
            _storage.Store<T>(filepath, value);
        }
        
        #endregion
    }
}