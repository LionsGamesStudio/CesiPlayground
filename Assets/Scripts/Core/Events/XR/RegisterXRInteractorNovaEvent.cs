using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Events.UI;
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
    public class RegisterXRInteractorNovaEvent : IEvent
    {
        public XRHand Hand;
        public XRInteractorEvent XRInteractorEvent;
        public UnityAction<NovaUIHoverEvent> Callback;
    }
}
