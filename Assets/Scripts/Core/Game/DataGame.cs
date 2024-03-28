using UnityEngine;

namespace Assets.Scripts.Core.Game
{

    [CreateAssetMenu(fileName = "New Game", menuName = "ScriptableObjects/DataGame", order = 1)]
    public class DataGame : ScriptableObject
    {
        [Header("Game Data")]
        [Tooltip("Unique Id to identify the game")] public string GameId;
        [Tooltip("Name to recognize easily the game")] public string GameName;
        [Tooltip("Description of the game (maybe tuto)")] public string GameDescription;
        [Tooltip("Name of the file (.json actually) containing the scores of the game")] public string GameScoreboardFileName;
        [Tooltip("Image to represent the game")] public Sprite GameImage;
        [Tooltip("Prefab of the game")] public GameObject GamePrefab;

        [Header("Gameplay")]
        public GameStrategyFactory GameStrategyFactory;
    }
}
