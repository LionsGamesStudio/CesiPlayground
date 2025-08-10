using UnityEngine;
using UnityEngine.SceneManagement;

namespace CesiPlayground.Core.Events
{
    /// <summary>
    /// Static hub that holds and provides access to the distinct Client and Server EventBus instances.
    /// It also manages their lifecycle.
    /// </summary>
    public static class GameEventSystem
    {
        /// <summary>
        /// The event bus for client-side logic (UI, Input, Audio, Visuals).
        /// </summary>
        public static readonly EventBus Client = new EventBus();

        /// <summary>
        /// The event bus for server-side logic (Gameplay Rules, Physics, State Simulation).
        /// </summary>
        public static readonly EventBus Server = new EventBus();

        /// <summary>
        /// This method is called by Unity's runtime before the first scene loads.
        /// It sets up automatic cleanup of the event buses.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        /// <summary>
        /// Called automatically when a scene is unloaded to clear all listeners
        /// and prevent memory leaks or "ghost" listeners.
        /// </summary>
        private static void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
        {
            Client.Clear();
            Server.Clear();
        }
    }
}