using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace CesiPlayground.Client.Input
{
    /// <summary>
    /// Bridges events from a specific XRBaseInteractor (Ray, Direct, etc.)
    /// and translates them into abstract UIPointerEvents on the CLIENT event bus.
    /// Used for standard UI and object interaction feedback.
    /// </summary>
    [RequireComponent(typeof(XRBaseInteractor))]
    public class InteractorBridge : MonoBehaviour
    {
        private XRBaseInteractor _interactor;

        private void Awake()
        {
            _interactor = GetComponent<XRBaseInteractor>();
        }

        private void OnEnable()
        {
            _interactor.hoverEntered.AddListener(OnHoverEntered);
            _interactor.hoverExited.AddListener(OnHoverExited);
            _interactor.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            _interactor.hoverEntered.RemoveListener(OnHoverEntered);
            _interactor.hoverExited.RemoveListener(OnHoverExited);
            _interactor.selectEntered.RemoveListener(OnSelectEntered);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            // The interactorObject property is now an interface IXRInteractor, which is great.
            // Our UIPointerEvent is designed to accept this interface.
            GameEventSystem.Client.Raise(new UIPointerEvent { Interactor = args.interactorObject, Target = args.interactableObject.transform.gameObject, State = UIInteractionState.HoverEnter });
        }
        
        private void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactableObject == null) return;
            GameEventSystem.Client.Raise(new UIPointerEvent { Interactor = args.interactorObject, Target = args.interactableObject.transform.gameObject, State = UIInteractionState.HoverExit });
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            GameEventSystem.Client.Raise(new UIPointerEvent { Interactor = args.interactorObject, Target = args.interactableObject.transform.gameObject, State = UIInteractionState.Select });
        }
    }
}