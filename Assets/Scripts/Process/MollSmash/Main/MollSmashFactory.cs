using Assets.Scripts.Core.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Process.MollSmash.Main
{
    [CreateAssetMenu(fileName = "MollSmashFactory", menuName = "Game Factory/MollSmash")]
    public class MollSmashFactory : GameStrategyFactory
    {
        public override IGameStrategy CreateGameStrat(Game game)
        {
            return new MollSmashStrategy(game);
        }
    }

}
