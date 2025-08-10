using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Nova;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace CesiPlayground.Client.Input
{
    /// <summary>
    /// Replicates the custom detection logic for Nova UI components.
    /// This component should be attached to the Near-Far interactor.
    /// It detects when the interactor is hovering over a Nova CoreBlock
    /// and raises UIPointerEvents for hover and select actions.
    /// </summary>
    [RequireComponent(typeof(NearFarInteractor))]
    public class NovaInteractorBridge : MonoBehaviour
    {
        private NearFarInteractor _nearFarInteractor;
        private CoreBlock _currentlyHoveredBlock = null;

        // A reusable list to prevent allocating memory every frame.
        private readonly List<IXRInteractable> _validTargets = new List<IXRInteractable>();

        private InputReader _parentInputReader;

        private void Awake()
        {
            _nearFarInteractor = GetComponent<NearFarInteractor>();

            _parentInputReader = GetComponentInParent<InputReader>();
            if (_parentInputReader == null)
            {
                Debug.LogError("NovaInteractorBridge could not find an InputReader component in its parent hierarchy!", this);
                enabled = false; // Disable this component if it can't function correctly.
            }
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<ClientPlayerActionEvent>(OnPlayerAction);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<ClientPlayerActionEvent>(OnPlayerAction);
        }

        private void Update()
        {
            DetectNovaHover();
        }

        private void OnPlayerAction(ClientPlayerActionEvent e)
        {
            if (!IsActionFromThisHand(e.Hand)) return;

            if (_currentlyHoveredBlock != null && e.ActionType == PlayerActionType.Trigger && e.State == InputActionState.Performed)
            {
                GameEventSystem.Client.Raise(new UIPointerEvent
                {
                    Interactor = _nearFarInteractor,
                    Target = _currentlyHoveredBlock.gameObject,
                    State = UIInteractionState.Select
                });
            }
        }

        private bool IsActionFromThisHand(XRHand hand)
        {
            if (_parentInputReader == null) return false;

            return hand == _parentInputReader.Hand;
        }

        private void DetectNovaHover()
        {
            // First, check if the interactor is hitting a standard Unity UI element.
            // If so, we should not interact with Nova UI behind it.
            if (_nearFarInteractor.TryGetCurrentUIRaycastResult(out _))
            {
                ClearHover();
                return;
            }

            // Get the list of all valid interactables the Near-Far interactor is currently touching or pointing at.
            _nearFarInteractor.GetValidTargets(_validTargets);

            CoreBlock hitBlock = null;
            if (_validTargets.Count > 0)
            {
                // The first valid target is the most likely candidate.
                IXRInteractable firstTarget = _validTargets[0];

                // We need to get the GameObject from the interactable.
                if (firstTarget is MonoBehaviour interactableMonoBehaviour)
                {
                    hitBlock = interactableMonoBehaviour.gameObject.GetComponent<CoreBlock>();
                }
            }

            if (hitBlock != _currentlyHoveredBlock)
            {
                if (_currentlyHoveredBlock != null)
                {
                    GameEventSystem.Client.Raise(new UIPointerEvent { Interactor = _nearFarInteractor, Target = _currentlyHoveredBlock.gameObject, State = UIInteractionState.HoverExit });
                }
                if (hitBlock != null)
                {
                    GameEventSystem.Client.Raise(new UIPointerEvent { Interactor = _nearFarInteractor, Target = hitBlock.gameObject, State = UIInteractionState.HoverEnter });
                }
                _currentlyHoveredBlock = hitBlock;
            }
        }

        private void ClearHover()
        {
            if (_currentlyHoveredBlock != null)
            {
                GameEventSystem.Client.Raise(new UIPointerEvent { Interactor = _nearFarInteractor, Target = _currentlyHoveredBlock.gameObject, State = UIInteractionState.HoverExit });
                _currentlyHoveredBlock = null;
            }
        }
    }
}