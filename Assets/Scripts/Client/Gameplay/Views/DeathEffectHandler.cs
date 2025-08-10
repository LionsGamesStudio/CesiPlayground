using UnityEngine;

namespace CesiPlayground.Client.Gameplay.Views
{
    /// <summary>
    /// A client-side component that handles playing "death" effects (sound, particles)
    /// before an object is fully destroyed. It takes over the destruction process
    /// to ensure effects can finish playing.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class DeathEffectHandler : MonoBehaviour
    {
        [Tooltip("Optional sound to play on destruction.")]
        [SerializeField] private AudioClip destructionSound;
        
        [Tooltip("Optional particle effect prefab to instantiate on destruction.")]
        [SerializeField] private GameObject destructionParticles;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        /// <summary>
        /// Allows other scripts to configure the particle effect at runtime.
        /// </summary>
        public void SetDestructionEffect(GameObject effectPrefab)
        {
            destructionParticles = effectPrefab;
        }

        /// <summary>
        /// This is the main entry point, called by the ClientObjectFactory.
        /// It makes the object invisible, plays effects, and then destroys it.
        /// </summary>
        public void PlayEffectsAndDestroy()
        {
            // 1. Make the object visually and physically disappear immediately.
            SetChildrenActive(false);
            if(TryGetComponent<Collider>(out var col)) col.enabled = false;
            if(TryGetComponent<Renderer>(out var rend)) rend.enabled = false;

            // 2. Play effects at the object's last known position.
            if (destructionParticles != null)
            {
                // The effect will clean itself up if it has a ClientSideDestroySelf component.
                Instantiate(destructionParticles, transform.position, transform.rotation);
            }
            if (destructionSound != null)
            {
                // PlayClipAtPoint creates a temporary AudioSource that plays the clip
                // and then destroys itself, ensuring the sound isn't cut off.
                AudioSource.PlayClipAtPoint(destructionSound, transform.position);
            }
            
            // 3. The original object has served its visual purpose. Destroy it.
            Destroy(gameObject);
        }

        private void SetChildrenActive(bool isActive)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }
}