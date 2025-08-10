using CesiPlayground.Shared.Events.Interfaces;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace CesiPlayground.Shared.Events.Input
{
    public enum XRHand { Left, Right }
    public enum PlayerActionType { Primary, Grab, Trigger, Secondary, Teleport, Parameters, ThumbstickPress, MeleeHit }
    public enum InputActionState { Performed, Canceled }
    public enum UIInteractionState { HoverEnter, HoverExit, Select }

    /// <summary>
    /// Fired ONLY on the CLIENT BUS when a local input action occurs.
    /// This is the event that client-side systems should listen to.
    /// </summary>
    public struct ClientPlayerActionEvent : IEvent
    {
        public XRHand Hand;
        public PlayerActionType ActionType;
        public InputActionState State;
    }

    /// <summary>
    /// Fired ONLY on the SERVER BUS after a network message for a player action is received.
    /// </summary>
    public struct ServerPlayerActionEvent : IEvent
    {
        public string PlayerId;
        public PlayerActionType ActionType;
        public InputActionState State;
    }

    /// <summary>
    /// Event fired when the player moves an analog input like a thumbstick.
    /// </summary>
    public struct PlayerMovementInputEvent : IEvent
    {
        public XRHand Hand;
        public Vector2 MoveValue;
    }

    /// <summary>
    /// Event fired by client-side interactors for local UI feedback.
    /// </summary>
    public struct UIPointerEvent : IEvent
    {
        public IXRInteractor Interactor;
        public GameObject Target;
        public UIInteractionState State;
    }
}