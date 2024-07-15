using Assets.Scripts.Core.Events.Interfaces;
using Assets.Scripts.Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.Events
{
    /// <summary>
    /// Event sent when a wave is finished
    /// </summary>
    public class OnWaveFinished : IEvent
    {
        public Game Game;
    }
}
