using UnityEngine;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A client-side controller for the parameters window.
    /// It listens to a specific player action to toggle its visibility.
    /// </summary>
    public class ParameterController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera xrCamera;
        [SerializeField] private GameObject parametersWindow;

        private void OnEnable()
        {
            GameEventSystem.Client.Register<ClientPlayerActionEvent>(OnPlayerAction);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<ClientPlayerActionEvent>(OnPlayerAction);
        }

        private void LateUpdate()
        {
            // Keep the window facing the player if it's active
            if (parametersWindow.activeSelf && xrCamera != null)
            {
                parametersWindow.transform.rotation = xrCamera.transform.rotation;
            }
        }
        
        /// <summary>
        /// Toggles the parameters window visibility when the specific action is performed.
        /// </summary>
        private void OnPlayerAction(ClientPlayerActionEvent e)
        {
            if (e.ActionType == PlayerActionType.Parameters && e.State == InputActionState.Performed)
            {
                parametersWindow.SetActive(!parametersWindow.activeSelf);
            }
        }
    }
}