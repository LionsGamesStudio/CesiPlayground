using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.Events
{
    public class OnWaveFinished : IEvent
    {
        public Game Game;
    }
}
