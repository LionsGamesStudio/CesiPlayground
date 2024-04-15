using UnityEngine;

namespace Assets.Scripts.Core.Game
{
    /// <summary>
    /// Permit to spawn a game where this object is place
    /// </summary>
    public class GameBeacon : MonoBehaviour
    {
        [SerializeField, Tooltip("Data of the game to spawn")] protected DataGame _gameData;

        public void Start()
        {
            if (_gameData != null)
            {
                SpawnGame();
                Destroy(this.gameObject);
            }
        }

        public void SpawnGame()
        {
            GameManager.Instance.LoadGame(_gameData, this.transform.position);
        }
    }
}
