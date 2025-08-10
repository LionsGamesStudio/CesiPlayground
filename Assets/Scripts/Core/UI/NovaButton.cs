using UnityEngine;
using UnityEngine.Events;
using Nova;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// NovaButton component that listens to UIPointerEvents and triggers UnityEvents.
    /// This is used for UI buttons in the Nova framework.
    /// </summary>
    [RequireComponent(typeof(CoreBlock), typeof(XRSimpleInteractable))]
    public class NovaButton : MonoBehaviour
    {
        [Header("Button Events")]
        [SerializeField] private UnityEvent _onHoverEnter;
        [SerializeField] private UnityEvent _onHoverExit;
        [SerializeField] private UnityEvent _onSelect;

        [Header("Button Settings")]
        [SerializeField] private UIBlock _background;
        [SerializeField] private Color _classicColor;
        [SerializeField] private Color _hoverColor;

        private void OnEnable()
        {
            GameEventSystem.Client.Register<UIPointerEvent>(OnUIPointerEvent);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<UIPointerEvent>(OnUIPointerEvent);
        }

        /// <summary>
        /// Handles UI pointer events from the client event bus.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnUIPointerEvent(UIPointerEvent e)
        {
            // Filter events that are not for this specific GameObject
            if (e.Target != this.gameObject) return;

            switch (e.State)
            {
                case UIInteractionState.HoverEnter:
                    _background.Color = _hoverColor;
                    _onHoverEnter?.Invoke();
                    break;
                case UIInteractionState.HoverExit:
                    _background.Color = _classicColor;
                    _onHoverExit?.Invoke();
                    break;
                case UIInteractionState.Select:
                    _onSelect?.Invoke();
                    break;
            }
        }
    }
}