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
    public class LoginUI : MonoBehaviour
    {
        public NovaInputText TextBlockPseudo;
        public NovaInputText TextBlockPassword;

        public void ValidateName()
        {
            string newName = TextBlockPseudo.Text;
            string password = TextBlockPassword.Text;

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
