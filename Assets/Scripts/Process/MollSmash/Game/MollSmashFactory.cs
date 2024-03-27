using Assets.Scripts.Core.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Process.MollSmash.Game
{
    [CreateAssetMenu(fileName = "MollSmashFactory", menuName = "Game Factory/MollSmash")]
    public class MollSmashFactory : GameStrategyFactory
    {
        public override IGameStrategy CreateGameStrat(Core.Game.Game game)
        {
            return new MollSmashStrategy(game);
        }
    }

}
