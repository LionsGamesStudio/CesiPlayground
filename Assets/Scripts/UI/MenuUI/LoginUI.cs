using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.API;
using Assets.Scripts.Core.Storing;
using Assets.Scripts.Core.UI;
using Nova.TMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI
{
    [RequireComponent(typeof(NovaButton))]
    public class LoginUI : MonoBehaviour
    {
        [Header("Input Texts")]
        [SerializeField] private NovaInputText _textBlockPseudo;
        [SerializeField] private NovaInputText _textBlockPassword;

        private NovaButton _button;

        private void Awake()
        {
            _button = GetComponent<NovaButton>();
            if (_button == null)
            {
                Debug.LogError("LoginUI requires a NovaButton component to function.");
            }

            bool hasRegisterLogIn = false;

            foreach (var item in _button.OnSelect.ListSubscribers())
            {
                Debug.Log(item);
            }
        }

        /// <summary>
        /// Check if the player exists and if the password is correct and log in
        /// </summary>
        public void LogIn()
        {
            string newName = _textBlockPseudo.Text;
            string password = _textBlockPassword.Text;

            OnAPIRequestEvent apiRequestEvent = new OnAPIRequestEvent();
            apiRequestEvent.RequesterName = "PlayersRequests";
            apiRequestEvent.Method = "GetPlayer";
            apiRequestEvent.Data = new Blackboard();

            apiRequestEvent.Data.Set("pseudo", newName);
            apiRequestEvent.Data.Set("password", password);

            EventBus<OnAPIRequestEvent>.Raise(apiRequestEvent);
        }

    }
}
