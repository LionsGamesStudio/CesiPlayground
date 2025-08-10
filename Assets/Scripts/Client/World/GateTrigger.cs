using UnityEngine;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.World;

namespace CesiPlayground.Client.World
{
    [RequireComponent(typeof(Collider))]
    public class GateTrigger : MonoBehaviour
    {
        [Tooltip("A unique name for this gate, must match the one on the server.")]
        [SerializeField] private string gateName;
        
        public string GateName => gateName;

        private int _playerLayer;

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _playerLayer)
            {
                Debug.Log($"[Client] Player entered gate '{gateName}'. Firing CLIENT event.");
                
                // Raise the CLIENT-specific event.
                GameEventSystem.Client.Raise(new ClientPlayerUsedGateEvent
                {
                    GateName = this.gateName
                });
            }
        }
    }
}