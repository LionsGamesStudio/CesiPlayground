using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Network;
using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Client.Network
{
    public struct PlayerTransformCorrectionEvent : IEvent
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public class NetworkedPlayer : MonoBehaviour
    {
        [Tooltip("How often to send transform updates to the server, in seconds.")]
        [SerializeField] private float sendInterval = 0.05f;

        /// <summary>
        /// A public flag controlled by the PlayerRigController. When false, this script
        /// will not send any transform updates to the server.
        /// </summary>
        public bool IsNetworkingActive { get; set; } = false;

        private ClientNetworkManager _networkManager;
        private float _timer;
        private Transform _xrOriginTransform;

        private void Start()
        {
            _xrOriginTransform = this.transform;
            _networkManager = ServiceLocator.Get<ClientNetworkManager>();
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<PlayerTransformCorrectionEvent>(OnTransformCorrection);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<PlayerTransformCorrectionEvent>(OnTransformCorrection);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= sendInterval)
            {
                _timer = 0f;

                // Only send the transform if networking is explicitly activated and not teleporting.
                if (IsNetworkingActive)
                {
                    SendTransformToServer();
                }
            }
        }

        private void SendTransformToServer()
        {
            if (_networkManager == null) return;

            var message = new PlayerTransformUpdateMessage
            {
                Position = _xrOriginTransform.position,
                Rotation = _xrOriginTransform.rotation
            };
            _networkManager.Send(message);
        }
        
        private void OnTransformCorrection(PlayerTransformCorrectionEvent e)
        {
            if (TryGetComponent<CharacterController>(out var controller))
            {
                controller.enabled = false;
            }

            _xrOriginTransform.position = e.Position;
            _xrOriginTransform.rotation = e.Rotation;

            if (controller != null)
            {
                controller.enabled = true;
            }
            
            Debug.Log("[Client] Applied transform correction from server.");
        }
    }
}