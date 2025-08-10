using UnityEngine;

namespace CesiPlayground.Client.LevelDesign
{
    /// <summary>
    /// A marker component used to locate spawn points in the scene.
    /// This component is used by the SpawnPointBaker editor script to collect spawn positions.
    /// It is not intended for runtime use.
    /// </summary>
    public class SpawnPointMarker : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 1f, 0.8f, 0.75f);
            Gizmos.DrawSphere(transform.position, 0.25f);
            Gizmos.DrawLine(transform.position, transform.position + transform.up * 1f);
        }
    }
}