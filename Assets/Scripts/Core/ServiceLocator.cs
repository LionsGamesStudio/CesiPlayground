using System;
using System.Collections.Generic;
using UnityEngine;

namespace CesiPlayground.Core
{
    /// <summary>
    /// A static Service Locator that holds instances of important systems (Managers).
    /// This provides a single point of access for core systems without using global
    /// static singletons, making the architecture cleaner and safer for multiplayer.
    /// </summary>
    public static class ServiceLocator
    {
        // The dictionary that holds all registered service instances.
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service instance, making it available globally.
        /// </summary>
        /// <typeparam name="T">The type of the service being registered.</typeparam>
        /// <param name="service">The instance of the service.</param>
        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogError($"Service of type {type.Name} is already registered.");
                return;
            }
            
            _services[type] = service;
            Debug.Log($"Service registered: {type.Name}");
        }

        /// <summary>
        /// Unregisters a service instance, typically called in OnDestroy.
        /// </summary>
        /// <typeparam name="T">The type of the service to unregister.</typeparam>
        public static void Unregister<T>() where T : class
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                return;
            }

            _services.Remove(type);
            Debug.Log($"Service unregistered: {type.Name}");
        }

        /// <summary>
        /// Gets a registered service instance.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>The registered instance, or null if not found.</returns>
        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (!_services.TryGetValue(type, out var service))
            {
                Debug.LogError($"Service of type {type.Name} not found.");
                return null;
            }
            
            return service as T;
        }

        /// <summary>
        /// Clears all registered services. Useful for editor testing and shutdowns.
        /// </summary>
        public static void Clear()
        {
            _services.Clear();
        }
    }
}