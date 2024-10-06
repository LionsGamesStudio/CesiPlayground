using Assets.Scripts.Core.Games;
using UnityEngine;

namespace Assets.Scripts.Process.Games
{
    /// <summary>
    /// Permit to spawn a game where this object is place
    /// </summary>
    public class GameBeacon : MonoBehaviour
    {
        [SerializeField, Tooltip("Data of the game to spawn")] protected DataGame _gameData;

        private GameObject _ghost;

        public void OnEnable()
        {
            
        }

        public void Start()
        {
            if (_gameData != null)
            {
                SpawnGame();
                Destroy(this.gameObject);
            }
        }

        public void OnDrawGizmos()
        {
            if(_ghost == null)
            {
                _ghost = Instantiate(_gameData.GamePrefab, this.transform.position, Quaternion.identity);
            }
            _ghost.SetActive(false);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(this.transform.position, _ghost.GetComponent<BoxCollider>().size);
        }

        public void SpawnGame()
        {
            GameManager.Instance.LoadGame(_gameData, this.transform.position);
        }

        public void OnDestroy()
        {
            if (_ghost != null)
            {
                Destroy(_ghost);
            }
        }
    }
}
