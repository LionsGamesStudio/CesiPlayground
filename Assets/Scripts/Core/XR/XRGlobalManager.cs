using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.UI;
using Assets.Scripts.Core.Events.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

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

        private EventBinding<RegisterXRInteractorEvent> _registerXRInteractorEvent;
        private EventBinding<UnregisterXRInteractorEvent> _unregisterXRInteractorEvent;

        private EventBinding<RegisterXRInteractorUIEvent> _registerXRInteractorUIEvent;
        private EventBinding<UnregisterXRInteractorUIEvent> _unregisterXRInteractorUIEvent;


        private List<int> _eventsIds = new List<int>();
        private List<int> _interactorsEventsIds = new List<int>();
        private List<int> _interactorsUIEventsIds = new List<int>();

        public void Awake()
        {
            _registerXRInputEvent = new EventBinding<RegisterXRInputEvent>(RegisterXRInputEvent);
            _unregisterXRInputEvent = new EventBinding<UnregisterXRInputEvent>(UnregisterXRInputEvent);

            _registerXRInteractorEvent = new EventBinding<RegisterXRInteractorEvent>(RegisterXRInteractorEvent);
            _unregisterXRInteractorEvent = new EventBinding<UnregisterXRInteractorEvent>(UnregisterXRInteractorEvent);

            _registerXRInteractorUIEvent = new EventBinding<RegisterXRInteractorUIEvent>(RegisterXRInteractorUIEvent);
            _unregisterXRInteractorUIEvent = new EventBinding<UnregisterXRInteractorUIEvent>(UnregisterXRInteractorUIEvent);


            EventBus<RegisterXRInputEvent>.Register(_registerXRInputEvent);
            EventBus<UnregisterXRInputEvent>.Register(_unregisterXRInputEvent);
            EventBus<RegisterXRInteractorEvent>.Register(_registerXRInteractorEvent);
            EventBus<UnregisterXRInteractorEvent>.Register(_unregisterXRInteractorEvent);
            EventBus<RegisterXRInteractorUIEvent>.Register(_registerXRInteractorUIEvent);
            EventBus<UnregisterXRInteractorUIEvent>.Register(_unregisterXRInteractorUIEvent);

        }

        public void Start()
        {
            EventBus<InitXRHandEvent>.Raise(new InitXRHandEvent());
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

        #region Interactors

        /// <summary>
        /// Register the interactor event with the given callback, interactor type and interactor event
        /// </summary>
        /// <param name="e"></param>
        private void RegisterXRInteractorEvent(RegisterXRInteractorEvent e)
        {
            int id;
            if (e.Hand == XRHand.Primary)
            {
                id = PrimaryHand.Register(e.Callback, e.InteractorType, e.InteractorEvent);
            }
            else
            {
                id = SecondaryHand.Register(e.Callback, e.InteractorType, e.InteractorEvent);
            }
            _interactorsEventsIds.Add(id);
        }

        /// <summary>
        /// Register the interactor ui event with the given callback and interactor event
        /// </summary>
        /// <param name="e"></param>
        private void RegisterXRInteractorUIEvent(RegisterXRInteractorUIEvent e)
        {
            int id;
            if (e.Hand == XRHand.Primary)
            {
                id = PrimaryHand.RegisterUI(e.Callback, e.XRInteractorEvent);
            }
            else
            {
                id = SecondaryHand.RegisterUI(e.Callback, e.XRInteractorEvent);
            }
            _interactorsUIEventsIds.Add(id);
        }

        /// <summary>
        /// Unregister the interactor event with the given id
        /// </summary>
        /// <param name="e"></param>
        private void UnregisterXRInteractorEvent(UnregisterXRInteractorEvent e)
        {
            if (e.Hand == XRHand.Primary)
            {
                PrimaryHand.Unregister(e.Id, e.InteractorType, e.InteractorEvent);
            }
            else
            {
                SecondaryHand.Unregister(e.Id, e.InteractorType, e.InteractorEvent);
            }
            _interactorsEventsIds.Remove(e.Id);
        }

        /// <summary>
        /// Unregister the interactor ui event with the given id
        /// </summary>
        /// <param name="e"></param>
        private void UnregisterXRInteractorUIEvent(UnregisterXRInteractorUIEvent e)
        {
            if (e.Hand == XRHand.Primary)
            {
                PrimaryHand.UnregisterUI(e.Id, e.XRInteractorEvent);
            }
            else
            {
                SecondaryHand.UnregisterUI(e.Id, e.XRInteractorEvent);
            }
            _interactorsUIEventsIds.Remove(e.Id);
        }

        /// <summary>
        /// Get the interactor event with the given id and hand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        private (int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent) GetInteractorEvent(int id, XRHand hand)
        {
            if (hand == XRHand.Primary)
            {
                return PrimaryHand.GetInteractorEvent(id);
            }
            else
            {
                return SecondaryHand.GetInteractorEvent(id);
            }
        }

        /// <summary>
        /// Get the interactor ui event with the given id and hand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        private (int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent) GetInteractorUIEvent(int id, XRHand hand)
        {
            if (hand == XRHand.Primary)
            {
                return PrimaryHand.GetUIEvent(id);
            }
            else
            {
                return SecondaryHand.GetUIEvent(id);
            }
        }

        /// <summary>
        /// Get all interactor events for the given hand
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        private List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)> GetInteractorEvents(XRHand hand)
        {
            List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)> events = new List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)>();
            if (hand == XRHand.Primary)
            {
                foreach (var id in _interactorsEventsIds)
                {
                    events.Add(GetInteractorEvent(id, XRHand.Primary));
                }
            }
            else
            {
                foreach (var id in _interactorsEventsIds)
                {
                    events.Add(GetInteractorEvent(id, XRHand.Secondary));
                }
            }
            return events;
        }

        /// <summary>
        /// Get all interactor ui events for the given hand
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        private List<(int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent)> GetInteractorUIEvents(XRHand hand)
        {
            List<(int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent)> events = new List<(int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent)>();
            if (hand == XRHand.Primary)
            {
                foreach (var id in _interactorsUIEventsIds)
                {
                    events.Add(GetInteractorUIEvent(id, XRHand.Primary));
                }
            }
            else
            {
                foreach (var id in _interactorsUIEventsIds)
                {
                    events.Add(GetInteractorUIEvent(id, XRHand.Secondary));
                }
            }
            return events;
        }

        /// <summary>
        /// Reset the interactor
        /// </summary>
        /// <param name="hand"></param>
        private void ResetInteractor(XRHand hand)
        {
            List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)> events = GetInteractorEvents(hand);
            foreach (var (id, callback, interactorType, interactorEvent) in events)
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

            List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)> primaryInteractorEvents = GetInteractorEvents(XRHand.Primary);
            List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)> secondaryInteractorEvents = GetInteractorEvents(XRHand.Secondary);

            ResetHand(XRHand.Primary);
            ResetHand(XRHand.Secondary);
            ResetInteractor(XRHand.Primary);
            ResetInteractor(XRHand.Secondary);

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

            foreach (var (id, callback, interactorType, interactorEvent) in primaryInteractorEvents)
            {
                RegisterXRInteractorEvent registerXRInteractorEvent = new RegisterXRInteractorEvent
                {
                    Hand = XRHand.Secondary,
                    Callback = callback,
                    InteractorType = interactorType,
                    InteractorEvent = interactorEvent
                };
                EventBus<RegisterXRInteractorEvent>.Raise(registerXRInteractorEvent);
            }

            foreach (var (id, callback, interactorType, interactorEvent) in secondaryInteractorEvents)
            {
                RegisterXRInteractorEvent registerXRInteractorEvent = new RegisterXRInteractorEvent
                {
                    Hand = XRHand.Primary,
                    Callback = callback,
                    InteractorType = interactorType,
                    InteractorEvent = interactorEvent
                };
                EventBus<RegisterXRInteractorEvent>.Raise(registerXRInteractorEvent);
            }
        }
    }
}
