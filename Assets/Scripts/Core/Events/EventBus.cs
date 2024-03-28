using Assets.Scripts.Core.Events.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Events
{
    public static class EventBus<T> where T : IEvent
    {
        static readonly HashSet<IEventBinding<T>> _bindings = new HashSet<IEventBinding<T>>();

        #region Events

        /// <summary>
        /// Register something to listen to event of T type
        /// </summary>
        /// <param name="binding"></param>
        public static void Register(IEventBinding<T> binding) => _bindings.Add(binding);

        /// <summary>
        /// Unregister the listening of a T event
        /// </summary>
        /// <param name="binding"></param>
        public static void Unregister(IEventBinding<T> binding) => _bindings.Remove(binding);

        /// <summary>
        /// Raise an event
        /// </summary>
        /// <param name="evt"></param>
        public static void Raise(T @evt)
        {
            foreach (var binding in _bindings)
            {
                binding.OnEvent.Invoke(@evt);
                binding.OnEventNoArgs.Invoke();
            }
        }

        #endregion

        static void Clear() => _bindings.Clear();
    }
}
