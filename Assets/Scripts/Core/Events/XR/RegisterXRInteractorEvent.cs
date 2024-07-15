using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts.Core.Events.XR
{
    public class RegisterXRInteractorEvent : IEvent
    {
        public XRHand Hand;
        public XRInteractorType InteractorType;
        public Assets.Scripts.Core.XR.XRInteractorEvent InteractorEvent;
        public UnityAction<BaseInteractionEventArgs> Callback;
    }
}
