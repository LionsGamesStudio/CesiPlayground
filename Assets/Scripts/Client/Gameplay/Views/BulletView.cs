using UnityEngine;
using CesiPlayground.Client.Network;

namespace CesiPlayground.Client.Gameplay.Views
{
    /// <summary>
    /// The purely visual representation of a bullet on the client.
    /// It has no scoring logic, but it moves based on an initial velocity
    /// provided by the server and handles its own visual impact effects.
    /// </summary>
    [RequireComponent(typeof(NetworkedObjectView), typeof(Rigidbody), typeof(Collider))]
    public class BulletView : MonoBehaviour
    {
        [Tooltip("Visual effect to instantiate on impact.")]
        [SerializeField] private GameObject impactEffectPrefab;

        private Rigidbody _rigidbody;
        private bool _hasImpacted = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            // Use continuous collision detection for fast-moving objects.
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        /// <summary>
        /// Called by the ClientObjectFactory after this bullet is spawned.
        /// It gives the bullet its initial momentum.
        /// </summary>
        /// <param name="velocity">The initial velocity from the server.</param>
        public void Initialize(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }

        /// <summary>
        /// Handles visual effects when the bullet collides with something.
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            // Ensure this only runs once.
            if (_hasImpacted) return;
            _hasImpacted = true;

            // 1. Instantiate a visual impact effect at the point of collision.
            if (impactEffectPrefab != null)
            {
                // Instantiate the effect and clean it up after a few seconds.
                GameObject effect = Instantiate(impactEffectPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
                Destroy(effect, 2f);
            }
            
            // 2. IMPORTANT: We DO NOT destroy the bullet here.
            // The server will decide when the bullet should be destroyed and send a
            // DestroyNetworkedObjectMessage.
            
            // 3. To prevent weird physics (like bouncing), we neutralize the bullet visually and physically.
            // This makes it "disappear" for the player while it waits for the server's command to be destroyed.
            if(TryGetComponent<Renderer>(out var renderer)) renderer.enabled = false;
            if(TryGetComponent<Collider>(out var collider)) collider.enabled = false;
            _rigidbody.isKinematic = true;
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}