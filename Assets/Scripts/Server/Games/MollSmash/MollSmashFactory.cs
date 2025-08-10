using UnityEngine;
using CesiPlayground.Shared.Games;

namespace CesiPlayground.Server.Games.MollSmash
{
    [CreateAssetMenu(fileName = "MollSmashFactory", menuName = "Game Factory/MollSmash")]
    public class MollSmashFactory : GameStrategyFactory
    {
        /// <summary>
        /// Creates the server-side game strategy logic for MollSmash.
        /// </summary>
        /// <returns>An instance of the MollSmash game strategy.</returns>
        public override IGameStrategy CreateGameStrat()
        {
            return new MollSmashStrategy();
        }
    }
}