using UnityEngine;
using CesiPlayground.Client.Network;

namespace CesiPlayground.Client.Gameplay.Views
{
    /// <summary>
    /// The client-side visual representation of a target.
    /// Its primary role is to configure the visual effects for its destruction,
    /// which are then executed by a DeathEffectHandler.
    /// </summary>
    [RequireComponent(typeof(Collider), typeof(NetworkedObjectView))]
    public class TargetView : MonoBehaviour
    {
        [Tooltip("The visual effect prefab (e.g., explosion particles) to instantiate when this target is destroyed.")]
        [SerializeField] private GameObject effectOnDestroy;

        private void Awake()
        {
            // This ensures that a DeathEffectHandler is always present on this target.
            if (!TryGetComponent<DeathEffectHandler>(out var deathHandler))
            {
                deathHandler = gameObject.AddComponent<DeathEffectHandler>();
            }

            // Configure the handler with the specific effect for this type of target.
            if (effectOnDestroy != null)
            {
                deathHandler.SetDestructionEffect(effectOnDestroy);
            }
        }
    }
}