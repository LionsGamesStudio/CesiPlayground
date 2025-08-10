using UnityEngine;


namespace CesiPlayground.Client.Gameplay.VR
{
    /// <summary>
    /// A client-side component that visually attaches a grabbable object
    /// to a specific hand socket upon start. This is for setting up the player's initial state.
    /// </summary>
    public class AutoAttachView : MonoBehaviour
    {
        [Tooltip("The XRGrabInteractable of the object to attach (e.g., a pistol).")]
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable objectToAttach;
        
        [Tooltip("The socket interactor on the hand where the object should be attached.")]
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor handSocket;

        private void Start()
        {
            if (objectToAttach == null || handSocket == null)
            {
                Debug.LogError("AutoAttachView is missing references.");
                return;
            }
            
            // Ensure the object is not already attached
            handSocket.interactionManager.SelectEnter((UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor)handSocket, (UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)objectToAttach);
        }
    }
}