using UnityEngine;
using CesiPlayground.Client.API;
using CesiPlayground.Core;
using CesiPlayground.Client.Network;
using CesiPlayground.Shared.Network;

namespace CesiPlayground.Client.UI.MenuUI
{
    public class RegisterUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NovaInputText _textBlockPseudo;
        [SerializeField] private NovaInputText _textBlockPassword;

        /// <summary>
        /// Called by a UI Button's OnClick() event.
        /// Asynchronously attempts to create a new player account.
        /// </summary>
        public void RegisterAccount()
        {
            var networkManager = ServiceLocator.Get<ClientNetworkManager>();
            if (networkManager == null)
            {
                Debug.LogError("ClientNetworkManager not found. Cannot send register request.");
                return;
            }

            string username = _textBlockPseudo.Text;
            string password = _textBlockPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.LogWarning("Username or password cannot be empty.");
                return;
            }
            
            Debug.Log($"[Client] Sending RegisterRequestMessage for user '{username}' to the game server...");

            var message = new RegisterRequestMessage 
            {
                Username = username,
                Password = password
            };
            
            networkManager.Send(message);
        }
    }
}