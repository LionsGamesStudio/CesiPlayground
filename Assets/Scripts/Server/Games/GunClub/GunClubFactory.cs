using System.Collections.Generic;
using CesiPlayground.Shared.Games;
using CesiPlayground.Shared.Games.GunClub;
using UnityEngine;

namespace CesiPlayground.Server.Games.GunClub
{
    [CreateAssetMenu(fileName = "GunClubFactory", menuName = "Game Factory/GunClub")]
    public class GunClubFactory : GameStrategyFactory
    {
        public List<DataGunClubWave> Waves;

        /// <summary>
        /// Creates a new, uninitialized instance of the GunClubStrategy.
        /// </summary>
        public override IGameStrategy CreateGameStrat()
        {
            return new GunClubStrategy();
        }
    }
}