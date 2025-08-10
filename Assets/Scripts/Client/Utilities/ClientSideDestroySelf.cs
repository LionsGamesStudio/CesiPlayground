using UnityEngine;

namespace CesiPlayground.Client.Utilities
{
    /// <summary>
    /// Destroys the GameObject it is attached to after a specified lifetime.
    /// WARNING: This should ONLY be used for non-networked, purely visual effects
    /// like muzzle flashes or impact particles. Networked objects must be
    // destroyed by the ClientObjectFactory on command from the server.
    /// </summary>
    public class ClientSideDestroySelf : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The amount of time, in seconds, to wait after Start before destroying the GameObject.")]
        private float lifetime = 0.5f;
    
        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}