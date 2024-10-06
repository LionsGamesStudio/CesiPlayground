using Assets.Scripts.Core.API;
using Assets.Scripts.Core.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Events.API
{
    public class OnAPIResponseEvent : IEvent
    {
        public APIRequestType RequestType;
        public string URL;
        public string JSON;
    }
}
