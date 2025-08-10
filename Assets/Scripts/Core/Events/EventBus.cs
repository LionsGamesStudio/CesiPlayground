using System;
using System.Collections.Generic;
using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Core.Events
{
    /// <summary>
    /// Manages event subscriptions and publications for a specific context (e.g., Client or Server).
    /// This class is not static and must be instantiated.
    /// </summary>
    public class EventBus
    {
        private readonly Dictionary<Type, object> _bindings = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a listener for a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event to listen for.</typeparam>
        /// <param name="onEvent">The callback action to execute when the event is raised.</param>
        public void Register<T>(Action<T> onEvent) where T : IEvent
        {
            var eventType = typeof(T);
            if (!_bindings.ContainsKey(eventType))
            {
                _bindings[eventType] = new HashSet<Action<T>>();
            }
            (_bindings[eventType] as HashSet<Action<T>>).Add(onEvent);
        }

        /// <summary>
        /// Unregisters a listener for a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event to stop listening for.</typeparam>
        /// <param name="onEvent">The callback action to remove.</param>
        public void Unregister<T>(Action<T> onEvent) where T : IEvent
        {
            var eventType = typeof(T);
            if (_bindings.TryGetValue(eventType, out var binding))
            {
                (binding as HashSet<Action<T>>)?.Remove(onEvent);
            }
        }

        /// <summary>
        /// Raises an event, invoking all registered listeners for that event type.
        /// </summary>
        /// <typeparam name="T">The type of the event being raised.</typeparam>
        /// <param name="eventData">The event data payload.</param>
        public void Raise<T>(T eventData) where T : IEvent
        {
            var eventType = typeof(T);
            if (_bindings.TryGetValue(eventType, out var binding))
            {
                // Create a copy to prevent issues if a listener unregisters itself during invocation.
                var listeners = new HashSet<Action<T>>(binding as HashSet<Action<T>>);
                foreach (var listener in listeners)
                {
                    listener?.Invoke(eventData);
                }
            }
        }

        /// <summary>
        /// Clears all registered bindings from this event bus instance.
        /// </summary>
        public void Clear()
        {
            _bindings.Clear();
        }
    }
}
