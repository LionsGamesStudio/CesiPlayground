using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.Events.XR
{
    public class RegisterXRInputEvent : IEvent
    {
        public XRHand Hand;
        public XRInputType InputType;
        public Action<InputAction.CallbackContext> InputAction;
    }
}
