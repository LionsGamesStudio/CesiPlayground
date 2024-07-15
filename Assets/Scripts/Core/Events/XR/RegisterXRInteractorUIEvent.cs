using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Assets.Scripts.Core.Events.XR
{
    public class RegisterXRInteractorUIEvent : IEvent
    {
        public XRHand Hand;
        public XRInteractorEvent XRInteractorEvent;
        public UnityAction<UIHoverEventArgs> Callback;
    }
}
