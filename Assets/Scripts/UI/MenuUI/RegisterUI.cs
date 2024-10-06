using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.API;
using Assets.Scripts.Core.Players;
using Assets.Scripts.Core.Storing;
using Assets.Scripts.Core.UI;
using Nova;
using Nova.TMP;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI
{
    public class RegisterUI : MonoBehaviour
    {
        public NovaInputText TextBlockPseudo;
        public NovaInputText TextBlockPassword;

        public void ValidateName()
        {
            string newName = TextBlockPseudo.Text;
            string password = TextBlockPassword.Text;

            OnAPIRequestEvent apiRequestEvent = new OnAPIRequestEvent();
            apiRequestEvent.RequesterName = "PlayersRequests";
            apiRequestEvent.Method = "CreatePlayer";
            apiRequestEvent.Data = new Blackboard();

            apiRequestEvent.Data.Set("pseudo", newName);
            apiRequestEvent.Data.Set("password", password);

            EventBus<OnAPIRequestEvent>.Raise(apiRequestEvent);
        }
    }
}
