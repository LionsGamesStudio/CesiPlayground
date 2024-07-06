using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.XR
{
    public enum XRHand
    {
        Primary,
        Secondary
    }

    public class XRGlobalManager : MonoBehaviour
    {
        public XRInteractionManager PrimaryHand;
        public XRInteractionManager SecondaryHand;

        private EventBinding<RegisterXRInputEvent> _registerXRInputEvent;
        private EventBinding<UnregisterXRInputEvent> _unregisterXRInputEvent;


        private List<int> _eventsIds = new List<int>();

        public void Awake()
        {
            _registerXRInputEvent = new EventBinding<RegisterXRInputEvent>(RegisterXRInputEvent);
            _unregisterXRInputEvent = new EventBinding<UnregisterXRInputEvent>(UnregisterXRInputEvent);
        }

        #region Events

        /// <summary>
        /// Register the event with the given callback and input type
        /// </summary>
        /// <param name="e"></param>
        public void RegisterXRInputEvent(RegisterXRInputEvent e)
        {
            int id;
            if (e.Hand == XRHand.Primary)
            {
                id = PrimaryHand.Register(e.InputAction, e.InputType);
            }
            else
            {
                id = SecondaryHand.Register(e.InputAction, e.InputType);
            }
            _eventsIds.Add(id);
        }

        /// <summary>
        /// Unregister the event with the given id
        /// </summary>
        /// <param name="e"></param>
        public void UnregisterXRInputEvent(UnregisterXRInputEvent e)
        {
            if (e.Hand == XRHand.Primary)
            {
                PrimaryHand.Unregister(e.Id);
            }
            else
            {
                SecondaryHand.Unregister(e.Id);
            }
            _eventsIds.Remove(e.Id);
        }

        /// <summary>
        /// Get the event with the given id and hand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        public (int, Action<InputAction.CallbackContext>, XRInputType) GetEvent(int id, XRHand hand)
        {
            if (hand == XRHand.Primary)
            {
                return PrimaryHand.GetEvent(id);
            }
            else
            {
                return SecondaryHand.GetEvent(id);
            }
        }

        /// <summary>
        /// Get all events for the given hand
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public List<(int, Action<InputAction.CallbackContext>, XRInputType)> GetEvents(XRHand hand)
        {
            List<(int, Action<InputAction.CallbackContext>, XRInputType)> events = new List<(int, Action<InputAction.CallbackContext>, XRInputType)>();
            if (hand == XRHand.Primary)
            {
                foreach (var id in _eventsIds)
                {
                    events.Add(GetEvent(id, XRHand.Primary));
                }
            }
            else
            {
                foreach (var id in _eventsIds)
                {
                    events.Add(GetEvent(id, XRHand.Secondary));
                }
            }
            return events;
        }

        /// <summary>
        /// Reset the hand
        /// </summary>
        /// <param name="hand"></param>
        public void ResetHand(XRHand hand)
        {
            List<(int, Action<InputAction.CallbackContext>, XRInputType)> events = GetEvents(hand);
            foreach (var (id, callback, inputType) in events)
            {
                if (hand == XRHand.Primary)
                {
                    PrimaryHand.Unregister(id);
                }
                else
                {
                    SecondaryHand.Unregister(id);
                }
            }
        }

        #endregion

        /// <summary>
        /// Unregister all events
        /// </summary>
        public void OnDestroy()
        {
            foreach (var id in _eventsIds)
            {
                UnregisterXRInputEvent unregisterXRInputEvent = new UnregisterXRInputEvent
                {
                    Id = id
                };
                EventBus<UnregisterXRInputEvent>.Unregister(_unregisterXRInputEvent);
            }
        }

        /// <summary>
        /// Switch the primary hand
        /// </summary>
        public void SwitchPrimaryHand()
        {
            List<(int, Action<InputAction.CallbackContext>, XRInputType)> primaryEvents = GetEvents(XRHand.Primary);
            List<(int, Action<InputAction.CallbackContext>, XRInputType)> secondaryEvents = GetEvents(XRHand.Secondary);

            ResetHand(XRHand.Primary);
            ResetHand(XRHand.Secondary);

            foreach (var (id, callback, inputType) in primaryEvents)
            {
                RegisterXRInputEvent registerXRInputEvent = new RegisterXRInputEvent
                {
                    Hand = XRHand.Secondary,
                    InputAction = callback,
                    InputType = inputType
                };
                EventBus<RegisterXRInputEvent>.Raise(registerXRInputEvent);
            }

            foreach (var (id, callback, inputType) in secondaryEvents)
            {
                RegisterXRInputEvent registerXRInputEvent = new RegisterXRInputEvent
                {
                    Hand = XRHand.Primary,
                    InputAction = callback,
                    InputType = inputType
                };
                EventBus<RegisterXRInputEvent>.Raise(registerXRInputEvent);
            }
        }

    }
}
