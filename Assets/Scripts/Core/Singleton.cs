using UnityEngine;

namespace CesiPlayground.Core
{
    /// <summary>
    /// A base class for creating components that should only have one instance in the scene.
    /// This version does NOT provide a static .Instance property to avoid global coupling,
    /// making it safer for multiplayer and testable code.
    /// It ensures uniqueness and handles its own destruction if a duplicate is found.
    /// </summary>
    /// <typeparam name="T">The type of the component inheriting from this singleton.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        // This is now a private instance, not static. It's used for the duplicate check.
        private static T _instance;

        public static bool HasInstance => _instance != null;
        public static T TryGetInstance() => HasInstance ? _instance : null;

        /// <summary>
        /// This is the core of the singleton logic.
        /// It's executed when the component awakens.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        /// <summary>
        /// Ensures that only one instance of this singleton exists in the scene.
        /// </summary>
        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            // If an instance already exists and it's not this one, destroy this one.
            if (_instance != null && _instance != this)
            {
                Debug.LogWarning($"Duplicate instance of singleton {typeof(T).Name} found on {gameObject.name}. Destroying component.");
                Destroy(this);
                return;
            }

            // This is the first or only instance.
            _instance = this as T;
        }

        /// <summary>
        /// When this component is destroyed, if it was the singleton instance,
        /// clear the static reference to allow for a new instance later.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}