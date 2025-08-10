using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CesiPlayground.Shared.Events.Input;
using CesiPlayground.Client.Input;

namespace CesiPlayground.Client.Players.Objects
{
    [RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
    public abstract class GrabbableObject : MonoBehaviour
    {
        protected UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable Grabbable;
        protected XRHand CurrentHand;
        protected bool IsHeld => Grabbable.isSelected;


        protected virtual void Awake()
        {
            Grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }

        protected virtual void OnEnable()
        {
            Grabbable.selectEntered.AddListener(OnGrabbed);
            Grabbable.selectExited.AddListener(OnReleased);
        }

        protected virtual void OnDisable()
        {
            Grabbable.selectEntered.RemoveListener(OnGrabbed);
            Grabbable.selectExited.RemoveListener(OnReleased);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            if (args.interactorObject.transform.TryGetComponent<InputReader>(out var reader))
            {
                CurrentHand = reader.Hand;
            }
        }
        
        private void OnReleased(SelectExitEventArgs args)
        {
            // Logic for when the object is released.
        }
    }
}