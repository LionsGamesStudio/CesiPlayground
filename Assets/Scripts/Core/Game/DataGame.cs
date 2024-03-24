using UnityEngine;

namespace Assets.Scripts.Core.Game
{

    [CreateAssetMenu(fileName = "New Game", menuName = "ScriptableObjects/DataGame", order = 1)]
    public class DataGame : ScriptableObject
    {
        public string GameId;
        public string GameName;
        public string GameDescription;
        public string GameScoreboardFileName;
        public Sprite GameImage;
        public GameObject GamePrefab;

        [Header("Gameplay")]
        public GameStrategyFactory GameStrategyFactory;
    }
}
