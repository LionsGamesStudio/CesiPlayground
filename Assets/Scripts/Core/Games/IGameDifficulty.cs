using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Games
{
    public interface IGameDifficulty
    {
        public void SetDifficulty(GameDifficulty difficulty);
    }
}
