using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Wave
{
    public interface IWave
    {
        /// <summary>
        /// Initialize the wave
        /// </summary>
        public void InitializeLevel();

        /// <summary>
        /// Reset the wave
        /// </summary>
        public void ResetLevel();
    }
}
