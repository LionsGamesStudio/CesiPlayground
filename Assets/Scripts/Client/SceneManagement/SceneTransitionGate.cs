using UnityEngine;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Data;

namespace CesiPlayground.Client.SceneManagement
{
    [RequireComponent(typeof(Collider))]
    public class SceneTransitionGate : MonoBehaviour
    {
        [Header("Transition Configuration")]
        [SerializeField] private DataScene sceneToLoad;
        [SerializeField] private DataScene sceneToUnload;
        [SerializeField] private string destinationGateName;
        [SerializeField] private GameObject gateVisual;

        [Header("Activation")]
        [SerializeField] private bool requiresLogin = true;

        private Collider _collider;
        private int _playerLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _playerLayer = LayerMask.NameToLayer("Player");
            if (requiresLogin)
            {
                _collider.enabled = false;
                gateVisual.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (requiresLogin)
            {
                GameEventSystem.Client.Register<PlayerDataLoadedEvent>(OnLoginSuccess);
            }
        }

        private void OnDisable()
        {
            if (requiresLogin)
            {
                GameEventSystem.Client.Unregister<PlayerDataLoadedEvent>(OnLoginSuccess);
            }
        }

        private void OnLoginSuccess(PlayerDataLoadedEvent e)
        {
            _collider.enabled = true;
            gateVisual.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[SceneTransitionGate] Triggered by {other.gameObject.name} on gate {gameObject.name}");
            if (other.gameObject.layer == _playerLayer)
            {
                GameEventSystem.Client.Raise(new TransitionToSceneEvent
                {
                    SceneToLoad = this.sceneToLoad,
                    SceneToUnload = this.sceneToUnload,
                    DestinationGateName = this.destinationGateName
                });
                _collider.enabled = false;
                gateVisual.SetActive(false);
            }
        }
    }
}