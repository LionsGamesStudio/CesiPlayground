using UnityEngine;
using CesiPlayground.Client.Players.Objects;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;

namespace CesiPlayground.Client.Gameplay.Views
{
    public class HammerView : GrabbableObject
    {
        [Header("Hit Zone References")]
        [SerializeField] private Collider RightZoneCollider;
        [SerializeField] private Collider LeftZoneCollider;

        private void Start()
        {
            if (RightZoneCollider != null)
            {
                RightZoneCollider.isTrigger = true;
                var rightHitbox = RightZoneCollider.gameObject.AddComponent<HammerHitbox>();
                rightHitbox.Initialize(this);
            }
            if (LeftZoneCollider != null)
            {
                LeftZoneCollider.isTrigger = true;
                var leftHitbox = LeftZoneCollider.gameObject.AddComponent<HammerHitbox>();
                leftHitbox.Initialize(this);
            }
        }

        public void ReportHit()
        {
            if (!IsHeld) return;
            
            GameEventSystem.Client.Raise(new ClientPlayerActionEvent
            {
                ActionType = PlayerActionType.MeleeHit,
                State = InputActionState.Performed,
                Hand = this.CurrentHand
            });
        }
    }

    /// <summary>
    /// A helper component that now uses layers to detect what it hits.
    /// </summary>
    public class HammerHitbox : MonoBehaviour
    {
        private HammerView _parentHammer;
        private int _targetLayer; // Cache the layer integer.

        public void Initialize(HammerView parent)
        {
            _parentHammer = parent;
            _targetLayer = LayerMask.NameToLayer("Target");
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the other collider's layer matches our cached 'Target' layer.
            if (other.gameObject.layer == _targetLayer)
            {
                _parentHammer?.ReportHit();
            }
        }
    }
}