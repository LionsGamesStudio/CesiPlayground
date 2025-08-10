using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CesiPlayground.Client.XR.Hands
{
    /// <summary>
    /// An extended XRGrabInteractable that allows for different attach transforms
    /// based on which hand is grabbing the object.
    /// </summary>
    public class XRGrabInteractableTwoAttach : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
    {
        [SerializeField] private Transform leftAttachTransform;
        [SerializeField] private Transform rightAttachTransform;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            // Use IXRInteractor interface for better compatibility
            if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor)
            {
                if (interactor.transform.CompareTag("Left Hand"))
                {
                    attachTransform = leftAttachTransform;
                }
                else if (interactor.transform.CompareTag("Right Hand"))
                {
                    attachTransform = rightAttachTransform;
                }
            }

            base.OnSelectEntered(args);
        }
    }
}