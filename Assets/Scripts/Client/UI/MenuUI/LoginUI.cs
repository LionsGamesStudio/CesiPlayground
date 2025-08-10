using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Client.Network;
using CesiPlayground.Shared.Network;

namespace CesiPlayground.Client.UI.MenuUI
{
    [RequireComponent(typeof(NovaButton))]
    public class LoginUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NovaInputText _textBlockPseudo;
        [SerializeField] private NovaInputText _textBlockPassword;

        public void LogIn()
        {
            var networkManager = ServiceLocator.Get<ClientNetworkManager>();
            if (networkManager == null) return;

            string username = _textBlockPseudo.Text;
            string password = _textBlockPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

            var message = new LoginRequestMessage
            {
                Username = username,
                Password = password
            };

            networkManager.Send(message);
        }
    }
}