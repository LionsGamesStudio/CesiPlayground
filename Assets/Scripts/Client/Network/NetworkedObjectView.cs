using UnityEngine;

namespace CesiPlayground.Client.Network
{
    /// <summary>
    /// A crucial component that links a client-side GameObject to its
    /// server-side logical entity via a unique NetworkId.
    /// </summary>
    public class NetworkedObjectView : MonoBehaviour
    {
        public int NetworkId { get; private set; }

        public void Initialize(int networkId)
        {
            NetworkId = networkId;
        }
    }
}