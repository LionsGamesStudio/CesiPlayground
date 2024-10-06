using Assets.Scripts.Core.API;
using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Events.API
{
    public class OnAPIRequestEvent : IEvent
    {
        public string RequesterName;
        public string Method;
        public Blackboard Data;
    }
}
