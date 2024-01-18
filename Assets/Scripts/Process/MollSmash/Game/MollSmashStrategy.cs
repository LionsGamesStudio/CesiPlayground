using Assets.Scripts.Core.Game;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Process.GunClub.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.MollSmash.Game
{
    public class MollSmashStrategy : IGameStrategy
    {
        private Assets.Scripts.Core.Game.Game _game;
        private Spawner _spawner;
        private int _life = 3;

        private enum MollSmashDifficulty
        {
            Easy,
            Medium,
            Hard
        }


        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public void ResetGame()
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }

        public void SetSpawner(Spawner spawner)
        {
            _spawner = spawner;
        }
    }
}
