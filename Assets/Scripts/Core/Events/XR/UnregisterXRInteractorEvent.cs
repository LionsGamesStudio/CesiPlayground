using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Events.XR
{
    public class UnregisterXRInteractorEvent : IEvent
    {
        public int Id;
        public XRHand Hand;
        public XRInteractorType InteractorType;
        public XRInteractorEvent InteractorEvent;
    }
}
