using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts.Core.XR
{
    public enum XRInteractorType
    {
        Ray,
        Direct,
        Teleport
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
        public InputActionReference Grab;
        public InputActionReference Trigger;
        public InputActionReference PrimaryButton;
        public InputActionReference SecondaryButton;
        public InputActionReference Thumbstick;
        public InputActionReference ThumbstickPress;
        public InputActionReference Parameters;

        [Header("Interactors")]
        public XRBaseControllerInteractor RayInteractor;
        public XRBaseControllerInteractor DirectInteractor;
        public XRBaseControllerInteractor TeleportInteractor;

        private List<(int, Action<InputAction.CallbackContext>, XRInputType)> _events = new List<(int, Action<InputAction.CallbackContext>, XRInputType)>();


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
                    return Grab;
                case XRInputType.Trigger:
                    return Trigger;
                case XRInputType.PrimaryButton:
                    return PrimaryButton;
                case XRInputType.SecondaryButton:
                    return SecondaryButton;
                case XRInputType.Thumbstick:
                    return Thumbstick;
                case XRInputType.ThumbstickPress:
                    return ThumbstickPress;
                case XRInputType.Parameters:
                    return Parameters;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
            }
        }
    }
}
