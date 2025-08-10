using UnityEngine;
using UnityEngine.InputSystem;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;

namespace CesiPlayground.Client.Input
{
    /// <summary>
    /// Reads raw hardware inputs from InputActionReferences and translates them
    /// into abstract PlayerActionInputEvents on the CLIENT event bus.
    /// This is the only place with direct dependency on specific hardware buttons.
    /// </summary>
    public class InputReader : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private XRHand hand;

        [Header("Input Action References")]
        [SerializeField] private InputActionReference primaryAction;
        [SerializeField] private InputActionReference secondaryAction;
        [SerializeField] private InputActionReference grabAction;
        [SerializeField] private InputActionReference triggerAction;
        [SerializeField] private InputActionReference thumbstickPressAction;
        [SerializeField] private InputActionReference teleportAction;
        [SerializeField] private InputActionReference parametersAction;
        [SerializeField] private InputActionReference thumbstickMoveAction;

        /// <summary>
        /// The hand this InputReader is associated with.
        /// This is used to filter events for the correct hand.
        /// </summary>
        public XRHand Hand => hand;

        private void OnEnable()
        {
            primaryAction.action.performed += ctx => OnAction(PlayerActionType.Primary, InputActionState.Performed);
            primaryAction.action.canceled += ctx => OnAction(PlayerActionType.Primary, InputActionState.Canceled);

            secondaryAction.action.performed += ctx => OnAction(PlayerActionType.Secondary, InputActionState.Performed);
            secondaryAction.action.canceled += ctx => OnAction(PlayerActionType.Secondary, InputActionState.Canceled);

            grabAction.action.performed += ctx => OnAction(PlayerActionType.Grab, InputActionState.Performed);
            grabAction.action.canceled += ctx => OnAction(PlayerActionType.Grab, InputActionState.Canceled);

            triggerAction.action.performed += ctx => OnAction(PlayerActionType.Trigger, InputActionState.Performed);
            triggerAction.action.canceled += ctx => OnAction(PlayerActionType.Trigger, InputActionState.Canceled);

            thumbstickPressAction.action.performed += ctx => OnAction(PlayerActionType.ThumbstickPress, InputActionState.Performed);
            thumbstickPressAction.action.canceled += ctx => OnAction(PlayerActionType.ThumbstickPress, InputActionState.Canceled);

            teleportAction.action.performed += ctx => OnAction(PlayerActionType.Teleport, InputActionState.Performed);
            teleportAction.action.canceled += ctx => OnAction(PlayerActionType.Teleport, InputActionState.Canceled);

            parametersAction.action.performed += ctx => OnAction(PlayerActionType.Parameters, InputActionState.Performed);
            parametersAction.action.canceled += ctx => OnAction(PlayerActionType.Parameters, InputActionState.Canceled);

            thumbstickMoveAction.action.performed += OnThumbstickMove;
        }

        private void OnDisable()
        {
            // Unsubscribe from all actions to prevent errors.
            primaryAction.action.performed -= ctx => OnAction(PlayerActionType.Primary, InputActionState.Performed);
            primaryAction.action.canceled -= ctx => OnAction(PlayerActionType.Primary, InputActionState.Canceled);

            secondaryAction.action.performed -= ctx => OnAction(PlayerActionType.Secondary, InputActionState.Performed);
            secondaryAction.action.canceled -= ctx => OnAction(PlayerActionType.Secondary, InputActionState.Canceled);

            grabAction.action.performed -= ctx => OnAction(PlayerActionType.Grab, InputActionState.Performed);
            grabAction.action.canceled -= ctx => OnAction(PlayerActionType.Grab, InputActionState.Canceled);

            triggerAction.action.performed -= ctx => OnAction(PlayerActionType.Trigger, InputActionState.Performed);
            triggerAction.action.canceled -= ctx => OnAction(PlayerActionType.Trigger, InputActionState.Canceled);

            thumbstickPressAction.action.performed -= ctx => OnAction(PlayerActionType.ThumbstickPress, InputActionState.Performed);
            thumbstickPressAction.action.canceled -= ctx => OnAction(PlayerActionType.ThumbstickPress, InputActionState.Canceled);

            teleportAction.action.performed -= ctx => OnAction(PlayerActionType.Teleport, InputActionState.Performed);
            teleportAction.action.canceled -= ctx => OnAction(PlayerActionType.Teleport, InputActionState.Canceled);

            parametersAction.action.performed -= ctx => OnAction(PlayerActionType.Parameters, InputActionState.Performed);
            parametersAction.action.canceled -= ctx => OnAction(PlayerActionType.Parameters, InputActionState.Canceled);

            thumbstickMoveAction.action.performed -= OnThumbstickMove;
        }

        /// <summary>
        /// Generic handler for discrete button-like actions.
        /// </summary>
        private void OnAction(PlayerActionType type, InputActionState state)
        {
            GameEventSystem.Client.Raise(new ClientPlayerActionEvent { Hand = this.hand, ActionType = type, State = state });
        }

        /// <summary>
        /// Handler for analog thumbstick movement.
        /// </summary>
        private void OnThumbstickMove(InputAction.CallbackContext context)
        {
            GameEventSystem.Client.Raise(new PlayerMovementInputEvent { Hand = this.hand, MoveValue = context.ReadValue<Vector2>() });
        }
    }
}