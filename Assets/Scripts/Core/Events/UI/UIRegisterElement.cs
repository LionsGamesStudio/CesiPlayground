using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Games;
using Assets.Scripts.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Events.UI
{
    public class UIRegisterElement : IEvent
    {
        public int Id;
        public UIBaseElement Element;
    }
}
