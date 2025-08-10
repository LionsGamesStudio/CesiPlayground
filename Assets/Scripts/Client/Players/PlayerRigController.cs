using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Data;
using CesiPlayground.Client.Network;
using CesiPlayground.Shared.Data.Players;
using TMPro;

namespace CesiPlayground.Client.Players
{
    /// <summary>
    /// The single, central controller for the persistent PlayerXR prefab.
    /// It ensures the player is a unique singleton, persists across scenes,
    /// and manages the state of its child XR Origin.
    /// </summary>
    public class PlayerRigController : Singleton<PlayerRigController>
    {
        [Header("Child References")]
        [Tooltip("A direct reference to the XR Origin GameObject, which is a child of this object.")]
        [SerializeField] private GameObject xrOriginObject;

        [Tooltip("A direct reference to the NetworkedPlayer component on the XR Origin.")]
        [SerializeField] private NetworkedPlayer networkedPlayer;

        [Tooltip("The CharacterController component on the XR Origin child.")]
        [SerializeField] private CharacterController characterController;

        [Header("Visuals (Optional)")]
        [Tooltip("A TextMeshPro component to display the player's name.")]
        [SerializeField] private TextMeshPro playerNameTag;

        private PlayerData _playerData;

        protected override void Awake()
        {
            base.Awake();
            if (this.enabled == false) return;
            
            DontDestroyOnLoad(this.gameObject);
            
            // Start in "Menu Mode": networking is disabled.
            SetNetworkingActive(false);
            if (playerNameTag != null) playerNameTag.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<PlayerDataLoadedEvent>(OnPlayerDataLoaded);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<PlayerDataLoadedEvent>(OnPlayerDataLoaded);
        }
        
        private void OnPlayerDataLoaded(PlayerDataLoadedEvent e)
        {
            _playerData = e.Data;
            gameObject.name = $"PlayerXR - {_playerData.PlayerName}";
            
            if (playerNameTag != null)
            {
                playerNameTag.text = _playerData.PlayerName;
                playerNameTag.gameObject.SetActive(true);
            }
            
            SetNetworkingActive(true);
        }
        
        public void SetNetworkingActive(bool isActive)
        {
            if (networkedPlayer != null)
            {
                networkedPlayer.IsNetworkingActive = isActive;
            }
            else
            {
                Debug.LogError("NetworkedPlayer component is not assigned on the PlayerRigController!");
            }
        }

        public void TeleportTo(Transform target)
        {
            if (target == null || xrOriginObject == null || characterController == null)
            {
                Debug.LogError("Teleport failed: A reference is missing in PlayerRigController.", this);
                return;
            }

            // 1. Set the networked player to not send updates during teleport.
            SetNetworkingActive(false);

            // 2. Disable the CharacterController to allow for direct transform manipulation.
            characterController.enabled = false;

            // 3. Move the parent container AND the child rig.
            //    This ensures the entire prefab moves as one unit.
            this.transform.position = target.position;
            this.transform.rotation = target.rotation;
            xrOriginObject.transform.localPosition = Vector3.zero;
            xrOriginObject.transform.localRotation = Quaternion.identity;

            // 4. Re-enable the CharacterController. This forces it to re-evaluate its
            //    surroundings at the new position and detect the ground.
            characterController.enabled = true;

            // 5. Re-enable networking to allow transform updates again.
            SetNetworkingActive(true);
        }
    }
}