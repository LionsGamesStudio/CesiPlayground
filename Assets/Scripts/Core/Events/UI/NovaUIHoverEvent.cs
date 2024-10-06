using Assets.Scripts.Core.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts.Core.Events.UI
{
    public class NovaUIHoverEvent : IEvent
    {
        public Nova.CoreBlock Element;
        public IXRHoverInteractor Interactor;
        public bool IsHovering;
    }
}
