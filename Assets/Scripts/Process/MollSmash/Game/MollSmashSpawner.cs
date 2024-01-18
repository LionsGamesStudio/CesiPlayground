using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Process.GunClub.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.MollSmash.Game
{
    public class MollSmashSpawner : Spawner
    {
        [SerializeField] private Assets.Scripts.Core.Game.Game _game;

        private void Start()
        {
            StartCoroutine(InitSpawner());
        }

        private IEnumerator InitSpawner()
        {
            // Wait for the game to be ready
            yield return new WaitUntil(() => _game != null);
            yield return new WaitUntil(() => _game.GameStrategy != null);

            if (_game.GameStrategy.GetType() != typeof(MollSmashStrategy))
            {
                Debug.LogError("Wrong game");
                yield break;
            }

            ((MollSmashStrategy)_game.GameStrategy).SetSpawner(this);
        }
    }
}
