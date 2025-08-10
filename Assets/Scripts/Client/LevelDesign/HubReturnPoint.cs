using UnityEngine;

namespace CesiPlayground.Client.LevelDesign
{
    /// <summary>
    /// A marker component to identify the single, primary return point for players
    /// in the Hub scene.
    /// </summary>
    public class HubReturnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.5f, 1f, 0.75f); // Blue
            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
        }
    }
}