using UnityEngine;

namespace CesiPlayground.Shared.Data.Games
{
    [CreateAssetMenu(fileName = "New Game", menuName = "ScriptableObjects/DataGame", order = 1)]
    public class DataGame : ScriptableObject
    {
        [Header("Shared Data (Server & Client)")]
        public string GameId;
        public string GameName;
        [TextArea] public string GameDescription;

#if UNITY_EDITOR || UNITY_CLIENT
        [Header("Client-Only Data")]
        [Tooltip("Image to represent the game in the UI.")] 
        public Sprite GameImage;
        
        [Tooltip("The client-side visual prefab of the game stand/arena (GameView).")] 
        public GameObject GamePrefab;
#endif
    }
}