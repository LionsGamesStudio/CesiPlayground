using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.UI;
using Nova;
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
    public enum XRInteractorType
    {
        Ray,
        Direct,
        Teleport
    }

    public enum XRInteractorEvent
    {
        Select,
        Deselect,
        HoverEnter,
        HoverExit
    }

    public enum XRInputType
    {
        Grab,
        Trigger,
        PrimaryButton,
        SecondaryButton,
        Thumbstick,
        ThumbstickPress,
        Parameters
    }

    public class XRInteractionManager : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionReference grab;
        [SerializeField] private InputActionReference trigger;
        [SerializeField] private InputActionReference primaryButton;
        [SerializeField] private InputActionReference secondaryButton;
        [SerializeField] private InputActionReference thumbstick;
        [SerializeField] private InputActionReference thumbstickPress;
        [SerializeField] private InputActionReference parameters;

        [Header("Interactors")]
        [SerializeField] private XRBaseControllerInteractor RayInteractor;
        [SerializeField] private XRBaseControllerInteractor DirectInteractor;
        [SerializeField] private XRBaseControllerInteractor TeleportInteractor;

        private List<(int, Action<InputAction.CallbackContext>, XRInputType)> _events = new List<(int, Action<InputAction.CallbackContext>, XRInputType)>();
        private List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)> _interactorEvents = new List<(int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent)>();
        private List<(int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent)> _uiEvents = new List<(int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent)>();
        private List<(int, UnityAction<NovaUIHoverEvent>, XRInteractorType, XRInteractorEvent)> _novaEvents = new List<(int, UnityAction<NovaUIHoverEvent>, XRInteractorType, XRInteractorEvent)>(); // Nova UI Events

        private EventBinding<NovaUIHoverEvent> _onHoverBinding;
        private EventBinding<NovaUISelectEvent> _onSelectBinding;

        private UnityAction<NovaUIHoverEvent> _onHoverEnter;
        private UnityAction<NovaUIHoverEvent> _onHoverExit;

        private CoreBlock _blockHovered;

        private XRRayInteractor _rayInteractor;

        private void Awake()
        {
            _rayInteractor = RayInteractor as XRRayInteractor;

            _onHoverBinding = new EventBinding<NovaUIHoverEvent>(DetectHoverEvent);

            EventBus<NovaUIHoverEvent>.Register(_onHoverBinding);
        }

        private void Update()
        {
            // Detect hover event for Nova UI
            NovaUIHoverEvent hoverEvent = DetectHover();
            if (hoverEvent != null)
            {
                EventBus<NovaUIHoverEvent>.Raise(hoverEvent);
            }
            DetectSelect();
        }

        #region Nova Events

        /// <summary>
        /// Detect the hover event for Nova UI
        /// </summary>
        /// <returns></returns>
        private NovaUIHoverEvent DetectHover()
        {
            // If the ray is over a classic UI element, we don't want to trigger the hover event
            if (_rayInteractor.IsOverUIGameObject()) return null;

            // If the ray is over a 3D element, we trigger the hover event
            if (_rayInteractor.TryGetCurrent3DRaycastHit(out var raycast))
            {
                // If the ray is over a Nova block, we trigger the hover event
                CoreBlock block = raycast.collider.GetComponent<CoreBlock>();
                if (block != null)
                {
                    NovaUIHoverEvent hoverEventEnter = new NovaUIHoverEvent();
                    hoverEventEnter.Element = block;
                    hoverEventEnter.Interactor = RayInteractor;
                    hoverEventEnter.IsHovering = true;

                    // If the block hovered is different from the previous one, we trigger the hover exit event
                    if (_blockHovered != block)
                    {
                        NovaUIHoverEvent hoverEventExit = new NovaUIHoverEvent();
                        hoverEventExit.Element = _blockHovered;
                        hoverEventExit.Interactor = RayInteractor;
                        hoverEventExit.IsHovering = false;

                        _blockHovered = block;

                        EventBus<NovaUIHoverEvent>.Raise(hoverEventExit);
                    }

                    return hoverEventEnter;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Fire the hover event for Nova UI
        /// </summary>
        /// <param name="hoverEvent"></param>
        private void DetectHoverEvent(NovaUIHoverEvent hoverEvent)
        {
            if (hoverEvent.IsHovering)
            {
                _onHoverEnter?.Invoke(hoverEvent);
            }
            else
            {
                _onHoverExit?.Invoke(hoverEvent);
            }
        }

        /// <summary>
        /// Detect the select event for Nova UI
        /// </summary>
        private void DetectSelect()
        {
            InputActionReference trigger = GetRefFromInputType(XRInputType.Trigger);
            if (trigger.action.triggered)
            {
                if (_blockHovered != null)
                {
                    NovaUISelectEvent selectEvent = new NovaUISelectEvent();
                    selectEvent.Element = _blockHovered;
                    selectEvent.Interactor = RayInteractor;

                    EventBus<NovaUISelectEvent>.Raise(selectEvent);
                }
            }
        }

        #endregion


        #region Events

        /// <summary>
        /// Register the event with the given callback and input type
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="inputType"></param>
        /// <returns></returns>
        public int Register(Action<InputAction.CallbackContext> callback, XRInputType inputType)
        {
            int id = Guid.NewGuid().GetHashCode();
            _events.Add((id, callback, inputType));

            InputActionReference reference = GetRefFromInputType(inputType);
            reference.action.started += callback;

            return id;
        }

        /// <summary>
        /// Unregister the event with the given id
        /// </summary>
        /// <param name="id"></param>
        public void Unregister(int id)
        {
            var (index, _, inputType) = _events.Find(e => e.Item1 == id);
            _events.RemoveAt(index);

            InputActionReference reference = GetRefFromInputType(inputType);
            reference.action.started -= _events[index].Item2;
        }

        public (int, Action<InputAction.CallbackContext>, XRInputType) GetEvent(int id)
        {
            return _events.Find(e => e.Item1 == id);
        }

        #endregion

        #region Interactor Events

        /// <summary>
        /// Register the interactor event with the given callback, interactor type and interactor event
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="interactorType"></param>
        /// <param name="interactorEvent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Register(UnityAction<BaseInteractionEventArgs> callback, XRInteractorType interactorType, XRInteractorEvent interactorEvent)
        {
            int id = Guid.NewGuid().GetHashCode();
            _interactorEvents.Add((id, callback, interactorType, interactorEvent));

            XRBaseControllerInteractor interactor = GetInteractor(interactorType);

            switch (interactorEvent)
            {
                case XRInteractorEvent.Select:
                    interactor.selectEntered.AddListener(SelectEnterEventArgs => callback(SelectEnterEventArgs));
                    break;
                case XRInteractorEvent.Deselect:
                    interactor.selectExited.AddListener(SelectExitEventArgs => callback(SelectExitEventArgs));
                    break;
                case XRInteractorEvent.HoverEnter:
                    interactor.hoverEntered.AddListener(HoverEnterEventArgs => callback(HoverEnterEventArgs));
                    break;
                case XRInteractorEvent.HoverExit:
                    interactor.hoverExited.AddListener(HoverExitEventArgs => callback(HoverExitEventArgs));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorEvent), interactorEvent, null);
            }

            return id;
        }

        /// <summary>
        /// Register the interactor ui event with the given callback, interactor type and interactor event
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="interactorEvent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int RegisterUI(UnityAction<UIHoverEventArgs> callback, XRInteractorEvent interactorEvent)
        {
            int id = Guid.NewGuid().GetHashCode();
            _uiEvents.Add((id, callback, XRInteractorType.Ray, interactorEvent));

            XRRayInteractor rayInteractor = RayInteractor as XRRayInteractor;

            switch (interactorEvent)
            {
                case XRInteractorEvent.HoverEnter:
                    rayInteractor.uiHoverEntered.AddListener(callback);
                    break;
                case XRInteractorEvent.HoverExit:
                    rayInteractor.uiHoverExited.AddListener(callback);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorEvent), interactorEvent, null);
            }

            return id;
        }

        /// <summary>
        /// Register the interactor nova event with the given callback, interactor type and interactor event
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="interactorEvent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int RegisterNova(UnityAction<NovaUIHoverEvent> callback, XRInteractorEvent interactorEvent)
        {
            int id = Guid.NewGuid().GetHashCode();
            _novaEvents.Add((id, callback, XRInteractorType.Ray, interactorEvent));

            switch (interactorEvent)
            {
                case XRInteractorEvent.HoverEnter:
                    _onHoverEnter += callback;
                    break;
                case XRInteractorEvent.HoverExit:
                    _onHoverExit += callback;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorEvent), interactorEvent, null);
            }

            return id;
        }

        /// <summary>
        /// Unregister the interactor event with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interactorType"></param>
        /// <param name="interactorEvent"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Unregister(int id, XRInteractorType interactorType, XRInteractorEvent interactorEvent)
        {
            var (index, _, _, _) = _interactorEvents.Find(e => e.Item1 == id);
            _interactorEvents.RemoveAt(index);

            XRBaseControllerInteractor interactor = GetInteractor(interactorType);
            switch (interactorEvent)
            {
                case XRInteractorEvent.Select:
                    interactor.selectEntered.RemoveListener(SelectEnterEventArgs => _interactorEvents[index].Item2(SelectEnterEventArgs));
                    break;
                case XRInteractorEvent.Deselect:
                    interactor.selectExited.RemoveListener(SelectExitEventArgs => _interactorEvents[index].Item2(SelectExitEventArgs));
                    break;
                case XRInteractorEvent.HoverEnter:
                    interactor.hoverEntered.RemoveListener(HoverEnterEventArgs => _interactorEvents[index].Item2(HoverEnterEventArgs));
                    break;
                case XRInteractorEvent.HoverExit:
                    interactor.hoverExited.RemoveListener(HoverExitEventArgs => _interactorEvents[index].Item2(HoverExitEventArgs));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorEvent), interactorEvent, null);
            }
        }

        /// <summary>
        /// Unregister the interactor ui event with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interactorEvent"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UnregisterUI(int id, XRInteractorEvent interactorEvent)
        {
            var (index, _, _, _) = _uiEvents.Find(e => e.Item1 == id);
            _uiEvents.RemoveAt(index);

            XRRayInteractor rayInteractor = RayInteractor as XRRayInteractor;
            switch (interactorEvent)
            {
                case XRInteractorEvent.HoverEnter:
                    rayInteractor.uiHoverEntered.RemoveListener(HoverEnterEventArgs => _uiEvents[index].Item2(HoverEnterEventArgs));
                    break;
                case XRInteractorEvent.HoverExit:
                    rayInteractor.uiHoverExited.RemoveListener(HoverExitEventArgs => _uiEvents[index].Item2(HoverExitEventArgs));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorEvent), interactorEvent, null);
            }
        }

        /// <summary>
        /// Unregister the interactor nova event with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interactorEvent"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UnregisterNova(int id, XRInteractorEvent interactorEvent)
        {
            var (index, _, _, _) = _novaEvents.Find(e => e.Item1 == id);
            _novaEvents.RemoveAt(index);

            switch (interactorEvent)
            {
                case XRInteractorEvent.HoverEnter:
                    _onHoverEnter -= _novaEvents[index].Item2;
                    break;
                case XRInteractorEvent.HoverExit:
                    _onHoverExit -= _novaEvents[index].Item2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorEvent), interactorEvent, null);
            }
        }

        /// <summary>
        /// Get the interactor event with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (int, UnityAction<BaseInteractionEventArgs>, XRInteractorType, XRInteractorEvent) GetInteractorEvent(int id)
        {
            return _interactorEvents.Find(e => e.Item1 == id);
        }

        /// <summary>
        /// Get the interactor ui event with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (int, UnityAction<UIHoverEventArgs>, XRInteractorType, XRInteractorEvent) GetUIEvent(int id)
        {
            return _uiEvents.Find(e => e.Item1 == id);
        }

        /// <summary>
        /// Get the interactor nova event with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (int, UnityAction<NovaUIHoverEvent>, XRInteractorType, XRInteractorEvent) GetNovaEvent(int id)
        {
            return _novaEvents.Find(e => e.Item1 == id);
        }

        #endregion

        /// <summary>
        /// Get the reference from the input type
        /// </summary>
        /// <param name="inputType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private InputActionReference GetRefFromInputType(XRInputType inputType)
        {
            switch (inputType)
            {
                case XRInputType.Grab:
                    return grab;
                case XRInputType.Trigger:
                    return trigger;
                case XRInputType.PrimaryButton:
                    return primaryButton;
                case XRInputType.SecondaryButton:
                    return secondaryButton;
                case XRInputType.Thumbstick:
                    return thumbstick;
                case XRInputType.ThumbstickPress:
                    return thumbstickPress;
                case XRInputType.Parameters:
                    return parameters;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
            }
        }

        /// <summary>
        /// Get the interactor with the given type
        /// </summary>
        /// <param name="interactorType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public XRBaseControllerInteractor GetInteractor(XRInteractorType interactorType)
        {
            switch (interactorType)
            {
                case XRInteractorType.Ray:
                    return RayInteractor;
                case XRInteractorType.Direct:
                    return DirectInteractor;
                case XRInteractorType.Teleport:
                    return TeleportInteractor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactorType), interactorType, null);
            }
        }
    }
}
